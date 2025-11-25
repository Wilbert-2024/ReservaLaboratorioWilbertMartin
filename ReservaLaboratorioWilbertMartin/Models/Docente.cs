using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservaLaboratorioWilbertMartin.Models
{
    [Table("Docentes")]
    public class Docente
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public string Asignatura { get; set; }


        // Relación con reservas
        public ICollection<ReservaLaboratorio> Reservas { get; set; } = new List<ReservaLaboratorio>();

    }
}
