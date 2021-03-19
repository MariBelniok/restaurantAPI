using Microsoft.AspNetCore.Mvc;
using RestauranteRepositorios.Services;
using System.Collections.Generic;
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

        [HttpPost("{comandaId}")]
        public async Task Post(AdicionarNovoModel model, int comandaId)
        {
            await _service.AdicionarPedido(model, comandaId);
        }

        [HttpPut("{comandaId}/{pedidoId}")]
        public async Task PutAtualizar(AtualizarModel model)
        {
            await _service.AtualizarPedido(model);
        }

        [HttpDelete("{comandaId}/{pedidoId}")]
        public async Task PutDelete(int comandaId, int pedidoId)
        {
            await _service.RemoverPedido(comandaId, pedidoId);
        }
    }
}
