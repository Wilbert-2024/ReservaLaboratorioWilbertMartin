using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservaLaboratorioWilbertMartin.Models
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Relación de uno a mucho
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
