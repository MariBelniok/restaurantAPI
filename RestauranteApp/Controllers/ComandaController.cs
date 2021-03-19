using Microsoft.AspNetCore.Mvc;
using RestauranteRepositorios.Services;
using System.Threading.Tasks;

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

        [HttpGet("aberta/{comandaId}", Name = "aberta")]
        public async Task<AndamentoModel> GetComandaAberta(int comandaId)
        {
            var comanda = await _service.BuscarComandaAberta(comandaId);
            return comanda;
        }

        [HttpGet("finalizada/{comandaId}", Name = "finalizada")]
        public async Task<FinalizadaModel> GetComandaFinalizada(int comandaId)
        {
            var comanda = await _service.BuscarComandaPaga(comandaId);
            return comanda;
        }

        [HttpPost]
        public async Task Post(AdicionarAtendimentoModel model)
        {
            await _service.AdicionarComanda(model);
        }

        [HttpPut("pagar/{comandaId}", Name = "pagar")]
        public async Task PutPagar(int comandaId)
        {
            await _service.EncerrarComanda(comandaId);
        }

        [HttpDelete("{comandaId}", Name = "cancelar")]
        public async Task PutCancelar(int comandaId)
        {
            await _service.CancelarComanda(comandaId);
        }
    }
}
