using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservaLaboratorioWilbertMartin.Models
{
    [Table("Administrador")]
    public class Administrador
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public ICollection<ReservaLaboratorio> ReservasAprobadas { get; set; } = new List<ReservaLaboratorio>();

    }
}
