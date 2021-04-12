using Microsoft.EntityFrameworkCore;
using RestauranteDominio;
using RestauranteDominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranteRepositorios.Services
{
    public class PedidoService
    {
        const int PRODUTO_RODIZIO = 1;

        private readonly RestauranteContexto _contexto;
        private readonly ProdutoService _produtoService;

        public PedidoService(RestauranteContexto contexto, ProdutoService produtoService)
        {
            _contexto = contexto;
            _produtoService = produtoService;
        }

        public async Task<List<BuscarModel>> Buscar(int comandaId)
        {
            return await _contexto.Pedido
                .Where(p => p.ComandaId == comandaId)
                .Select(p => new BuscarModel
                {
                    ComandaId = p.ComandaId,
                    PedidoId = p.PedidoId,
                    ProdutoId = p.ProdutoId,
                    Produto = new ListarModel
                    {
                        ProdutoId = p.ProdutoId,
                        NomeProduto = p.Produto.NomeProduto,
                        ValorProduto = p.Produto.ValorProduto,
                        QtdePermitida = p.Produto.QtdePermitida,
                    },
                    QtdeProduto = p.QtdeProduto,
                    StatusPedidoEnum = p.StatusPedidoEnum,
                    ValorPedido = p.ValorPedido
                }).ToListAsync();
        }
        public async Task<int> AdicionarPedido(AdicionarNovoModel model, int comandaId)
        {
            model.Validar();

            var produto = await _produtoService.ObterProduto(model.ProdutoId);
            _ = produto ?? throw new Exception("Produto inexistente");

            //Calcular valor total pedido
            var valorTotalPedido = produto.ValorProduto * model.QtdeProduto;

            var comanda = await _contexto
                .Comanda
                .Where(c => comandaId == c.ComandaId)
                .FirstOrDefaultAsync();

            _ = comanda ?? throw new Exception("Comanda inexistente");

            if (comanda.ComandaPaga)
                throw new Exception("Comanda incorreta! Essa comanda já encontra-se paga");

            if (model.ProdutoId == PRODUTO_RODIZIO) //item rodizio
                throw new Exception("Esse produto é inválido.");

            //Verificar a quantidade de items conforme pedido do cliente
            if (!QtdeValida(comanda.QtdePessoasMesa, model.QtdeProduto, produto.QtdePermitida))
                throw new Exception("Quantidade de items escolhido invalida! ");

            var pedido = new Pedido()
            {
                ProdutoId = model.ProdutoId,
                ComandaId = comandaId,
                QtdeProduto = model.QtdeProduto,
                ValorPedido = valorTotalPedido,
                StatusPedidoEnum = StatusPedidoEnum.Realizado,               
            };

            _contexto.Add(pedido);

            if (produto.ValorProduto > 0)
            {
                comanda.Valor += pedido.ValorPedido;
            }

            await _contexto.SaveChangesAsync();

            return pedido.PedidoId;
        }

        public async Task<BuscarModel> AtualizarPedido(AtualizarModel model)
        {
            if (model.ProdutoId == PRODUTO_RODIZIO)
                throw new Exception("O rodizio não pode ser editado!");

            var produto = await _produtoService.ObterProduto(model.ProdutoId);
            _ = produto ?? throw new Exception("Produto inexistente");
            var valorTotalPedido = produto.ValorProduto * model.QtdeProduto;

            var comanda = await _contexto
                .Comanda
                .Where(c => model.ComandaId == c.ComandaId)
                .Include(c => c.Pedidos)
                .FirstOrDefaultAsync();
            _ = comanda ?? throw new Exception("Comanda inexistente");

            var pedido = comanda.Pedidos
                .Where(p => p.PedidoId == model.PedidoId && p.StatusPedidoEnum != StatusPedidoEnum.Cancelado)
                .FirstOrDefault();
            _ = pedido ?? throw new Exception("Pedido inválido.");

            var ultimoPedido = comanda.Pedidos
                .OrderBy(p => p.PedidoId)
                .LastOrDefault();

            if (ultimoPedido != pedido)
                throw new Exception("Só é possivel atualizar o ultimo pedido realizado.");

            if (!QtdeValida(comanda.QtdePessoasMesa, model.QtdeProduto, produto.QtdePermitida))
                throw new Exception("Quantidade de items escolhido invalida! ");

            pedido.QtdeProduto = model.QtdeProduto;

            if (produto.ValorProduto > 0)
            {
                comanda.Valor -= pedido.ValorPedido;

                comanda.Valor += valorTotalPedido;
            }

            pedido.ValorPedido = valorTotalPedido;

            await _contexto.SaveChangesAsync();

            return new BuscarModel
            { 
                QtdeProduto = pedido.QtdeProduto, 
                ValorPedido = pedido.ValorPedido,
                PedidoId = pedido.PedidoId,
                ComandaId = pedido.ComandaId
            };
        }

        public async Task<StatusPedidoEnum> RemoverPedido(int comandaId, int pedidoId)
        {
            var comanda = await _contexto
                .Comanda
                .Where(c => comandaId == c.ComandaId)
                .Include(c => c.Pedidos)
                .FirstOrDefaultAsync();
            _ = comanda ?? throw new Exception("Comanda inexistente");

            var pedido = comanda.Pedidos
                .Where(p => p.PedidoId == pedidoId && p.StatusPedidoEnum != StatusPedidoEnum.Cancelado)
                .FirstOrDefault();

            _ = pedido ?? throw new Exception("Pedido inválido. Só é possivel cancelar o ultimo pedido realizado e que não esteja cancelado!");

            if(pedido.ProdutoId == PRODUTO_RODIZIO)
                throw new Exception("O rodizio não pode ser cancelado!");

            var ultimoPedido = comanda.Pedidos
                .OrderBy(p => p.PedidoId)
                .LastOrDefault();

            if (ultimoPedido != pedido)
                throw new Exception("Só é possivel atualizar o ultimo pedido realizado.");

            pedido.StatusPedidoEnum = StatusPedidoEnum.Cancelado;

            if (pedido.ValorPedido > 0)
            {
                comanda.Valor -= pedido.ValorPedido;
            }

            await _contexto.SaveChangesAsync();

            return pedido.StatusPedidoEnum;
        }

        //CALCULA A QUANTIDADE VALIDA DE ITEM POR PEDIDO
        public bool QtdeValida(int pessoasMesa, int qtdeEscolhida, int qtdePermitida)
        {
            if (qtdeEscolhida > (qtdePermitida * pessoasMesa) || qtdeEscolhida < 1)
                return false;

            return true;
        }
    }
}
