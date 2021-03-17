using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranteRepositorios.Services
{ 
    public class ProdutoService
    {
        private readonly RestauranteContexto _contexto;

        public ProdutoService(RestauranteContexto contexto)
        {
            _contexto = contexto;
        }
        //FILTRA PRODUTOS DISPONIVEIS
        public async Task<List<ListarModel>> BuscarProdutoDisponivel()
        {
            var produtos = _contexto.Produto
                .Where(p => p.Disponivel)
                .Select(a => new ListarModel
                {
                    NomeProduto = a.NomeProduto,
                    ProdutoId = a.ProdutoId,
                    ValorProduto = a.ValorProduto,
                    QtdePermitida = a.QtdePermitida
                })
                .ToListAsync();

            _ = produtos ?? throw new Exception("Produtos inexistentes");

            return await produtos;
        }
        public async Task<List<ListarModel>> ListarMenu()
        {
            var produtos = _contexto.Produto
                .Where(p => p.Disponivel && p.ProdutoId > 1)
                .Select(a => new ListarModel
                {
                    NomeProduto = a.NomeProduto,
                    ProdutoId = a.ProdutoId,
                    ValorProduto = a.ValorProduto,
                    QtdePermitida = a.QtdePermitida
                })
                .ToListAsync();

            _ = produtos ?? throw new Exception("Produtos inexistentes");

            return await produtos;
        }

        public async Task<ListarModel> ObterProduto(int produtoId)
        {
            var produto = await _contexto.Produto
                        .Where(p => p.ProdutoId == produtoId)
                        .Select(p => new ListarModel()
                        {
                            ProdutoId = p.ProdutoId,
                            NomeProduto = p.NomeProduto,
                            ValorProduto = p.ValorProduto,
                            QtdePermitida = p.QtdePermitida
                        })
                        .FirstOrDefaultAsync();

            _ = produto ?? throw new Exception("Produtos inexistentes");

            return produto;
        }
    }
}
