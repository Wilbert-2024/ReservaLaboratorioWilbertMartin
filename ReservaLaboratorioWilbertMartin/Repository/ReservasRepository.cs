using Microsoft.EntityFrameworkCore;
using ReservaLaboratorioWilbertMartin.Data;
using ReservaLaboratorioWilbertMartin.Models;

namespace ReservaLaboratorioWilbertMartin.Repository
{
    public class ReservasRepository(AppDbContext context) : IReservasRepository
    {
        private readonly AppDbContext _context = context;

        public async Task ActualizarAsync(ReservaLaboratorio reserva)
        {
            _context.ReservasLaboratorio.Update(reserva);
            await _context.SaveChangesAsync();
        }

        public async Task<ReservaLaboratorio> AgregarAsync(ReservaLaboratorio reserva)
        {
            await _context.ReservasLaboratorio.AddAsync(reserva);
            await _context.SaveChangesAsync();
            return reserva;
        }

        public async Task EliminarAsync(int id)
        {
            var reserva = await _context.ReservasLaboratorio.FindAsync(id);
            if (reserva != null)
            {
                _context.ReservasLaboratorio.Remove(reserva);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteReservaSolapadaAsync(ReservaLaboratorio nuevaReserva)
        {
            return await _context.ReservasLaboratorio
                 .AnyAsync(r =>  r.LaboratorioId == nuevaReserva.LaboratorioId &&  r.Fecha.Date == nuevaReserva.Fecha.Date &&
                     r.Estado == "Aprobada" &&  r.Id != nuevaReserva.Id &&
                     (
                         (nuevaReserva.HoraInicio >= r.HoraInicio && nuevaReserva.HoraInicio < r.HoraFin) ||
                         (nuevaReserva.HoraFin > r.HoraInicio && nuevaReserva.HoraFin <= r.HoraFin) ||
                         (nuevaReserva.HoraInicio <= r.HoraInicio && nuevaReserva.HoraFin >= r.HoraFin)
                     )
                 );
        }

        public async Task<ReservaLaboratorio?> ObtenerConDetallesAsync(int id)
        {
            return await _context.ReservasLaboratorio.Include(r => r.Docente).ThenInclude(d => d.User).Include(r => r.Laboratorio).Include(r => r.MonitorLab).ThenInclude(m => m.User).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<ReservaLaboratorio>> ObtenerPendientesAsync()
        {
            return await _context.ReservasLaboratorio
                .Where(r => r.Estado == "Pendiente")
                .Include(r => r.Docente).ThenInclude(d => d.User)
                .Include(r => r.Laboratorio)
                .ToListAsync();
        }

        public async Task<IEnumerable<ReservaLaboratorio>> ObtenerPorDocenteIdAsync(int docenteId)
        {
            return await _context.ReservasLaboratorio.Where(r => r.Estado == "Pendiente").Include(r => r.Docente).ThenInclude(d => d.User).Include(r => r.Laboratorio).ToListAsync();
        }

        public async Task<ReservaLaboratorio?> ObtenerPorIdAsync(int id)
        {
            return await _context.ReservasLaboratorio.FindAsync(id);
        }

        public async Task<IEnumerable<ReservaLaboratorio>> ObtenerTodasAsync()
        {
            return await _context.ReservasLaboratorio.Include(r => r.Docente).ThenInclude(d => d.User).Include(r => r.Laboratorio)
                 .Include(r => r.MonitorLab).ThenInclude(m => m.User).ToListAsync();

        }
    }
}
