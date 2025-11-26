using ReservaLaboratorioWilbertMartin.Models;

namespace ReservaLaboratorioWilbertMartin.Services
{
    public interface ILaboratoriosService
    {
        Task<IEnumerable<Laboratorio>> ObtenerTodosAsync();
        Task<Laboratorio?> ObtenerPorIdAsync(int id);
        Task<bool> EstaDisponibleAsync(int laboratorioId, DateTime fecha, string horaReserva);
    }
}