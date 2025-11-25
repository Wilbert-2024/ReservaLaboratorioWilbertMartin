using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservaLaboratorioWilbertMartin.Models
{
    [Table("Monitores")]
    public class MonitorLab
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public string Turno { get; set; }   // Mañana, Tarde, Noche

        public ICollection<ReservaLaboratorio> ReservasAprobadas { get; set; } = new List<ReservaLaboratorio>();

    }
}
