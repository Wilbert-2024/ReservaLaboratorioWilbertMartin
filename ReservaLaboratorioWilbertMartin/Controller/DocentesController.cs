using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Necesario para SelectListItem
using ReservaLaboratorioWilbertMartin.Dtos;
using ReservaLaboratorioWilbertMartin.Models;
using ReservaLaboratorioWilbertMartin.Services;
using System.Security.Claims;

namespace ReservaLaboratorioWilbertMartin.Controllers
{
    [Authorize] // Todos los endpoints de este controlador requieren autenticación
    public class DocentesController( IDocentesService docentesService,   ILaboratoriosService laboratoriosService   ) : Controller
    {
        private readonly IDocentesService _docentesService = docentesService;
        private readonly ILaboratoriosService _laboratoriosService = laboratoriosService;
      

        /// <summary>
        /// Método de ayuda para obtener el ID del usuario autenticado desde el token JWT.
        /// Asume que el token tiene un claim llamado "NameIdentifier" (el estándar para el ID de usuario).
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
        /// Muestra la página principal del panel del docente (dashboard).
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

 
        /// Muestra la página con el formulario para crear una nueva reserva.
        /// Prepara las listas de laboratorios y horas para los menús desplegables.
   
        [HttpGet]
        public async Task<IActionResult> CrearReserva()
        {
            // Obtenemos la lista de laboratorios para el desplegable del formulario
            var laboratorios = await _laboratoriosService.ObtenerTodosAsync();
            ViewBag.Laboratorios = new SelectList(laboratorios, "Id", "Nombre");

            return View();
        }

  
        /// Muestra la página con el listado de todas las reservas del docente autenticado.
     
        [HttpGet]
        public IActionResult MisReservas()
        {
            return View();
        }

        #endregion

        #region API ENDPOINTS

        /// <summary>
        /// Crea una nueva reserva de laboratorio (API Endpoint).
        /// </summary>
        /// <param name="dto">Datos de la reserva a crear.</param>
        /// <returns>Un objeto JSON con el resultado de la operación.</returns>
        [HttpPost]
        public async Task<IActionResult> CrearReserva([FromBody] ReservaRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, errorMessage = "Datos inválidos." });

            try
            {
                var docenteId = GetCurrentUserId();
                var nuevaReserva = new ReservaLaboratorio
                {
                    DocenteId = docenteId,
                    LaboratorioId = dto.LaboratorioId,
                    Fecha = dto.Fecha,
                    HoraReserva = dto.HoraReserva,
                    Motivo = dto.Motivo
                };

                await _docentesService.CrearReservaAsync(nuevaReserva);
                return Ok(new { success = true, message = "Reserva creada exitosamente. Está pendiente de aprobación." });
            }
            catch (InvalidOperationException ex)
            {
                // Error de negocio, como un laboratorio ya reservado
                return Conflict(new { success = false, errorMessage = ex.Message });
            }
            catch (Exception ex)
            {
                // Error inesperado del servidor
                // Loguear el error ex aquí
                return StatusCode(500, new { success = false, errorMessage = "Ocurrió un error inesperado en el servidor." });
            }
        }

        /// <summary>
        /// Obtiene todas las reservas del docente autenticado (API Endpoint).
        /// </summary>
        /// <returns>Una lista de reservas en formato JSON.</returns>
        [HttpGet]
        public async Task<IActionResult> ObtenerMisReservas()
        {
            try
            {
                var docenteId = GetCurrentUserId();
                var reservas = await _docentesService.ObtenerMisReservasAsync(docenteId);
                return Ok(new { success = true, data = reservas });
            }
            catch (Exception ex)
            {
                // Loguear el error ex aquí
                return StatusCode(500, new { success = false, errorMessage = "Ocurrió un error al obtener las reservas." });
            }
        }

        /// <summary>
        /// Cancela una reserva específica del docente autenticado (API Endpoint).
        /// </summary>
        /// <param name="id">El ID de la reserva a cancelar.</param>
        /// <returns>Un objeto JSON con el resultado de la operación.</returns>
        [HttpPut]
        public async Task<IActionResult> CancelarReserva(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { success = false, errorMessage = "ID de reserva inválido." });
            }

            try
            {
                var docenteId = GetCurrentUserId();
                await _docentesService.CancelarReservaAsync(id, docenteId);
                return Ok(new { success = true, message = "Reserva cancelada exitosamente." });
            }
            catch (InvalidOperationException ex)
            {
                // Error de negocio, como intentar cancelar una reserva ya aprobada
                return BadRequest(new { success = false, errorMessage = ex.Message });
            }
            catch (Exception ex)
            {
                // Loguear el error ex aquí
                return StatusCode(500, new { success = false, errorMessage = "Ocurrió un error al cancelar la reserva." });
            }
        }

        #endregion
    }
}