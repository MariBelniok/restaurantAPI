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

        [HttpGet("{comandaId}", Name = "aberta")]
        public async Task<AndamentoModel> GetComandaAberta(int comandaId)
        {
            return await _service.BuscarComandaAberta(comandaId);
        }

        [HttpGet("{comandaId}/finalizacao", Name = "finalizada")]
        public async Task<FinalizadaModel> GetComandaFinalizada(int comandaId)
        {
            return await _service.BuscarComandaPaga(comandaId);
        }

        [HttpPost]
        public async Task<int> Post(AdicionarAtendimentoModel model)
        {
            return await _service.AdicionarComanda(model);
        }

        [HttpPost("{comandaId}/pagamento", Name = "pagar")]
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
