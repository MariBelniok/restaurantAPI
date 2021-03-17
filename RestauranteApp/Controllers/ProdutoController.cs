using Microsoft.AspNetCore.Mvc;
using RestauranteRepositorios.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestauranteApp.Controllers
{
    [Route("api/[controller]")]
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
            var menuProdutos = await _service.ListarMenu();
            return menuProdutos;
        }

        [HttpGet]
        public async Task<List<ListarModel>> GetProdutos()
        {
            var menuProdutos = await _service.BuscarProdutoDisponivel();
            return menuProdutos;
        }
    }
}
