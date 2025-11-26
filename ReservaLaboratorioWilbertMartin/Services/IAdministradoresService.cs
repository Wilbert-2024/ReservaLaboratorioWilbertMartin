using ReservaLaboratorioWilbertMartin.Models;

namespace ReservaLaboratorioWilbertMartin.Services
{
    public interface IAdministradoresService
    {
        Task<IEnumerable<ReservaLaboratorio>> ObtenerReservasPendientesAsync();
        Task AprobarReservaAsync(int reservaId, int administradorId);
        Task RechazarReservaAsync(int reservaId, int administradorId, string motivoRechazo);
    }
}
