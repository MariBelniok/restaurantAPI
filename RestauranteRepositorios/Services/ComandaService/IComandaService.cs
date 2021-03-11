using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestauranteRepositorios.Services.ComandaService
{
    public interface IComandaService
    {
        void AdicionarComanda(AdicionarComandaModel model);
        void EncerrarComanda(int mesaId);
        void CancelarComanda(int mesaId);
        Task<ComandaFinalizadaModel> BuscarComandaPaga(int comandaId);
        Task<ComandaModel> BuscarComandaAberta(int comandaId);
        double ValorTotalComanda(int comandaId);
    }
}
