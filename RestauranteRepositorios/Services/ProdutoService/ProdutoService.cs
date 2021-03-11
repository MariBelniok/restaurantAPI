using Microsoft.EntityFrameworkCore;
using RestauranteDominio;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranteRepositorios.Services
{ 
    public class ProdutoService : IProdutoService
    {
        private readonly RestauranteContexto _contexto;

        public ProdutoService(RestauranteContexto contexto)
        {
            _contexto = contexto;
        }
        //FILTRA PRODUTOS DISPONIVEIS
        public async Task<List<ListarProdutosModel>> BuscarProdutoDisponivel()
        {
            var produtos = _contexto.Produto
                .Where(p => p.Disponivel)
                .Select(a => new ListarProdutosModel
                {
                    NomeProduto = a.NomeProduto,
                    ProdutoId = a.ProdutoId,
                    ValorProduto = a.ValorProduto,
                    QtdePermitida = a.QtdePermitida
                })
                .ToListAsync();

            return await produtos;
        }
        public async Task<List<ListarProdutosModel>> ListarMenu()
        {
            var produtos = _contexto.Produto
                .Where(p => p.Disponivel && p.ProdutoId > 1)
                .Select(a => new ListarProdutosModel
                {
                    NomeProduto = a.NomeProduto,
                    ProdutoId = a.ProdutoId,
                    ValorProduto = a.ValorProduto,
                    QtdePermitida = a.QtdePermitida
                })
                .ToListAsync();

            return await produtos;
        }
    }
}
