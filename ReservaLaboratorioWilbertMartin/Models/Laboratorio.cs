using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservaLaboratorioWilbertMartin.Models
{
    [Table("Laboratorios")]
    public class Laboratorio
    {

        
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public int Capacidad { get; set; }

        public ICollection<ReservaLaboratorio> Reservas { get; set; } 

    }
}
