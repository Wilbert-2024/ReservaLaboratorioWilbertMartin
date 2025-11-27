using ReservaLaboratorioWilbertMartin.Models;

namespace ReservaLaboratorioWilbertMartin.Repository
{
    public interface ILaboratoriosRepository
    {
        Task<Laboratorio> AgregarAsync(Laboratorio laboratorio);
        Task<Laboratorio?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Laboratorio>> ObtenerTodosAsync();
        Task ActualizarAsync(Laboratorio laboratorio);
        Task EliminarAsync(int id);
        Task<bool> EstaDisponibleAsync(int laboratorioId, string dia, string hora);

    }
}
