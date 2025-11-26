using System.ComponentModel.DataAnnotations;

namespace ReservaLaboratorioWilbertMartin.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [MaxLength(50, ErrorMessage = "El nombre de usuario no puede tener más de 50 caracteres")]
        [MinLength(4, ErrorMessage = "El nombre de usuario debe tener al menos 4 caracteres")]
        [Display(Name = "Nombre de usuario", Prompt = "Ingrese el nombre de usuario")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        [MaxLength(50, ErrorMessage = "El email no puede tener más de 50 caracteres")]
        [Display(Name = "Correo electrónico", Prompt = "Ingrese el correo electrónico")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MaxLength(100, ErrorMessage = "La contraseña no puede superar 100 caracteres")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña", Prompt = "Ingrese la contraseña")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Correo confirmado")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Token de confirmación de email")]
        public string? EmailConfirmationToken { get; set; }

        [Display(Name = "Refrescar Token")]
        public string? RefreshToken { get; set; }

        [Display(Name = "Expiración del Refrescar Token")]
        public DateTime? RefreshTokenExpiryTime { get; set; }

        [Display(Name = "Token para reiniciar contraseña")]
        public string? PasswordResetToken { get; set; }

        [Display(Name = "Expiración del token de reinicio")]
        public DateTime? ResetTokenExpiryTime { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un rol")]
        [Display(Name = "Rol del usuario")]
        public int RoleId { get; set; }

        [Display(Name = "Nombre del rol")]
        public string? RoleName { get; set; }


    }
}
