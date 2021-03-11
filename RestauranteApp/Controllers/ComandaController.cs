using Microsoft.AspNetCore.Mvc;
using RestauranteRepositorios.Services;
using RestauranteRepositorios.Services.ComandaService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestauranteApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComandaController : ControllerBase
    {
        private readonly IComandaService _service;

        public ComandaController(IComandaService service)
        {
            _service = service;
        }
        // GET: api/<ComandaController>
        [HttpGet("{comandaAberta/id}")]
        public async Task<ComandaModel> GetComandaAberta(int id)
        {
            var comanda = await _service.BuscarComandaAberta(id);
            return comanda;
        }

        // GET api/<ComandaController>/5
        [HttpGet("{comandaPaga/id}")]
        public async Task<ComandaFinalizadaModel> GetComandaFinalizada(int id)
        {
            var comanda = await _service.BuscarComandaPaga(id);
            return comanda;
        }

        // POST api/<ComandaController>
        [HttpPost]
        public async Task<AdicionarComandaModel> Post(AdicionarComandaModel model)
        {
            var adicionarComanda = await _service.AdicionarComanda(model);
        }

        // PUT api/<ComandaController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ComandaController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
