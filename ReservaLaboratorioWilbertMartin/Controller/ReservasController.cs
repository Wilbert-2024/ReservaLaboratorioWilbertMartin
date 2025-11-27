using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservaLaboratorioWilbertMartin.Models;
using ReservaLaboratorioWilbertMartin.Repository;
using System.Security.Claims;

namespace ReservaLaboratorioWilbertMartin.Controllers
{
    // Este controlador maneja la visualización de reservas.
    // Los usuarios autenticados (Docentes y Admins) pueden ver detalles.
    // Solo los Admins pueden ver la lista completa de todas las reservas.
    public class ReservasController(IReservasRepository reservasRepository) : Controller
    {
        private readonly IReservasRepository _reservasRepository = reservasRepository;

        /// Método de ayuda para obtener el ID del usuario autenticado desde el token JWT.

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            throw new Exception("ID de usuario no encontrado en el token de autenticación.");
        }




        /// Muestra la página de detalles de una reserva específica.
        /// Accesible para usuarios autenticados (Docentes y Admins).

        /// <param name="id">El ID de la reserva a visualizar.</param>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Detalles(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID de reserva no válido.");
            }

            // Obtenemos la reserva con todos los detalles (docente, laboratorio, administrador)
            var reserva = await _reservasRepository.ObtenerConDetallesAsync(id);

            if (reserva == null)
            {
                return NotFound("Reserva no encontrada.");
            }

            // --- LÓGICA DE AUTORIZACIÓN GRANULAR ---
            // Un docente solo puede ver sus propias reservas. Un administrador puede ver cualquiera.
            var currentUserId = GetCurrentUserId();
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin)
            {
                // El usuario no es admin y la reserva no le pertenece.
                return Forbid(); // Devuelve un error 403 Forbidden
            }

            return View(reserva);
        }



      

        /// <summary>
        /// Obtiene los detalles de una reserva específica en formato JSON (API Endpoint).
        /// Requiere autenticación.
        /// </summary>
        /// <param name="id">El ID de la reserva a obtener.</param>
        /// <returns>Un objeto JSON con los detalles de la reserva.</returns>
        [HttpGet]
        [Authorize]
        [Route("api/[controller]/{id}")] // Ruta de API explícita para evitar conflictos
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { success = false, errorMessage = "ID de reserva inválido." });
            }

            try
            {
                var reserva = await _reservasRepository.ObtenerConDetallesAsync(id);
                if (reserva == null)
                {
                    return NotFound(new { success = false, errorMessage = "Reserva no encontrada." });
                }

                // La misma lógica de autorización que en la vista
                var currentUserId = GetCurrentUserId();
                var isAdmin = User.IsInRole("Admin");
                if (!isAdmin )
                {
                    return Forbid(); // Devuelve un 403 Forbidden
                }

                return Ok(new { success = true, data = reserva });
            }
            catch (Exception ex)
            {
                // Loguear el error ex aquí
                return StatusCode(500, new { success = false, errorMessage = "Ocurrió un error al obtener la reserva." });
            }
        }

        /// <summary>
        /// Obtiene una lista de todas las reservas del sistema (API Endpoint).
        /// Este endpoint es exclusivo para Administradores.
        /// </summary>
        /// <returns>Una lista de todas las reservas en formato JSON.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/[controller]")] // Ruta de API explícita
        public async Task<IActionResult> ObtenerTodas()
        {
            try
            {
                var reservas = await _reservasRepository.ObtenerTodasAsync();
                return Ok(new { success = true, data = reservas });
            }
            catch (Exception ex)
            {
                // Loguear el error ex aquí
                return StatusCode(500, new { success = false, errorMessage = "Ocurrió un error al obtener las reservas." });
            }
        }


    } 
}