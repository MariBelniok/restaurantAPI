using Microsoft.AspNetCore.Mvc;
using RestauranteRepositorios.Services.ServiceMesa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/<MesaController>
        [HttpGet]
        public async Task<List<int>> Get()
        {
            var mesas = await _service.BuscarMesasDisponiveis();
            return mesas;
        }
    }
}
