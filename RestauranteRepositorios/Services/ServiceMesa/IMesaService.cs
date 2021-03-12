using RestauranteDominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestauranteRepositorios.Services.ServiceMesa
{
    public interface IMesaService
    {
        Task OcuparMesa(int mesaId);
        Task DesocuparMesa(int mesaId);
        Task<List<int>> BuscarMesasDisponiveis();
    }
}
