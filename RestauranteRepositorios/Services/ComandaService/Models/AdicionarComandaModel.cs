using System;

namespace RestauranteRepositorios.Services
{
    public class AdicionarComandaModel
    {
        public int PedidoId { get; set; }
        public DateTime DataHoraEntrada { get; set; }
        public double Valor { get; set; }
        public bool ComandaPaga { get; set; }
        public int QtdePessoasMesa { get; set; }
        public int MesaId { get; set; }
    }
}
