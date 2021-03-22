using RestauranteDominio;
using RestauranteDominio.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranteRepositorios.Services
{
    public class PedidoService
    {
        private readonly RestauranteContexto _contexto;
        private readonly ProdutoService _produtoService;

        public PedidoService(RestauranteContexto contexto, ProdutoService produtoService)
        {
            _contexto = contexto;
            _produtoService = produtoService;
        }

        public async Task AdicionarPedido(AdicionarNovoModel model, int comandaId)
        {
            model.Validar();

            var produto = await _produtoService.ObterProduto(model.ProdutoId);
            _ = produto ?? throw new Exception("Produto inexistente");
            var valorTotalPedido = produto.ValorProduto * model.QtdeProduto;

            var comanda = _contexto.Comanda
                .Where(c => comandaId == c.ComandaId)
                .FirstOrDefault();
            _ = comanda ?? throw new Exception("Comanda inexistente");

            if (model.ProdutoId == 1) //item rodizio
                throw new Exception("Esse produto é inválido.");

            if (!QtdeValida(model.ProdutoId, model.QtdeProduto, model.ComandaId))
                throw new Exception("Quantidade de items escolhido invalida! ");

            var pedido = new Pedido()
            {
                ProdutoId = model.ProdutoId,
                ComandaId = comandaId,
                QtdeProduto = model.QtdeProduto,
                ValorPedido = valorTotalPedido,
                StatusPedidoEnum = StatusPedidoEnum.Realizado
            };

            _contexto.Add(pedido);

            if (produto.ValorProduto > 0)
            {
                comanda.Valor += pedido.ValorPedido;
            }

            await _contexto.SaveChangesAsync();
        }

        public async Task AtualizarPedido(AtualizarModel model)
        {
            if (model.PedidoId == 1)
                throw new Exception("O rodizio não pode ser editado!");

            var produto = await _produtoService.ObterProduto(model.ProdutoId);
            _ = produto ?? throw new Exception("Produto inexistente");
            var valorTotalPedido = produto.ValorProduto * model.QtdeProduto;

            var comanda = _contexto.Comanda
                .Where(c => model.ComandaId == c.ComandaId)
                .FirstOrDefault();
            _ = comanda ?? throw new Exception("Comanda inexistente");

            var pedido = _contexto.Pedido
                    .Where(p => p.ComandaId == model.ComandaId && p.PedidoId == model.PedidoId && p.StatusPedidoEnum != StatusPedidoEnum.Cancelado)
                    .OrderBy(p => p.PedidoId)
                    .LastOrDefault();

            _ = pedido ?? throw new Exception("Pedido inválido. Só é possivel atualizar o ultimo pedido realizado e que não esteja cancelado!");

            if (!QtdeValida(pedido.ProdutoId, model.QtdeProduto, pedido.ComandaId))
                throw new Exception("Quantidade de items escolhido invalida! ");

            pedido.QtdeProduto = model.QtdeProduto;

            if (produto.ValorProduto > 0)
            {
                comanda.Valor -= pedido.ValorPedido;

                comanda.Valor += valorTotalPedido;
            }

            pedido.ValorPedido = valorTotalPedido;

            await _contexto.SaveChangesAsync();
        }

        public async Task RemoverPedido(int comandaId, int pedidoId)
        {
            if(pedidoId == 1)
                throw new Exception("O rodizio não pode ser cancelado da sua comanda!");

            var pedido = _contexto.Pedido
                        .Where(p => p.ComandaId == comandaId && p.PedidoId == pedidoId && p.StatusPedidoEnum != StatusPedidoEnum.Cancelado)
                        .OrderBy(p => p.PedidoId)
                        .LastOrDefault();

            _ = pedido ?? throw new Exception("Pedido inválido. Só é possivel cancelar o ultimo pedido realizado e que não esteja cancelado!");

            var comanda = _contexto.Comanda
                .Where(c => comandaId == c.ComandaId)
                .FirstOrDefault();
            _ = comanda ?? throw new Exception("Comanda inexistente");

            pedido.StatusPedidoEnum = StatusPedidoEnum.Cancelado;

            if (pedido.ValorPedido > 0)
            {
                comanda.Valor -= pedido.ValorPedido;
            }

            await _contexto.SaveChangesAsync();
        }

        //CALCULA A QUANTIDADE VALIDA DE ITEM POR PEDIDO
        public bool QtdeValida(int prodId, int qtdeEscolhida, int comandaId)
        {
            int pessoasMesa = _contexto.Comanda
                        .Where(c => c.ComandaId == comandaId)
                        .Select(c => c.QtdePessoasMesa).FirstOrDefault();
            int qtdePermitida = _contexto.Produto
                        .Where(p => p.ProdutoId == prodId)
                        .Select(p => p.QtdePermitida).FirstOrDefault();
            if(qtdeEscolhida > (qtdePermitida * pessoasMesa) || qtdeEscolhida < 1)
            {
                return false;
            }

            return true;
        }
    }
}
