﻿using RestauranteDominio.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestauranteDominio
{
    public class Pedido
    {
        [Key]
        public int PedidoId { get; set; } //PK

        public int ComandaId { get; set; } //FK
        [ForeignKey("ComandaId")]
        public Comanda Comanda { get; set; }

        public int ProdutoId { get; set; } //FK
        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }
        
        public int QtdeProduto { get; set; }
        public double ValorPedido { get; set; }

        public int StatusPedidoId { get; set; } //FK
        [ForeignKey("StatusPedidoId")]
        public StatusPedido StatusPedido { get; set; }

    }
}
