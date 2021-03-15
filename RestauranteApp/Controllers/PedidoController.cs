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
        private readonly PedidoService _service;

        public PedidoController(PedidoService service)
        {
            _service = service;
        }

        // GET api/<PedidoController>/5
        [HttpGet("{comandaId}")]
        public async Task<List<BuscarPedidoModel>> Get(int comandaId)
        {
            var pedidos = _service.BuscarPedidos(comandaId);
            return await pedidos;
        }

        // POST api/<PedidoController>
        [HttpPost("{comandaId}")]
        public Task Post(AdicionarPedidoModel model, int comandaId)
        {
            return _service.AdicionarPedido(model, comandaId);
        }

        // PUT api/<PedidoController>/5
        [HttpPut("Atualizar/{comandaId}/{pedidoId}")]
        public Task PutAtualizar(AtualizarPedidoModel model)
        {
            return _service.AtualizarPedido(model);
        }

        // PUT api/<PedidoController>/5
        [HttpPut("Cancelar/{comandaId}/{pedidoId}")]
        public Task PutDelete(int comandaId, int pedidoId)
        {
            return _service.RemoverPedido(comandaId, pedidoId);
        }
    }
}
