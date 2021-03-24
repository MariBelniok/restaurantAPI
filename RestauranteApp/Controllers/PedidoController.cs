using Microsoft.AspNetCore.Mvc;
using RestauranteRepositorios.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestauranteApp.Controllers
{
    [Route("api/")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _service;

        public PedidoController(PedidoService service)
        {
            _service = service;
        }

        [HttpPost("comanda/{comandaId}/[controller]")]
        public async Task<int> Post(AdicionarNovoModel model, int comandaId)
        {
            return await _service.AdicionarPedido(model, comandaId);
        }

        [HttpPut("comanda/{comandaId}/[controller]/{pedidoId}")]
        public async Task PutAtualizar(AtualizarModel model)
        {
            await _service.AtualizarPedido(model);
        }

        [HttpDelete("comanda/{comandaId}/[controller]/{pedidoId}")]
        public async Task PutDelete(int comandaId, int pedidoId)
        {
            await _service.RemoverPedido(comandaId, pedidoId);
        }
    }
}
