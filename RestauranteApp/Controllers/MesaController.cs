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
        public Task Get()
        {
            var mesas = _service.BuscarMesasDisponiveis();
            return mesas;
        }

        // PUT api/<MesaController>/5
        [HttpPut("Ocupar/{id}")]
        public async Task PutOcupar(int id)
        {
           await _service.OcuparMesa(id);
        }
        
        [HttpPut("Desocupar/{id}")]
        public async Task PutDesocupar(int id)
        {
            await _service.DesocuparMesa(id);
        }
    }
}
