using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservaLaboratorioWilbertMartin.Models
{
 
    public class ReservaLaboratorio
    {
        
        public int Id { get; set; }

       
        public int UserId { get; set; }
        public virtual User User { get; set; }

       
        public int LaboratorioId { get; set; }

        public virtual Laboratorio Laboratorio { get; set; }

        public DateTime Fecha { get; set; }
        public string HoraReserva { get; set; }

        public string Motivo { get; set; }

        public string Estado { get; set; } = "Pendiente"; // Pendiente, Aprobada, Rechazada

       
    }
}
