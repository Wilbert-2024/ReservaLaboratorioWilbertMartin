using Microsoft.EntityFrameworkCore;
using ReservaLaboratorioWilbertMartin.Data;
using ReservaLaboratorioWilbertMartin.Models;

namespace ReservaLaboratorioWilbertMartin.Repository
{
    public class MonitoresRepository(AppDbContext context) : AdministradorRepository
    {
        private readonly AppDbContext _context = context;
        public async Task ActualizarAsync(Administrador monitor)
        {
            _context.MonitorLab.Update(monitor);
            await _context.SaveChangesAsync();
        }

        public async Task<Administrador> AgregarAsync(Administrador monitor)
        {
            await _context.MonitorLab.AddAsync(monitor);
            await _context.SaveChangesAsync();
            return monitor;
        }

        public async Task EliminarAsync(int id)
        {
            var monitor = await _context.MonitorLab.FindAsync(id);
            if (monitor != null)
            {
                _context.MonitorLab.Remove(monitor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Administrador?> ObtenerPorIdAsync(int id)
        {
            return await _context.MonitorLab.Include(m => m.User).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Administrador?> ObtenerPorUserIdAsync(int userId)
        {
            return await _context.MonitorLab.Include(m => m.User).FirstOrDefaultAsync(m => m.UserId == userId);
        }

        public async Task<IEnumerable<Administrador>> ObtenerTodosAsync()
        {
            return await _context.MonitorLab.Include(m => m.User).ToListAsync();
        }
    }
}
