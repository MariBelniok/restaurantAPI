using RestauranteDominio;
using System;
using System.Collections.Generic;

namespace RestauranteRepositorios.Services
{
    public class ComandaFinalizadaModel
    {
        public int ComandaId { get; set; }
        public int MesaId { get; set; }
        public DateTime DataHoraEntrada { get; set; }
        public DateTime? DataHoraSaida { get; set; }
        public double Valor { get; set; }
        public bool ComandaPaga { get; set; }
        public int QtdePessoasMesa { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
    }
}
