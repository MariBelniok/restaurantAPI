using Microsoft.EntityFrameworkCore;
using RestauranteRepositorios.Services.ServiceMesa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestauranteRepositorios.Services
{
    public class GerenciaService
    {
        private readonly RestauranteContexto _contexto;
        private readonly MesaService _mesaService;

        public GerenciaService(RestauranteContexto contexto, MesaService mesaService)
        {
            _contexto = contexto;
            _mesaService = mesaService;
        }

        public async Task<List<Completa>> ListarMesas()
        {
            var mesas = await _contexto
                .Mesa
                .Select(m => new Completa
                {
                    MesaId = m.MesaId,
                    Capacidade = m.Capacidade,
                    MesaOcupada = m.MesaOcupada
                }).ToListAsync();

            return mesas;
        }
        public async Task<InformacoesModel> ObterInformacoesMesas(int mesaId)
        {
            var comanda = await _contexto
               .Comanda
               .Where(c => c.MesaId == mesaId && c.ComandaPaga != true)
               .Include(c => c.Pedidos)
               .ThenInclude(p => p.Produto)
               .Include(c => c.Mesa)
               .Select(comanda => new
               {
                   comanda.ComandaId,
                   comanda.MesaId,
                   comanda.DataHoraEntrada,
                   comanda.Valor,
                   comanda.ComandaPaga,
                   comanda.QtdePessoasMesa,
                   comanda.Mesa,
                   comanda.Pedidos
               }).OrderByDescending(c => c.ComandaId).FirstOrDefaultAsync();

            _ = comanda ?? throw new Exception("Comanda inexistente!");

            var res = new InformacoesModel
            {
                ComandaId = comanda.ComandaId,
                MesaId = comanda.MesaId,
                DataHoraEntrada = comanda.DataHoraEntrada,
                Valor = comanda.Valor,
                ComandaPaga = comanda.ComandaPaga,
                QtdePessoasMesa = comanda.QtdePessoasMesa,
                Mesa = new Completa
                {
                    MesaId = comanda.MesaId,
                    Capacidade = comanda.Mesa.Capacidade,
                    MesaOcupada = comanda.Mesa.MesaOcupada
                }
            };

            res.Pedidos = comanda.Pedidos.Select(p => new BuscarModel
            {
                PedidoId = p.PedidoId,
                ProdutoId = p.ProdutoId,
                Produto = new ListarModel
                {
                    ProdutoId = p.ProdutoId,
                    NomeProduto = p.Produto.NomeProduto,
                    ValorProduto = p.Produto.ValorProduto,
                    QtdePermitida = p.Produto.QtdePermitida,
                },
                ComandaId = p.ComandaId,
                QtdeProduto = p.QtdeProduto,
                ValorPedido = p.ValorPedido,
                StatusPedidoEnum = p.StatusPedidoEnum
            }).ToList();

            return res;
        }

        public async Task<InformacoesModel> CancelarComanda(int comandaId)
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
            comanda.Valor = 0;

            await _contexto.SaveChangesAsync();

            var res = new InformacoesModel
            {
                ComandaId = comanda.ComandaId,
                MesaId = comanda.MesaId,
                DataHoraEntrada = comanda.DataHoraEntrada,
                DataHoraSaida = comanda.DataHoraSaida,
                Valor = comanda.Valor,
                ComandaPaga = comanda.ComandaPaga,
                QtdePessoasMesa = comanda.QtdePessoasMesa,
            };

            return res;
        }
    }
}
