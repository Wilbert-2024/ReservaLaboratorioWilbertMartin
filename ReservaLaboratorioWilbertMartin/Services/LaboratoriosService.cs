using ReservaLaboratorioWilbertMartin.Models;
using ReservaLaboratorioWilbertMartin.Repository;

namespace ReservaLaboratorioWilbertMartin.Services
{
    public class LaboratoriosService(ILaboratoriosRepository laboratoriosRepository, IReservasRepository reservasRepository) : ILaboratoriosService
    {
        private readonly ILaboratoriosRepository _laboratoriosRepository = laboratoriosRepository;
        private readonly IReservasRepository _reservasRepository = reservasRepository;

        public async Task<Laboratorio?> ObtenerPorIdAsync(int id)
        {
            return await _laboratoriosRepository.ObtenerPorIdAsync(id);
        }

        public async Task<IEnumerable<Laboratorio>> ObtenerTodosAsync()
        {
            return await _laboratoriosRepository.ObtenerTodosAsync();
        }

        public async Task<bool> EstaDisponibleAsync(int laboratorioId, DateTime fecha, string horaReserva)
        {
            var reservaDePrueba = new ReservaLaboratorio
            {
                LaboratorioId = laboratorioId,
                Fecha = fecha,
                HoraReserva = horaReserva
            };

            bool existeReserva = await _reservasRepository.ExisteReservaSolapadaAsync(reservaDePrueba);
            return !existeReserva;
        }
    }
}