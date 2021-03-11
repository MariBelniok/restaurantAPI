using RestauranteDominio;
using RestauranteDominio.Enums;

namespace RestauranteRepositorios.Services
{
    public class BuscarPedidoModel
    {
        public int PedidoId { get; set; } 
        public int ComandaId { get; set; } 
        public int ProdutoId { get; set; } 
        public int QtdeProduto { get; set; }
        public double ValorPedido { get; set; }
        public StatusPedidoEnum StatusPedidoId { get; set; }
    }
}
