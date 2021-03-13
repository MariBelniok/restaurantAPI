using RestauranteDominio.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestauranteDominio
{
    public class Pedido
    {
        [Key]
        public int PedidoId { get; set; } 

        public int ComandaId { get; set; } 
        [ForeignKey("ComandaId")]
        public Comanda Comanda { get; set; }

        public int ProdutoId { get; set; } 
        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        public int QtdeProduto { get; set; }
        public double ValorPedido { get; set; }

        public int StatusPedidoId { get; set; } 
        [ForeignKey("StatusPedidoId")]
        public StatusPedido StatusPedido { get; set; }

    }
}
