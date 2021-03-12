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
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _service;

        public PedidoController(IPedidoService service)
        {
            _service = service;
        }
        // GET: api/<PedidoController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PedidoController>/5
        [HttpGet("{id}")]
        public async Task<List<BuscarPedidoModel>> Get(int comandaId)
        {
            var pedidos = _service.BuscarPedidos(comandaId);
            return await pedidos;
        }

        // POST api/<PedidoController>
        [HttpPost]
        public Task Post(AdicionarPedidoModel model)
        {
            return _service.AdicionarPedido(model);
        }

        // PUT api/<PedidoController>/5
        [HttpPut("{atualizar/id}")]
        public Task PutAtualizar(int pedidoId, int comandaId, int quantidadeItem)
        {
            return _service.AtualizarPedido(pedidoId, comandaId, quantidadeItem);
        }

        // DELETE api/<PedidoController>/5
        [HttpDelete("{cancelar/id}")]
        public Task PutDelete(int pedidoId, int comandaId)
        {
            return _service.RemoverPedido(pedidoId, comandaId);
        }
    }
}
