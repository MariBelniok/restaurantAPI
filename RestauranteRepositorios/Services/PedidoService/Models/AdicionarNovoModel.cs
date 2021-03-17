﻿using RestauranteDominio.Enums;
using System;

namespace RestauranteRepositorios.Services
{
    public class AdicionarNovoModel
    {
        public int ProdutoId { get; set; }
        public int ComandaId { get; set; }
        public int QtdeProduto { get; set; }
        public double ValorPedido { get; set; }
        public StatusPedidoEnum StatusPedidoId { get; set; }

        public void Validar()
        {
            if (QtdeProduto < 1)
                throw new Exception("A quantidade esta invalida!");

            if (StatusPedidoId == StatusPedidoEnum.Cancelado)
                throw new Exception("Todo pedido novo adicionado deve ter status 'recebido' ");
        }
    }
}
