//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace RestauranteRepositorios.Services
//{
//    public class GerenciaService
//    {
//        public async Task<AndamentoModel> BuscarComandaAberta(int comandaId)
//        {
//            var comanda = await _contexto
//               .Comanda
//               .Where(c => c.ComandaId == comandaId)
//               .Include(c => c.Pedidos)
//               .ThenInclude(p => p.Produto)
//               .Select(comanda => new
//               {
//                   comanda.ComandaId,
//                   comanda.MesaId,
//                   comanda.DataHoraEntrada,
//                   comanda.Valor,
//                   comanda.ComandaPaga,
//                   comanda.QtdePessoasMesa,
//                   comanda.Pedidos
//               }).OrderBy(c => c.ComandaId).FirstOrDefaultAsync();

//            _ = comanda ?? throw new Exception("Comanda inexistente!");

//            var res = new AndamentoModel
//            {
//                ComandaId = comanda.ComandaId,
//                MesaId = comanda.MesaId,
//                DataHoraEntrada = comanda.DataHoraEntrada,
//                Valor = comanda.Valor,
//                ComandaPaga = comanda.ComandaPaga,
//                QtdePessoasMesa = comanda.QtdePessoasMesa,
//            };

//            res.Pedidos = comanda.Pedidos.Select(p => new BuscarModel
//            {
//                PedidoId = p.PedidoId,
//                ProdutoId = p.ProdutoId,
//                Produto = new ListarModel
//                {
//                    ProdutoId = p.ProdutoId,
//                    NomeProduto = p.Produto.NomeProduto,
//                    ValorProduto = p.Produto.ValorProduto,
//                    QtdePermitida = p.Produto.QtdePermitida,
//                },
//                ComandaId = p.ComandaId,
//                QtdeProduto = p.QtdeProduto,
//                ValorPedido = p.ValorPedido,
//                StatusPedidoEnum = p.StatusPedidoEnum
//            }).ToList();

//            return res;
//        }
//    }
//}
