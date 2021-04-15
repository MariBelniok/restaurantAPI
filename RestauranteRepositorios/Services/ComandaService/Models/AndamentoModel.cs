using System;
using System.Collections.Generic;

namespace RestauranteRepositorios.Services
{
    public class AndamentoModel
    {
        public int ComandaId { get; set; }

        public DateTime DataHoraEntrada { get; set; }

        public double Valor { get; set; }

        public bool ComandaPaga { get; set; }

        public int QtdePessoasMesa { get; set; }

        public int MesaId { get; set; }

        public bool? Cancelada { get; set; }
        public ICollection<BuscarModel> Pedidos { get; set; }
    }
}
