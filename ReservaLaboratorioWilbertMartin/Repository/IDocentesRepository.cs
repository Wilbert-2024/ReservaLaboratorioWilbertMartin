using ReservaLaboratorioWilbertMartin.Models;

namespace ReservaLaboratorioWilbertMartin.Repository
{
    public interface IDocentesRepository
    {
        Task<Docente> AgregarAsync(Docente docente);
        Task<Docente?> ObtenerPorIdAsync(int id);
        Task<Docente?> ObtenerPorUserIdAsync(int userId);
        Task<IEnumerable<Docente>> ObtenerTodosAsync();
        Task ActualizarAsync(Docente docente);
        Task EliminarAsync(int id);
    }
}
