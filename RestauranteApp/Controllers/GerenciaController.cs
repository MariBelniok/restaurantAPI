using Microsoft.AspNetCore.Mvc;
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
    }
}
