using System;
using System.Collections.Generic;
using System.Text;

namespace RestauranteRepositorios.Services
{
    public class ComandaModel
    {
        public int ComandaId { get; set; }
        public int PedidoId { get; set; }
        public DateTime DataHoraEntrada { get; set; }
        public double Valor { get; set; }
        public bool ComandaPaga { get; set; }
        public int QtdePessoasMesa { get; set; }
        public int MesaId { get; set; }
        public ICollection<BuscarPedidoModel> Pedidos { get; set; }
    }
}
