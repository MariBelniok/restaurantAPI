using Microsoft.AspNetCore.Mvc;
using RestauranteDominio.Enums;
using RestauranteRepositorios.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranteApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GerenciaController : ControllerBase
    {
        private readonly GerenciaService _service;

        public GerenciaController(GerenciaService service)
        {
            _service = service;
        }

        [HttpGet()]
        public async Task<List<Completa>> ListarMesas()
        {
            return await _service.ListarMesas();
        }

        [HttpGet("{mesaId}")]
        public async Task<InformacoesModel> ObterMesa(int mesaId)
        {
            return await _service.ObterInformacoesMesas(mesaId);
        }

        [HttpDelete("{mesaId}/cancelar")]
        public async Task<InformacoesModel> CancelarComanda(int mesaId)
        {
            return await _service.CancelarComanda(mesaId);
        }

        [HttpDelete("{comandaId}/pedido/{pedidoId}")]
        public async Task<StatusPedidoEnum> PutDelete(int comandaId, int pedidoId)
        {
            return await _service.CancelarPedido(comandaId, pedidoId);
        }
    }
}
