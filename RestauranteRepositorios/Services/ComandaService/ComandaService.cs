using Microsoft.EntityFrameworkCore;
using RestauranteDominio.Enums;
using RestauranteRepositorios.Services.ServiceMesa;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RestauranteRepositorios.Services.ComandaService
{
    public class ComandaService : IComandaService
    {
        private readonly RestauranteContexto _contexto;
        private readonly MesaService _mesaService;

        public ComandaService(RestauranteContexto contexto)
        {
            _contexto = contexto;
        }

        public ComandaService(MesaService mesaService)
        {
            _mesaService = mesaService;
        }

        public async Task AdicionarComanda(AdicionarComandaModel model)
        {
            var comanda = new AdicionarComandaModel()
            {
                MesaId = model.MesaId,
                DataHoraEntrada = model.DataHoraEntrada,
                Valor = model.Valor,
                ComandaPaga = model.ComandaPaga,
                QtdePessoasMesa = model.QtdePessoasMesa
            };

            _contexto.Add(comanda);
            await _contexto.SaveChangesAsync();

            _mesaService.OcuparMesa(comanda.MesaId);
        }

        //ENCERRA E PAGA COMANDA
        public async Task EncerrarComanda(int mesaId)
        {
            var comanda = _contexto.Comanda
                        .Where(c => c.MesaId == mesaId && c.ComandaPaga == false)
                        .LastOrDefault();

            _ = comanda ?? throw new Exception("Comanda inexistente!");

            comanda.DataHoraSaida = DateTime.Now;
            comanda.Valor = ValorTotalComanda(comanda.ComandaId);
            comanda.ComandaPaga = true;

            await _contexto.SaveChangesAsync();

            _mesaService.DesocuparMesa(comanda.MesaId);
        }

        //CANCELA COMANDA
        public async Task CancelarComanda(int mesaId)
        {
            var comanda = _contexto.Comanda
                        .Where(c => c.MesaId == mesaId && c.ComandaPaga == false)
                        .LastOrDefault();

            _ = comanda ?? throw new Exception("Comanda inexistente!");
            comanda.DataHoraSaida = DateTime.Now;
            comanda.Valor = 0;
            comanda.ComandaPaga = true;

            await _contexto.SaveChangesAsync();

            _mesaService.DesocuparMesa(comanda.MesaId);
        }

        //BUSCA A COMANDAS ADICIONADAS NA MODEL
        public async Task<ComandaFinalizadaModel> BuscarComandaPaga(int comandaId)
        {

            var comanda = await _contexto.Comanda
                        .Where(c => c.ComandaId == comandaId)
                        .Include(c => c.Pedidos)
                        .Select(comanda => new
                        {
                            comanda.ComandaId,
                            comanda.MesaId,
                            comanda.DataHoraEntrada,
                            comanda.DataHoraSaida,
                            comanda.Valor,
                            comanda.ComandaPaga,
                            comanda.QtdePessoasMesa,
                            comanda.Pedidos
                        }).LastOrDefaultAsync();

            var res = new ComandaFinalizadaModel
            {
                ComandaId = comanda.ComandaId,
                MesaId = comanda.MesaId,
                DataHoraEntrada = comanda.DataHoraEntrada,
                DataHoraSaida = comanda.DataHoraSaida,
                Valor = comanda.Valor,
                ComandaPaga = comanda.ComandaPaga,
                QtdePessoasMesa = comanda.QtdePessoasMesa,
                Pedidos = (ICollection<BuscarPedidoModel>)comanda.Pedidos
            };

            res.Pedidos.Select(p => new BuscarPedidoModel
            {
                PedidoId = p.PedidoId,
                ComandaId = p.ComandaId,
                ProdutoId = p.ProdutoId,
                QtdeProduto = p.QtdeProduto,
                ValorPedido = p.ValorPedido,
                StatusPedidoId = p.StatusPedidoId
            }).ToList();

            _ = comanda ?? throw new Exception("Comanda inexistente!");

            return res;
        }

        public async Task<ComandaModel> BuscarComandaAberta(int comandaId)
        {
             var comanda = await _contexto.Comanda
                        .Where(c => c.ComandaId == comandaId)
                        .Include(c => c.Pedidos)
                        .Select(comanda => new 
                        {
                            comanda.ComandaId,
                            comanda.MesaId,
                            comanda.DataHoraEntrada,
                            comanda.Valor,
                            comanda.ComandaPaga,
                            comanda.QtdePessoasMesa,
                            comanda.Pedidos
                        }).LastOrDefaultAsync();

            var res = new ComandaModel
            {
                ComandaId = comanda.ComandaId,
                MesaId = comanda.MesaId,
                DataHoraEntrada = comanda.DataHoraEntrada,
                Valor = comanda.Valor,
                ComandaPaga = comanda.ComandaPaga,
                QtdePessoasMesa = comanda.QtdePessoasMesa,
                Pedidos = (ICollection<BuscarPedidoModel>)comanda.Pedidos
            };

            res.Pedidos.Select(p => new BuscarPedidoModel {
                PedidoId = p.PedidoId,
                ComandaId = p.ComandaId,
                ProdutoId = p.ProdutoId,
                QtdeProduto = p.QtdeProduto,
                ValorPedido = p.ValorPedido,
                StatusPedidoId = p.StatusPedidoId
            }).ToList();

            _ = comanda ?? throw new Exception("Comanda inexistente!");

            return res;
        }

        //CALCULA O VALOR TOTAL DA COMANDA
        public double ValorTotalComanda(int comandaId)
        {
            var valorTotalComanda = _contexto.Pedido
                .Where(p => p.ComandaId == comandaId && (int)StatusPedidoEnum.Realizado == p.StatusPedidoId)
                .Select(p => p.ValorPedido)
                .Sum();

            return valorTotalComanda;
        }
    }
}
