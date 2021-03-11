namespace RestauranteRepositorios.Services
{ 
    public class ListarProdutosModel
    {
        public int ProdutoId { get; set; } 
        public string NomeProduto { get; set; }
        public double ValorProduto { get; set; }
        public int QtdePermitida { get; set; }
    }
}
