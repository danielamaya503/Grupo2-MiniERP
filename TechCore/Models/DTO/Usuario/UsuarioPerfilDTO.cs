using System.ComponentModel.DataAnnotations;

namespace TechCore.Models.DTO.Usuario
{
    public class UsuarioPerfilDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty; 

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(200)]
        [Display(Name = "Nombre completo")]
        public string Nombre { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Correo inválido")]
        [Display(Name = "Correo electrónico")]
        public string? Email { get; set; }

        [MaxLength(15)]
        [Phone(ErrorMessage = "Telefono invalido")]
        [Display(Name = "Teléfono")]
        public string? Phone { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        public string? PasswordActual { get; set; }

        [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string? NuevaPassword { get; set; }

        [Compare(nameof(NuevaPassword), ErrorMessage = "Las contraseñas no coinciden")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nueva contraseña")]
        public string? ConfirmarPassword { get; set; }
    }
}
