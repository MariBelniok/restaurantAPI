using Microsoft.AspNetCore.Mvc;
using RestauranteRepositorios.Services.ServiceMesa;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestauranteRepositorios.Services.ServiceMesa.Models;

namespace RestauranteApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MesaController : ControllerBase
    {
        private readonly MesaService _service;

        public MesaController(MesaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<ObterId>> MesaDisponivel()
        {
             return await _service.BuscarMesasDisponiveis();
        }

        [HttpGet("ocupada")]
        public async Task<List<ObterId>> MesaOcupada()
        {
            return await _service.BuscarMesasOcupadas();
        }
    }
}
