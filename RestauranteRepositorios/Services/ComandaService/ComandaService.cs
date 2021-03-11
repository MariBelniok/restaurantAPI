using Microsoft.EntityFrameworkCore;
using RestauranteDominio.Enums;
using RestauranteRepositorios.Services.ServiceMesa;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        public void AdicionarComanda(AdicionarComandaModel model)
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
            _contexto.SaveChangesAsync();

            _mesaService.OcuparMesa(comanda.MesaId);
        }

        //ENCERRA E PAGA COMANDA
        public void EncerrarComanda(int mesaId)
        {
            var comanda = _contexto.Comanda
                        .Where(c => c.MesaId == mesaId && c.ComandaPaga == false)
                        .LastOrDefault();

            _ = comanda ?? throw new Exception("Comanda inexistente!");

            comanda.DataHoraSaida = DateTime.Now;
            comanda.Valor = ValorTotalComanda(comanda.ComandaId);
            comanda.ComandaPaga = true;

            _contexto.SaveChangesAsync();

            _mesaService.DesocuparMesa(comanda.MesaId);
        }

        //CANCELA COMANDA
        public void CancelarComanda(int mesaId)
        {
            var comanda = _contexto.Comanda
                        .Where(c => c.MesaId == mesaId && c.ComandaPaga == false)
                        .LastOrDefault();

            _ = comanda ?? throw new Exception("Comanda inexistente!");

            comanda.DataHoraSaida = DateTime.Now;
            comanda.Valor = 0;
            comanda.ComandaPaga = true;

            _contexto.SaveChangesAsync();

            _mesaService.DesocuparMesa(comanda.MesaId);
        }

        //BUSCA A COMANDAS ADICIONADAS NA MODEL
        public async Task<ComandaFinalizadaModel> BuscarComandaPaga(int comandaId)
        {

            var comanda = _contexto.Comanda
                        .Include(c => c.Pedidos)
                        .Where(c => c.ComandaId == comandaId)
                        .Select(comanda => new ComandaFinalizadaModel
                        {
                            ComandaId = comanda.ComandaId,
                            MesaId = comanda.MesaId,
                            DataHoraEntrada = comanda.DataHoraEntrada,
                            DataHoraSaida = (DateTime)comanda.DataHoraSaida,
                            Valor = comanda.Valor,
                            ComandaPaga = comanda.ComandaPaga,
                            QtdePessoasMesa = comanda.QtdePessoasMesa
                        }).LastOrDefaultAsync();

            _ = comanda ?? throw new Exception("Comanda inexistente!");

            return await comanda;
        }

        public async Task<ComandaModel> BuscarComandaAberta(int comandaId)
        {
             var comanda = _contexto.Comanda
                        .Include(c => c.Pedidos)
                        .Where(c => c.ComandaId == comandaId)
                        .Select(comanda => new ComandaModel
                        {
                            ComandaId = comanda.ComandaId,
                            MesaId = comanda.MesaId,
                            DataHoraEntrada = comanda.DataHoraEntrada,
                            Valor = comanda.Valor,
                            ComandaPaga = comanda.ComandaPaga,
                            QtdePessoasMesa = comanda.QtdePessoasMesa
                        }).LastOrDefaultAsync();

            _ = comanda ?? throw new Exception("Comanda inexistente!");

            return await comanda;
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
