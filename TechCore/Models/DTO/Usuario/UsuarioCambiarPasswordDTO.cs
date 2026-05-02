using System.ComponentModel.DataAnnotations;

namespace TechCore.Models.DTO.Usuario
{
    public class UsuarioCambiarPasswordDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ingresa tu contraseña actual")]
        [Display(Name = "Contraseña Actual")]
        public string PasswordActual { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresa la nueva contraseña")]
        [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
        [Display(Name = "Nueva Contraseña")]
        public string NuevaPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirma la nueva contraseña")]
        [Compare("NuevaPassword", ErrorMessage = "Las contraseñas no coinciden")]
        [Display(Name = "Confirmar Nueva Contraseña")]
        public string ConfirmarPassword { get; set; } = string.Empty;
    }
}
