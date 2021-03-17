using System;

namespace RestauranteRepositorios.Services.ServiceMesa.Models
{
    public class AtualizarStatusModel
    {
        public int MesaId { get; set; }
        public bool MesaOcupada { get; set; }
    }
}
