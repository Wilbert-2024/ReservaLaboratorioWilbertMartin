using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservaLaboratorioWilbertMartin.Models;
using ReservaLaboratorioWilbertMartin.Services;

namespace ReservaLaboratorioWilbertMartin.Controllers
{
    [Authorize] // Cualquier usuario autenticado (Docente o Admin) puede ver los laboratorios
    public class LaboratoriosController(ILaboratoriosService laboratoriosService) : Controller
    {
        private readonly ILaboratoriosService _laboratoriosService = laboratoriosService;

        #region API ENDPOINTS

  
        /// Obtiene una lista de todos los laboratorios disponibles en el sistema (API Endpoint).

        /// <returns>Una lista de objetos Laboratorio en formato JSON.</returns>
        [HttpGet]
        [Route("api/[controller]")] // Ruta de API explícita
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var laboratorios = await _laboratoriosService.ObtenerTodosAsync();
                return Ok(new { success = true, data = laboratorios });
            }
            catch (Exception ex)
            {
                // Loguear el error ex
                return StatusCode(500, new { success = false, errorMessage = "Ocurrió un error al obtener la lista de laboratorios." });
            }
        }

        /// <summary>
        /// Obtiene los detalles de un laboratorio específico por su ID (API Endpoint).
        /// </summary>
        /// <param name="id">El ID del laboratorio a buscar.</param>
        /// <returns>El objeto Laboratorio si se encuentra. Retorna un código 404 Not Found si no existe.</returns>
        [HttpGet]
        [Route("api/[controller]/{id}")] // Ruta de API explícita
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { success = false, errorMessage = "ID de laboratorio inválido." });
            }

            try
            {
                var laboratorio = await _laboratoriosService.ObtenerPorIdAsync(id);
                if (laboratorio == null)
                {
                    return NotFound(new { success = false, errorMessage = "Laboratorio no encontrado." });
                }
                return Ok(new { success = true, data = laboratorio });
            }
            catch (Exception ex)
            {
                // Loguear el error ex
                return StatusCode(500, new { success = false, errorMessage = "Ocurrió un error al obtener los detalles del laboratorio." });
            }
        }

        /// <summary>
        /// Verifica la disponibilidad de un laboratorio para una fecha y hora específicas (API Endpoint).
        /// </summary>
        /// <param name="id">El ID del laboratorio a verificar.</param>
        /// <param name="fecha">La fecha para la cual se quiere verificar la disponibilidad (ej: 2023-12-25).</param>
        /// <param name="horaReserva">La hora a verificar en formato HH:mm (ej: 10:00).</param>
        /// <returns>Un valor booleano: true si está disponible, false si no lo está. Retorna un código 200 OK.</returns>
        [HttpGet]
        [Route("api/[controller]/{id}/disponibilidad")] // Ruta de API explícita
        public async Task<IActionResult> VerDisponibilidad(int id, [FromQuery] DateTime fecha, [FromQuery] string horaReserva)
        {
            if (id <= 0)
            {
                return BadRequest(new { success = false, errorMessage = "ID de laboratorio inválido." });
            }

            if (string.IsNullOrWhiteSpace(horaReserva))
            {
                return BadRequest(new { success = false, errorMessage = "La hora de reserva es obligatoria." });
            }

            try
            {
                var disponible = await _laboratoriosService.EstaDisponibleAsync(id, fecha, horaReserva);
                return Ok(new { success = true, data = disponible });
            }
            catch (Exception ex)
            {
                // Loguear el error ex
                return StatusCode(500, new { success = false, errorMessage = "Ocurrió un error al verificar la disponibilidad." });
            }
        }

        #endregion
    }
}