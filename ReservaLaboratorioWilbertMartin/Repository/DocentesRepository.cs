using Microsoft.EntityFrameworkCore;
using ReservaLaboratorioWilbertMartin.Data;
using ReservaLaboratorioWilbertMartin.Models;

namespace ReservaLaboratorioWilbertMartin.Repository
{
    public class DocentesRepository(AppDbContext context) : IDocentesRepository
    {
        private readonly AppDbContext _context = context;

        public async Task ActualizarAsync(Docente docente)
        {
            _context.Docentes.Update(docente);
            await _context.SaveChangesAsync();
        }

        public async Task<Docente> AgregarAsync(Docente docente)
        {
            await _context.Docentes.AddAsync(docente);
            await _context.SaveChangesAsync();
            return docente;
        }

        public async Task EliminarAsync(int id)
        {
            var docente = await _context.Docentes.FindAsync(id);
            if (docente != null)
            {
                _context.Docentes.Remove(docente);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Docente?> ObtenerPorIdAsync(int id)
        {
            return await _context.Docentes.Include(d => d.User).FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Docente?> ObtenerPorUserIdAsync(int userId)
        {
                     
        }

        public async Task<IEnumerable<Docente>> ObtenerTodosAsync()
        {
            return await _context.Docentes.Include(d => d.User).ToListAsync();
        }
    }
}
