using Microsoft.AspNetCore.Mvc;
using RestauranteDominio.Enums;
using RestauranteRepositorios.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestauranteApp.Controllers
{
    [Route("")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _service;

        public PedidoController(PedidoService service)
        {
            _service = service;
        }

        [HttpGet("comanda/{comandaId}/[controller]")]
        public async Task<List<BuscarModel>> Buscar(int comandaId)
        {
            return await _service.Buscar(comandaId);
        }

        [HttpPost("comanda/{comandaId}/[controller]")]
        public async Task<int> Post(AdicionarNovoModel model, int comandaId)
        {
            return await _service.AdicionarPedido(model, comandaId);
        }

        [HttpPut("comanda/{comandaId}/[controller]/{pedidoId}")]
        public async Task<BuscarModel> PutAtualizar(AtualizarModel model, int comandaId)
        {
            return await _service.AtualizarPedido(model, comandaId);
        }

        [HttpDelete("comanda/{comandaId}/[controller]/{pedidoId}")]
        public async Task<StatusPedidoEnum> PutDelete(int comandaId, int pedidoId)
        {
            return await _service.RemoverPedido(comandaId, pedidoId);
        }
    }
}
