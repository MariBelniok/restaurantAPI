using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestauranteDominio
{
    public class Mesa
    {
        [Key]
        public int MesaId { get; set; } //PK
        public int Capacidade { get; set; }
        public bool MesaOcupada { get; set; }
    }
}
