using Microsoft.AspNetCore.Mvc;
using RestauranteRepositorios.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestauranteApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _service;

        public ProdutoController(ProdutoService service)
        {
            _service = service;
        }

        [HttpGet("menu")]
        public async Task<List<ListarModel>> GetMenu()
        {
            return await _service.ListarMenu();
        }

        [HttpGet]
        public async Task<List<ListarModel>> GetProdutos()
        {
            return await _service.BuscarProdutoDisponivel();
        }
    }
}