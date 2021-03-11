using RestauranteDominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestauranteRepositorios.Services.PedidoService
{
    public interface IPedidoService
    {
        void AdicionarPedido(AdicionarPedidoModel model);
        void AtualizarPedido(int pedidoId, int comandaId, int quantidadeItem);
        void RemoverPedido(int pedidoId, int comandaId);
        Task<List<BuscarPedidoModel>> BuscarPedidos(int comandaId);
        double ValorTotalPedido(int prodId, int quantidade);
        bool QtdeValida(int prodId, int qtdeEscolhida, int comandaId);
    }
}
