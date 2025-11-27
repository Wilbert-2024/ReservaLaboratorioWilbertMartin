using Microsoft.EntityFrameworkCore;
using ReservaLaboratorioWilbertMartin.Data;
using ReservaLaboratorioWilbertMartin.Models;

namespace ReservaLaboratorioWilbertMartin.Repository
{
    public class LaboratoriosRepository(AppDbContext context) : ILaboratoriosRepository
    {
        private readonly AppDbContext _context = context;

        public async Task ActualizarAsync(Laboratorio laboratorio)
        {
            _context.Laboratorios.Update(laboratorio);
            await _context.SaveChangesAsync();
        }

        public async Task<Laboratorio> AgregarAsync(Laboratorio laboratorio)
        {
            await _context.Laboratorios.AddAsync(laboratorio);
            await _context.SaveChangesAsync();
            return laboratorio;
        }

        public async Task EliminarAsync(int id)
        {
            var laboratorio = await _context.Laboratorios.FindAsync(id);
            if (laboratorio != null)
            {
                _context.Laboratorios.Remove(laboratorio);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Laboratorio?> ObtenerPorIdAsync(int id)
        {
            return await _context.Laboratorios.FindAsync(id);
        }

        public async Task<IEnumerable<Laboratorio>> ObtenerTodosAsync()
        {
            return await _context.Laboratorios.ToListAsync();
        }

        public async Task<bool> EstaDisponibleAsync(int laboratorioId, string dia, string hora)
        {
                  
            return await _context.Laboratorios.AnyAsync(l => l.Id == laboratorioId &&  l.dias_ocupado == dia &&  l.horas_ocupado == hora  );
        }
    }
}
