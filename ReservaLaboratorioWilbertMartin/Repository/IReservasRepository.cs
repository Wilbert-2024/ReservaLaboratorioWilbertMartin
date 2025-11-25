using ReservaLaboratorioWilbertMartin.Models;

namespace ReservaLaboratorioWilbertMartin.Repository
{
    public interface IReservasRepository
    {
        Task<ReservaLaboratorio> AgregarAsync(ReservaLaboratorio reserva);
        Task<ReservaLaboratorio?> ObtenerPorIdAsync(int id);
        Task<ReservaLaboratorio?> ObtenerConDetallesAsync(int id);
        Task<IEnumerable<ReservaLaboratorio>> ObtenerTodasAsync();
        Task<IEnumerable<ReservaLaboratorio>> ObtenerPendientesAsync();
        Task<IEnumerable<ReservaLaboratorio>> ObtenerPorDocenteIdAsync(int docenteId);
        Task<bool> ExisteReservaSolapadaAsync(ReservaLaboratorio nuevaReserva);
        Task ActualizarAsync(ReservaLaboratorio reserva);
        Task EliminarAsync(int id);
    }
}
