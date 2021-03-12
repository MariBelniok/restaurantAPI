using RestauranteDominio.Enums;

namespace RestauranteRepositorios.Services
{
    public class AdicionarPedidoModel
    {
        public int ProdutoId { get; set; }
        public int ComandaId { get; set; }
        public int QtdeProduto { get; set; }
        public double ValorPedido { get; set; }
        public StatusPedidoEnum StatusPedidoId { get; set; }
    }
}
