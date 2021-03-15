namespace RestauranteRepositorios.Services
{
    public class AtualizarPedidoModel
    {
        public int ProdutoId { get; set; }
        public int PedidoId { get; set; }
        public int ComandaId { get; set; }
        public int QtdeProduto { get; set; }
    }
}
