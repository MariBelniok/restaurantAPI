 using Microsoft.EntityFrameworkCore;
using RestauranteDominio;
using RestauranteDominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestauranteRepositorios.Services
{
    public class PedidoService
    {
        private readonly RestauranteContexto _contexto;
        private readonly ProdutoService _produtoService;
        private readonly ComandaService _comandaService;

        public PedidoService(RestauranteContexto contexto, ProdutoService produtoService, ComandaService comandaService)
        {
            _contexto = contexto;
            _produtoService = produtoService;
            _comandaService = comandaService;
        }

        //ADICIONA PEDIDOS
        public async Task AdicionarPedido(AdicionarNovoModel model, int comandaId)
        {
            model.Validar();

            var produto = await _produtoService.ObterProduto(model.ProdutoId);
            _ = produto ?? throw new Exception("Produto inexistente");
            var valorTotalPedido = produto.ValorProduto * model.QtdeProduto;

            if (model.ProdutoId == 1) //item rodizio
                throw new Exception("Esse produto é inválido.");

            if (!QtdeValida(model.ProdutoId, model.QtdeProduto, model.ComandaId))
                throw new Exception("Quantidade de items escolhido esta acima do permitido! ");

            var pedido = new Pedido()
            {
                ProdutoId = model.ProdutoId,
                ComandaId = comandaId,
                QtdeProduto = model.QtdeProduto,
                ValorPedido = valorTotalPedido,
                StatusPedidoId = (int)StatusPedidoEnum.Realizado
            };

            _contexto.Add(pedido);

            await _contexto.SaveChangesAsync();

            if(pedido.ValorPedido > 0)
                await _comandaService.AtualizarValorComanda(comandaId);
        }

        //ATUALIZA UM PRODUTO
        public async Task AtualizarPedido(AtualizarModel model)
        {
            var produto = await _produtoService.ObterProduto(model.ProdutoId);
            _ = produto ?? throw new Exception("Produto inexistente");
            var valorTotalPedido = produto.ValorProduto * model.QtdeProduto;

            var pedido = _contexto.Pedido
                    .Where(p => p.ComandaId == model.ComandaId)
                    .OrderBy(p => p.PedidoId)
                    .LastOrDefault();

            if (model.PedidoId < pedido.PedidoId)
                throw new Exception("Só é possivel atualizar o ultimo pedido realizado!");

            _ = pedido ?? throw new Exception("Pedido inexistente");

            if (!QtdeValida(pedido.ProdutoId, model.QtdeProduto, pedido.ComandaId))
                throw new Exception("Quantidade de items escolhido esta acima do permitido! ");

            pedido.QtdeProduto = model.QtdeProduto;
            pedido.ValorPedido = valorTotalPedido;

            await _contexto.SaveChangesAsync();

            if (pedido.ValorPedido > 0)
                await _comandaService.AtualizarValorComanda(pedido.ComandaId);
        }

        //REMOVE UM PRODUTO
        public async Task RemoverPedido(int comandaId, int pedidoId)
        {

            if(pedidoId == 1)
                throw new Exception("O rodizio não pode ser cancelado da sua comanda!");
            
            var pedido = _contexto.Pedido
                        .Where(p => p.ComandaId == comandaId)
                        .OrderBy(p => p.PedidoId)
                        .LastOrDefault();

            if (pedidoId < pedido.PedidoId)
                throw new Exception("Só é possivel cancelar o ultimo pedido realizado!");

            _ = pedido ?? throw new Exception("Pedido inexistente");

            pedido.StatusPedidoId = (int)StatusPedidoEnum.Cancelado;

            await _contexto.SaveChangesAsync();

            if (pedido.ValorPedido > 0)
                await _comandaService.AtualizarValorComanda(comandaId);
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
            if(qtdeEscolhida > (qtdePermitida * pessoasMesa))
            {
                return false;
            }

            return true;
        }
    }
}
