using Microsoft.EntityFrameworkCore;
using RestauranteDominio.Enums;
using RestauranteRepositorios.Services.ServiceMesa;
using System;
using System.Linq;
using System.Threading.Tasks;
using RestauranteDominio;
using System.Collections.Generic;

namespace RestauranteRepositorios.Services
{
    public class ComandaService
    {
        const int PRODUTO_RODIZIO = 1; 
        private readonly RestauranteContexto _contexto;
        private readonly MesaService _mesaService;
        private readonly ProdutoService _produtoService;

        public ComandaService(RestauranteContexto contexto, MesaService mesaService, ProdutoService produtoService)
        {
            _contexto = contexto;
            _mesaService = mesaService;
            _produtoService = produtoService;
        }


        public async Task<int> AdicionarComanda(AdicionarAtendimentoModel model)
        {
            model.Validar();

            await _mesaService.OcuparMesa(model.MesaId);

            var produto = await _produtoService.ObterProduto(PRODUTO_RODIZIO);
            _ = produto ?? throw new Exception("Produto inexistente");
            var valorTotalPedido = produto.ValorProduto * model.QtdePessoasMesa;

            var comanda = new Comanda()
            {
                DataHoraEntrada = DateTime.Now,
                ComandaPaga = false,
                QtdePessoasMesa = model.QtdePessoasMesa,
                Valor = valorTotalPedido,
                MesaId = model.MesaId,
            };

            comanda.Pedidos.Add(new Pedido()
            {
                ProdutoId = produto.ProdutoId,
                QtdeProduto = model.QtdePessoasMesa,
                ValorPedido = valorTotalPedido,
                StatusPedidoEnum = StatusPedidoEnum.Realizado,
                DataHoraPedido = DateTime.Now
            });

            _contexto.Add(comanda);
            await _contexto.SaveChangesAsync();

            return comanda.ComandaId;
        }

        public async Task<FinalizadaModel> EncerrarComanda(int comandaId)
        {
            var comanda = await _contexto
                .Comanda
                .Where(c => c.ComandaId == comandaId && c.ComandaPaga == false)
                .Include(c => c.Pedidos)
                .ThenInclude(p => p.Produto)
                .OrderBy(c => c.ComandaId)
                .FirstOrDefaultAsync();

            _ = comanda ?? throw new Exception("Comanda já esta paga ou inexistente!");

            await _mesaService.DesocuparMesa(comanda.MesaId);

            comanda.DataHoraSaida = DateTime.Now;
            comanda.ComandaPaga = true;

            await _contexto.SaveChangesAsync();

            var res = new FinalizadaModel
            {
                ComandaId = comanda.ComandaId,
                MesaId = comanda.MesaId,
                DataHoraEntrada = comanda.DataHoraEntrada,
                DataHoraSaida = comanda.DataHoraSaida,
                Valor = comanda.Valor,
                ComandaPaga = comanda.ComandaPaga,
                QtdePessoasMesa = comanda.QtdePessoasMesa,
            };

            res.Pedidos = comanda.Pedidos.Select(p => new BuscarModel
            {
                PedidoId = p.PedidoId,
                ProdutoId = p.ProdutoId,
                Produto = new ListarModel
                {
                    ProdutoId = p.ProdutoId,
                    ImagemProduto = p.Produto.ImagemProduto,
                    NomeProduto = p.Produto.NomeProduto,
                    ValorProduto = p.Produto.ValorProduto,
                    QtdePermitida = p.Produto.QtdePermitida,  
                },
                ComandaId = p.ComandaId,
                QtdeProduto = p.QtdeProduto,
                ValorPedido = p.ValorPedido,
                StatusPedidoEnum = p.StatusPedidoEnum,
                DataHoraPedido = p.DataHoraPedido
            }).ToList();

            return res;
        }

        public async Task<AndamentoModel> BuscarComandaAberta(int comandaId)
        {
             var comanda = await _contexto
                .Comanda
                .Where(c => c.ComandaId == comandaId)
                .Include(c => c.Pedidos)
                .ThenInclude(p => p.Produto)
                .Select(comanda => new 
                {
                    comanda.ComandaId,
                    comanda.MesaId,
                    comanda.DataHoraEntrada,
                    comanda.Valor,
                    comanda.ComandaPaga,
                    comanda.QtdePessoasMesa,
                    comanda.Pedidos
                }).OrderBy(c => c.ComandaId).FirstOrDefaultAsync();

            _ = comanda ?? throw new Exception("Comanda inexistente!");

            var res = new AndamentoModel
            {
                ComandaId = comanda.ComandaId,
                MesaId = comanda.MesaId,
                DataHoraEntrada = comanda.DataHoraEntrada,
                Valor = comanda.Valor,
                ComandaPaga = comanda.ComandaPaga,
                QtdePessoasMesa = comanda.QtdePessoasMesa,
            };

            res.Pedidos = comanda.Pedidos.Select(p => new BuscarModel {
                PedidoId = p.PedidoId,
                ProdutoId = p.ProdutoId,
                Produto = new ListarModel
                {
                    ProdutoId = p.ProdutoId,
                    ImagemProduto = p.Produto.ImagemProduto,
                    NomeProduto = p.Produto.NomeProduto,
                    ValorProduto = p.Produto.ValorProduto,
                    QtdePermitida = p.Produto.QtdePermitida,
                },
                ComandaId = p.ComandaId,
                QtdeProduto = p.QtdeProduto,
                ValorPedido = p.ValorPedido,
                StatusPedidoEnum = p.StatusPedidoEnum,
                DataHoraPedido = p.DataHoraPedido
            }).ToList();

            return res;
        }
    }
}
