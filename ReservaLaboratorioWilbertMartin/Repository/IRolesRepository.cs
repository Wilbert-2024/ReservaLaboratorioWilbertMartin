using ReservaLaboratorioWilbertMartin.Models;

namespace ReservaLaboratorioWilbertMartin.Repository
{
    public interface IRolesRepository
    {
        Task<Role> AgregarAsync(Role role);

        Task<Role?> ObtenerPorIdAsync(int id);

        Task<Role?> ObtenerPorNombreAsync(string nombre);

        Task<IEnumerable<Role>> ObtenerTodosAsync();

        Task ActualizarAsync(Role role);

        Task EliminarAsync(int id);
    }
}
