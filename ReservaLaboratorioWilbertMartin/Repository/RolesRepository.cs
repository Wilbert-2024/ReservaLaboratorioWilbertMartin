using Microsoft.EntityFrameworkCore;
using ReservaLaboratorioWilbertMartin.Data;
using ReservaLaboratorioWilbertMartin.Models;

namespace ReservaLaboratorioWilbertMartin.Repository
{
    public class RolesRepository(AppDbContext context) : IRolesRepository
    {
        private readonly AppDbContext _context = context;


        public async Task ActualizarAsync(Role role)
        {
            _context.roles.Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task<Role> AgregarAsync(Role role)
        {
            _context.roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task EliminarAsync(int id)
        {
            var role = await _context.roles.FindAsync(id);
            if (role != null)
            {
                _context.roles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Role?> ObtenerPorIdAsync(int id)
        {
            return await _context.roles.FindAsync(id);
        }

        public async Task<Role?> ObtenerPorNombreAsync(string nombre)
        {
            return await _context.roles.FirstOrDefaultAsync(r => r.Name == nombre); //FirstOrDefaultAsync
        }

        public async Task<IEnumerable<Role>> ObtenerTodosAsync()
        {
            return await _context.roles.ToListAsync();
        }
    
}
}
