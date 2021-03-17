using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestauranteDominio
{
    public class StatusPedido
    {
        [Key]

        public int StatusPedidoId { get; set; } //PK

        public string Descricao { get; set; }
    }
}
