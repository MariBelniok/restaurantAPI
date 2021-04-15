using RestauranteDominio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestauranteDominio
{
    public class Comanda
    {
        [Key]
        public int ComandaId { get; set; } //PK
        public DateTime DataHoraEntrada { get; set; }
        public DateTime? DataHoraSaida { get; set; }
        public double Valor { get; set; }
        public bool ComandaPaga { get; set; }
        public int QtdePessoasMesa { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
        public int MesaId { get; set; } //FK
        [ForeignKey("MesaId")]
        public Mesa Mesa { get; set; }
        public bool? Cancelada { get; set; }

        public Comanda()
        {
            this.Pedidos = new Collection<Pedido>();
        }
    }
}