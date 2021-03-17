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

        [HttpGet("aberta/{id}", Name = "aberta")]
        public async Task<AndamentoModel> GetComandaAberta(int id)
        {
            var comanda = await _service.BuscarComandaAberta(id);
            return comanda;
        }

        [HttpGet("finalizada/{id}", Name = "finalizada")]
        public async Task<FinalizadaModel> GetComandaFinalizada(int id)
        {
            var comanda = await _service.BuscarComandaPaga(id);
            return comanda;
        }

        [HttpPost]
        public async Task Post(AdicionarAtendimentoModel model)
        {
            await _service.AdicionarComanda(model);
        }

        [HttpPut("pagar/{id}", Name = "pagar")]
        public async Task PutPagar(int id)
        {
            await _service.EncerrarComanda(id);
        }

        [HttpPut("cancelar/{id}", Name = "cancelar")]
        public async Task PutCancelar(int id)
        {
            await _service.CancelarComanda(id);
        }
    }
}
