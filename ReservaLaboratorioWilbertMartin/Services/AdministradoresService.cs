using ReservaLaboratorioWilbertMartin.Models;
using ReservaLaboratorioWilbertMartin.Repository;
using ReservaLaboratorioWilbertMartin.Services;

namespace ReservaLaboratorioWilbertMartin.Services
{
    public class AdministradoresService(IReservasRepository reservasRepository) : IAdministradoresService
    {
        private readonly IReservasRepository _reservasRepository = reservasRepository;

        public async Task<IEnumerable<ReservaLaboratorio>> ObtenerReservasPendientesAsync()
        {
            return await _reservasRepository.ObtenerPendientesAsync();
        }

        public async Task AprobarReservaAsync(int reservaId, int administradorId)
        {
            var reserva = await _reservasRepository.ObtenerPorIdAsync(reservaId);
            if (reserva != null && reserva.Estado == "Pendiente")
            {
                reserva.Estado = "Aprobada";
                reserva.AdministradorId = administradorId;
                await _reservasRepository.ActualizarAsync(reserva);
            }
        }

        public async Task RechazarReservaAsync(int reservaId, int administradorId, string motivoRechazo)
        {
            var reserva = await _reservasRepository.ObtenerPorIdAsync(reservaId);
            if (reserva != null && reserva.Estado == "Pendiente")
            {
                reserva.Estado = "Rechazada";
                reserva.AdministradorId = administradorId;
                reserva.Motivo = motivoRechazo;
                await _reservasRepository.ActualizarAsync(reserva);
            }
        }
    }
}