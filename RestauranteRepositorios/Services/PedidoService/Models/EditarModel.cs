using System;
using System.Collections.Generic;
using System.Text;

namespace RestauranteRepositorios.Services
{
    public class EditarModel
    {
        public int QtdeProduto { get; set; }

        public double ValorPedido { get; set; }

        public int PedidoId { get; set; }
        public int ComandaId { get; set; }
    }
}
