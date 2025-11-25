using System.ComponentModel.DataAnnotations;

namespace ReservaLaboratorioWilbertMartin.Dtos
{
    public class RolesDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio")]
        [MaxLength(50, ErrorMessage = "El nombre del rol no puede tener más de 50 caracteres")]
        [MinLength(3, ErrorMessage = "El nombre del rol debe tener al menos 3 caracteres")]
        [Display(Name = "Nombre del rol", Prompt = "Ingrese el nombre del rol")]
        public string Name { get; set; } = string.Empty;

    }
}
