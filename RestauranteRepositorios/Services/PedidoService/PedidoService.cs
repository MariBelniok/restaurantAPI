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
    public class PedidoService : IPedidoService
    {
        private readonly RestauranteContexto _contexto;

        public PedidoService(RestauranteContexto contexto)
        {
            _contexto = contexto;
        }

        //ADICIONA PEDIDOS EM MODEL AUXILIAR
        public async Task AdicionarPedido(AdicionarPedidoModel model)
        {
            if(!QtdeValida(model.ProdutoId, model.QtdeProduto, model.ComandaId))
                throw new Exception("Quantidade de items escolhido esta acima do permitido! ");

            var pedido = new Pedido()
            {
                ComandaId = model.ComandaId,
                ProdutoId = model.ProdutoId,
                QtdeProduto = model.QtdeProduto,
                ValorPedido = model.ValorPedido,
                StatusPedidoId = (int)StatusPedidoEnum.Realizado
            };

            _contexto.Add(pedido);
            await _contexto.SaveChangesAsync();
        }

        //ATUALIZA UM PRODUTO
        public async Task AtualizarPedido(int pedidoId, int comandaId, int quantidadeItem)
        {        
            var pedido = _contexto.Pedido
                    .Where(ped => ped.PedidoId == pedidoId && ped.ComandaId == comandaId)
                    .LastOrDefault();

            _ = pedido ?? throw new Exception("Pedido inexistente");

            if (!QtdeValida(pedido.ProdutoId, quantidadeItem, comandaId))
            {
                throw new Exception("Quantidade de items escolhido esta acima do permitido! ");
            }

            pedido.QtdeProduto = quantidadeItem;
            pedido.ValorPedido = ValorTotalPedido(pedido.ProdutoId, quantidadeItem);

            await _contexto.SaveChangesAsync();
        }

        //REMOVE UM PRODUTO
        public async Task RemoverPedido(int pedidoId, int comandaId)
        {
            if(pedidoId == 1)
                throw new Exception("O rodizio não pode ser cancelado da sua comanda!");
            
            var pedido = _contexto.Pedido
                        .Where(ped => ped.PedidoId == pedidoId && ped.ComandaId == comandaId)
                        .LastOrDefault();

            _ = pedido ?? throw new Exception("Pedido inexistente");

            pedido.StatusPedidoId = (int)StatusPedidoEnum.Cancelado;
            await _contexto.SaveChangesAsync();
        }

        //LISTA OS PEDIDOS REALIZADOS
        public async Task<List<BuscarPedidoModel>> BuscarPedidos(int comandaId)
        {
            var pedidos =
                _contexto.Pedido
                .Where(ped => ped.ComandaId == comandaId)
                .Select(p => new BuscarPedidoModel()
                {
                    PedidoId = p.PedidoId,
                    ProdutoId = p.ProdutoId,
                    ComandaId = p.ComandaId,
                    QtdeProduto = p.QtdeProduto,
                    ValorPedido = p.ValorPedido,
                    StatusPedidoId = (StatusPedidoEnum)p.StatusPedidoId
                }).ToListAsync();

            _ = pedidos ?? throw new Exception("Pedidos inexistentes");

            return await pedidos;
        }

        //CALCULA O VALOR TOTAL DO PEDIDO
        public double ValorTotalPedido(int prodId, int quantidade)
        {
            return _contexto.Produto
                    .Where(p => p.ProdutoId == prodId)
                    .Sum(p => p.ValorProduto * quantidade);
        }

        public bool QtdeValida(int prodId, int qtdeEscolhida, int comandaId)
        {
            int pessoasMesa = _contexto.Comanda
                        .Where(c => c.ComandaId == comandaId)
                        .Select(c => c.QtdePessoasMesa).FirstOrDefault();
            int qtdePermitida = _contexto.Produto
                        .Where(p => p.ProdutoId == prodId)
                        .Select(p => p.QtdePermitida).FirstOrDefault();
            if(qtdePermitida * pessoasMesa > qtdeEscolhida)
            {
                return false;
            }
            return true;
        }
    }
}
