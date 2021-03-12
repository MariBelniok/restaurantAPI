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
        private readonly IMesaService _service;

        public MesaController(IMesaService service)
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
        [HttpPut("{ocuparMesa/id}")]
        public Task PutOcupar(int mesaId)
        {
            return _service.OcuparMesa(mesaId);
        }
        
        [HttpDelete("{desocuparMesa/id}")]
        public Task PutDesocupar(int mesaId)
        {
            return _service.DesocuparMesa(mesaId);
        }
    }
}
