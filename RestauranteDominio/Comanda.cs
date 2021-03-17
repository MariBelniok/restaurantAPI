using RestauranteDominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestauranteDominio
{
    public class Comanda
    {
        [Key]

        public int ComandaId { get; set; }

        public DateTime DataHoraEntrada { get; set; }

        public DateTime? DataHoraSaida { get; set; }

        public double Valor { get; set; }

        public bool ComandaPaga { get; set; }

        public int QtdePessoasMesa { get; set; }

        public ICollection<Pedido> Pedidos { get; set; }

        public int MesaId { get; set; }
        [ForeignKey("MesaId")]

        public Mesa Mesa { get; set; }
    }
}