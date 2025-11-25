using Microsoft.EntityFrameworkCore;
using ReservaLaboratorioWilbertMartin.Data;
using ReservaLaboratorioWilbertMartin.Models;

namespace ReservaLaboratorioWilbertMartin.Repository
{
    public class MonitoresRepository(AppDbContext context) : AdministradorRepository
    {
        private readonly AppDbContext _context = context;
        public async Task ActualizarAsync(Administrador admi)
        {
            _context.Administrador.Update(admi);
            await _context.SaveChangesAsync();
        }

        public async Task<Administrador> AgregarAsync(Administrador admi)
        {
            await _context.Administrador.AddAsync(admi);
            await _context.SaveChangesAsync();
            return admi;
        }

        public async Task EliminarAsync(int id)
        {
            var admi = await _context.Administrador.FindAsync(id);
            if (admi != null)
            {
                _context.Administrador.Remove(admi);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Administrador?> ObtenerPorIdAsync(int id)
        {
            return await _context.Administrador.Include(m => m.User).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Administrador?> ObtenerPorUserIdAsync(int userId)
        {
            return await _context.Administrador.Include(m => m.User).FirstOrDefaultAsync(m => m.UserId == userId);
        }

        public async Task<IEnumerable<Administrador>> ObtenerTodosAsync()
        {
            return await _context.Administrador.Include(m => m.User).ToListAsync();
        }
    }
}
