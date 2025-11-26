using ReservaLaboratorioWilbertMartin.Models;
using ReservaLaboratorioWilbertMartin.Repository;
using ReservaLaboratorioWilbertMartin.Services;

namespace ReservaLaboratorioWilbertMartin.Services
{
    public class DocentesService(IReservasRepository reservasRepository, IDocentesRepository docentesRepository) : IDocentesService
    {
        private readonly IReservasRepository _reservasRepository = reservasRepository;
        private readonly IDocentesRepository _docentesRepository = docentesRepository;

        public async Task<ReservaLaboratorio> CrearReservaAsync(ReservaLaboratorio nuevaReserva)
        {
            bool existeSolapamiento = await _reservasRepository.ExisteReservaSolapadaAsync(nuevaReserva);
            if (existeSolapamiento)
            {
                throw new InvalidOperationException("El laboratorio ya está reservado para esa hora. Por favor, elige otro horario.");
            }

            nuevaReserva.Estado = "Pendiente";
            return await _reservasRepository.AgregarAsync(nuevaReserva);
        }

        public async Task<IEnumerable<ReservaLaboratorio>> ObtenerMisReservasAsync(int docenteId)
        {
            return await _reservasRepository.ObtenerPorDocenteIdAsync(docenteId);
        }

        public async Task CancelarReservaAsync(int reservaId, int docenteId)
        {
            var reserva = await _reservasRepository.ObtenerPorIdAsync(reservaId);

            if (reserva != null && reserva.DocenteId == docenteId && reserva.Estado == "Pendiente")
            {
                reserva.Estado = "Cancelada";
                await _reservasRepository.ActualizarAsync(reserva);
            }
            else
            {
                throw new InvalidOperationException("No se puede cancelar esta reserva. Puede que ya haya sido procesada o no te pertenezca.");
            }
        }
    }
}