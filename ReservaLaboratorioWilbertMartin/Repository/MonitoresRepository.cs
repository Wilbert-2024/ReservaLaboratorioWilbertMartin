using Microsoft.EntityFrameworkCore;
using ReservaLaboratorioWilbertMartin.Data;
using ReservaLaboratorioWilbertMartin.Models;

namespace ReservaLaboratorioWilbertMartin.Repository
{
    public class MonitoresRepository(AppDbContext context) : IMonitoresRepository
    {
        private readonly AppDbContext _context = context;
        public async Task ActualizarAsync(MonitorLab monitor)
        {
            _context.MonitorLab.Update(monitor);
            await _context.SaveChangesAsync();
        }

        public async Task<MonitorLab> AgregarAsync(MonitorLab monitor)
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

        public async Task<MonitorLab?> ObtenerPorIdAsync(int id)
        {
            return await _context.MonitorLab.Include(m => m.User).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<MonitorLab?> ObtenerPorUserIdAsync(int userId)
        {
            return await _context.MonitorLab.Include(m => m.User).FirstOrDefaultAsync(m => m.UserId == userId);
        }

        public async Task<IEnumerable<MonitorLab>> ObtenerTodosAsync()
        {
            return await _context.MonitorLab.Include(m => m.User).ToListAsync();
        }
    }
}
