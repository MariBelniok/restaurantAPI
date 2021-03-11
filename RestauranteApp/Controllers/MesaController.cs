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
        public async Task<IActionResult> Get()
        {
            var mesas = await _service.BuscarMesasDisponiveis();
            return Ok(mesas);
        }

        // GET api/<MesaController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MesaController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MesaController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MesaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
