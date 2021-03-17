using Microsoft.AspNetCore.Mvc;
using RestauranteRepositorios.Services.ServiceMesa;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestauranteRepositorios.Services.ServiceMesa.Models;

namespace RestauranteApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MesaController : ControllerBase
    {
        private readonly MesaService _service;

        public MesaController(MesaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<ObterId>> Get()
        {
             return await _service.BuscarMesasDisponiveis();
        }
    }
}
