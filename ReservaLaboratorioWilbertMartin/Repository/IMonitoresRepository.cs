using ReservaLaboratorioWilbertMartin.Models;

namespace ReservaLaboratorioWilbertMartin.Repository
{
    public interface IMonitoresRepository
    {
        Task<MonitorLab> AgregarAsync(MonitorLab monitor);
        Task<MonitorLab?> ObtenerPorIdAsync(int id);
        Task<MonitorLab?> ObtenerPorUserIdAsync(int userId);
        Task<IEnumerable<MonitorLab>> ObtenerTodosAsync();
        Task ActualizarAsync(MonitorLab monitor);
        Task EliminarAsync(int id);
    }
}
