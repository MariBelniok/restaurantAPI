using Microsoft.EntityFrameworkCore;
using RestauranteDominio;

namespace RestauranteRepositorios
{
    public class RestauranteContexto : DbContext
    {
        public RestauranteContexto()
        {
        }

        public RestauranteContexto(DbContextOptions<RestauranteContexto> options) : base(options) { }

        public DbSet<Comanda> Comanda { get; set; }
        public DbSet<Mesa> Mesa { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<StatusPedido> StatusPedido { get; set; }

    }
}
