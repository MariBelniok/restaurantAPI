using Microsoft.AspNetCore.Mvc;
using RestauranteRepositorios.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/<ProdutoController>
        [HttpGet("menu")]
        public async Task<List<ListarProdutosModel>> GetMenu()
        {
            var menuProdutos = await _service.ListarMenu();
            return menuProdutos;
        }

        // GET api/<ProdutoController>/5
        [HttpGet]
        public async Task<List<ListarProdutosModel>> GetProdutos()
        {
            var menuProdutos = await _service.BuscarProdutoDisponivel();
            return menuProdutos;
        }
    }
}
