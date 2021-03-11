using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestauranteRepositorios.Services
{ 
    public interface IProdutoService
    {
        Task<List<ListarProdutosModel>> BuscarProdutoDisponivel();
        Task<List<ListarProdutosModel>> ListarMenu();
    }
}
