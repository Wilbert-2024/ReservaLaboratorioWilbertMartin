using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservaLaboratorioWilbertMartin.Dtos;
using ReservaLaboratorioWilbertMartin.Models;
using ReservaLaboratorioWilbertMartin.Service;
using ReservaLaboratorioWilbertMartin.Services;
using System.Security.Claims;

namespace ReservaLaboratorioWilbertMartin.Controllers
{
    [Authorize(Roles = "Admin")] // Solo los usuarios con rol "Admin" pueden acceder
    public class AdministradoresController(IAdministradoresService administradoresService) : Controller
    {
        private readonly IAdministradoresService _administradoresService = administradoresService;

        /// <summary>
        /// Método de ayuda para obtener el ID del usuario autenticado desde el token JWT.
        /// </summary>
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            throw new Exception("ID de usuario no encontrado en el token de autenticación.");
        }

        #region VISTAS (MVC)

        /// <summary>
        /// Muestra la página principal del panel del administrador.
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Muestra la página con la lista de reservas pendientes de aprobación.
        /// </summary>
        [HttpGet]
        public IActionResult ReservasPendientes()
        {
            return View();
        }

        #endregion

        #region API ENDPOINTS

        /// <summary>
        /// Obtiene todas las reservas que están en estado "Pendiente" (API Endpoint).
        /// </summary>
        /// <returns>Una lista de reservas pendientes en formato JSON.</returns>
        [HttpGet]
        [Route("api/[controller]/reservas-pendientes")] // Ruta de API explícita
        public async Task<IActionResult> ObtenerReservasPendientes()
        {
            try
            {
                var reservas = await _administradoresService.ObtenerReservasPendientesAsync();
                return Ok(new { success = true, data = reservas });
            }
            catch (Exception ex)
            {
                // Loguear el error ex aquí
                return StatusCode(500, new { success = false, errorMessage = "Ocurrió un error al obtener las reservas pendientes." });
            }
        }

        /// <summary>
        /// Aprueba una reserva específica que se encuentra en estado "Pendiente" (API Endpoint).
        /// </summary>
        /// <param name="id">El ID de la reserva a aprobar.</param>
        /// <returns>Un objeto JSON con el resultado de la operación.</returns>
        [HttpPut]
        [Route("api/[controller]/aprobar/{id}")] // Ruta de API explícita
        public async Task<IActionResult> AprobarReserva(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { success = false, errorMessage = "ID de reserva inválido." });
            }

            try
            {
                var administradorId = GetCurrentUserId();
                await _administradoresService.AprobarReservaAsync(id, administradorId);
                return Ok(new { success = true, message = "Reserva aprobada exitosamente." });
            }
            catch (Exception ex)
            {
                // Loguear el error ex aquí
                return StatusCode(500, new { success = false, errorMessage = "Ocurrió un error al aprobar la reserva." });
            }
        }

        /// <summary>
        /// Rechaza una reserva específica que se encuentra en estado "Pendiente" (API Endpoint).
        /// </summary>
        /// <param name="id">El ID de la reserva a rechazar.</param>
        /// <param name="dto">Objeto que contiene el motivo del rechazo.</param>
        /// <returns>Un objeto JSON con el resultado de la operación.</returns>
        [HttpPut]
        [Route("api/[controller]/rechazar/{id}")] // Ruta de API explícita
        public async Task<IActionResult> RechazarReserva(int id, [FromBody] ReservaStatusUpdateDto dto)
        {
            if (id <= 0)
            {
                return BadRequest(new { success = false, errorMessage = "ID de reserva inválido." });
            }

            if (string.IsNullOrWhiteSpace(dto.Motivo))
            {
                return BadRequest(new { success = false, errorMessage = "El motivo del rechazo es obligatorio." });
            }

            try
            {
                var administradorId = GetCurrentUserId();
                await _administradoresService.RechazarReservaAsync(id, administradorId, dto.Motivo);
                return Ok(new { success = true, message = "Reserva rechazada exitosamente." });
            }
            catch (Exception ex)
            {
                // Loguear el error ex aquí
                return StatusCode(500, new { success = false, errorMessage = "Ocurrió un error al rechazar la reserva." });
            }
        }

        #endregion
    }
}