using System.ComponentModel.DataAnnotations;

namespace ReservaLaboratorioWilbertMartin.Dtos
{
    public class ReservaRequestDto
    {
        /// <summary>
        /// El ID del laboratorio que se desea reservar.
        /// </summary>
        [Required(ErrorMessage = "Debe seleccionar un laboratorio.")]
        public int LaboratorioId { get; set; }

        /// <summary>
        /// La fecha para la cual se quiere hacer la reserva.
        /// </summary>
        [Required(ErrorMessage = "La fecha de la reserva es obligatoria.")]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// La hora de la reserva en formato HH:mm (ej: 10:00, 14:30).
        /// </summary>
        [Required(ErrorMessage = "La hora de la reserva es obligatoria.")]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "El formato de la hora debe ser HH:mm (ej. 09:00, 14:30).")]
        public string HoraReserva { get; set; }

        /// <summary>
        /// El motivo o razón por la cual se necesita el laboratorio.
        /// </summary>
        [Required(ErrorMessage = "El motivo de la reserva es obligatorio.")]
        [StringLength(500, ErrorMessage = "El motivo no puede exceder los 500 caracteres.")]
        public string Motivo { get; set; }
    }
}