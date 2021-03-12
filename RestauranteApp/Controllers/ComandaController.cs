using Microsoft.AspNetCore.Mvc;
using RestauranteDominio;
using RestauranteRepositorios.Services;
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
        private readonly ComandaService _service;

        public ComandaController(ComandaService service)
        {
            _service = service;
        }
        // GET: api/<ComandaController>
        [HttpGet("Aberta/{id}", Name = "Aberta")]
        public async Task<ComandaModel> GetComandaAberta(int id)
        {
            var comanda = await _service.BuscarComandaAberta(id);
            return comanda;
        }

        // GET api/<ComandaController>/5
        [HttpGet("Finalizada/{id}", Name = "Finalizada")]
        public async Task<ComandaFinalizadaModel> GetComandaFinalizada(int id)
        {
            var comanda = await _service.BuscarComandaPaga(id);
            return comanda;
        }

        // POST api/<ComandaController>
        [HttpPost]
        public async Task Post(AdicionarComandaModel model)
        {
            await _service.AdicionarComanda(model);
        }

        // PUT api/<ComandaController>/5
        [HttpPut("Pagar/{id}", Name = "Pagar")]
        public Task PutPagar(int id)
        {
            return _service.EncerrarComanda(id);
        }

        [HttpPut("Cancelar/{id}", Name = "Cancelar")]
        public Task PutCancelar(int id)
        {
            return _service.CancelarComanda(id);
        }


    }
}
