using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestauranteDominio
{
    public class Produto
    {
        [Key]

        public int ProdutoId { get; set; }

        public string ImagemProduto { get; set; }

        public string NomeProduto { get; set; }

        public double ValorProduto { get; set; }

        public int  QtdePermitida { get; set; }

        public bool Disponivel { get; set; }
    }
}
