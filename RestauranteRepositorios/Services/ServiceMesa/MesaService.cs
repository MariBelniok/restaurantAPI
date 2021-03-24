using Microsoft.EntityFrameworkCore;
using RestauranteRepositorios.Services.ServiceMesa.Models;
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

        public async Task OcuparMesa(int mesaId)
        {
            var mesa = await _contexto
                .Mesa
                .Where(m => m.MesaId == mesaId && m.MesaOcupada == false)
                .FirstOrDefaultAsync();

            _ = mesa ?? throw new Exception("Mesa inexistente ou ocupada");

            mesa.MesaOcupada = true;
        }

        public async Task DesocuparMesa(int mesaId)
        {
            var mesa = await _contexto
                .Mesa
                .Where(m => m.MesaId == mesaId && m.MesaOcupada == true)
                .FirstOrDefaultAsync();

            _ = mesa ?? throw new Exception("Mesa inexistente ou ocupada");
            
            mesa.MesaOcupada = false;
        }

        public async Task<List<ObterId>> BuscarMesasDisponiveis()
        {
            return await _contexto
                .Mesa
                .Where(m => m.MesaOcupada != true)
                .Select(m => new ObterId{ 
                    MesaId = m.MesaId
                }).ToListAsync();
        }
    }
}
