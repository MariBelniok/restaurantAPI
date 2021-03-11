using RestauranteDominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestauranteRepositorios.Services.ServiceMesa
{
    public interface IMesaService
    {
        void OcuparMesa(int mesaId);
        void DesocuparMesa(int mesaId);
        Task<List<int>> BuscarMesasDisponiveis();
    }
}
