using ReservaLaboratorioWilbertMartin.Models;

namespace ReservaLaboratorioWilbertMartin.Services
{
    public interface IDocentesService
    {
        Task<ReservaLaboratorio> CrearReservaAsync(ReservaLaboratorio reserva);
        Task<IEnumerable<ReservaLaboratorio>> ObtenerMisReservasAsync(int docenteId);
        Task CancelarReservaAsync(int reservaId, int docenteId);
    }
}
