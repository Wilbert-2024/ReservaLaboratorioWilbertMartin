using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservaLaboratorioWilbertMartin.Models
{
    [Table("ReservasLaboratorio")]
    public class ReservaLaboratorio
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Docente")]
        public int DocenteId { get; set; }
        public virtual Docente Docente { get; set; }

        [ForeignKey("Laboratorio")]
        public int LaboratorioId { get; set; }
        public virtual Laboratorio Laboratorio { get; set; }

        public DateTime Fecha { get; set; }
        public TimeSpan HoraReserva { get; set; }

        public string Motivo { get; set; }

        public string Estado { get; set; } = "Pendiente"; // Pendiente, Aprobada, Rechazada

        [ForeignKey("Administrador")]
        public int? AdministradorId { get; set; }
        public virtual Administrador Administrador { get; set; }
    }
}
