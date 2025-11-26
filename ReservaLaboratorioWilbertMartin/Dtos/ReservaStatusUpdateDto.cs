using System.ComponentModel.DataAnnotations;

namespace ReservaLaboratorioWilbertMartin.Dtos
{
    public class ReservaStatusUpdateDto
    {
        /// El motivo por el cual se rechaza la reserva.   
        [Required(ErrorMessage = "El motivo del rechazo es obligatorio.")]
        [StringLength(500, ErrorMessage = "El motivo no puede exceder los 500 caracteres.")]
        public string? Motivo { get; set; }
    }
}
