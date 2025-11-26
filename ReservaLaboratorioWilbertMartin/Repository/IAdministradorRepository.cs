using ReservaLaboratorioWilbertMartin.Models;

namespace ReservaLaboratorioWilbertMartin.Repository
{
    public interface IAdministradorRepository
    {
        Task<Administrador> AgregarAsync(Administrador administrador);
        Task<Administrador?> ObtenerPorIdAsync(int id);
        Task<Administrador?> ObtenerPorUserIdAsync(int userId);
        Task<IEnumerable<Administrador>> ObtenerTodosAsync();
        Task ActualizarAsync(Administrador administrador);
        Task EliminarAsync(int id);
    }
}
