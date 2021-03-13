using Microsoft.EntityFrameworkCore;
using RestauranteDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranteRepositorios.Services.ServiceMesa
{
    public class MesaService
    {
        private readonly RestauranteContexto _contexto;

        public MesaService(RestauranteContexto contexto)
        {
            _contexto = contexto;
        }

        //MUDA MESAOCUPADA PARA TRUE
        public async Task OcuparMesa(int mesaId)
        {
            var mesa = _contexto.Mesa
                        .Where(m => m.MesaId == mesaId && m.MesaOcupada == false)
                        .FirstOrDefault();

            _ = mesa ?? throw new Exception("Mesa inexistente ou ocupada");

            mesa.MesaOcupada = true;

            await _contexto.SaveChangesAsync();
        }

        //MUDA MESOCUPADA PARA FALSE
        public async Task DesocuparMesa(int mesaId)
        {
            var mesa = _contexto.Mesa
                    .Where(m => m.MesaId == mesaId && m.MesaOcupada == true)
                    .FirstOrDefault();

            _ = mesa ?? throw new Exception("Mesa inexistente ou ocupada");
            
            mesa.MesaOcupada = false;

            await _contexto.SaveChangesAsync();
        }

        //LISTA AS MESAS DISPONIVEIS
        public async Task<List<int>> BuscarMesasDisponiveis()
        {
            return await _contexto.Mesa
                    .Where(m => m.MesaOcupada != true)
                    .Select(m => m.MesaId)
                    .ToListAsync();
        }
    }
}
