using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestauranteRepositorios.Services.ComandaService
{
    public interface IComandaService
    {
        Task AdicionarComanda(AdicionarComandaModel model);
        Task EncerrarComanda(int mesaId);
        Task CancelarComanda(int mesaId);
        Task<ComandaFinalizadaModel> BuscarComandaPaga(int comandaId);
        Task<ComandaModel> BuscarComandaAberta(int comandaId);
        double ValorTotalComanda(int comandaId);
    }
}
