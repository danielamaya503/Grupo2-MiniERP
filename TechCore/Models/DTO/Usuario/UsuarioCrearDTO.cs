using System.ComponentModel.DataAnnotations;

namespace TechCore.Models.DTO.Usuario
{
    public class UsuarioCrearDTO
    {
        [Required(ErrorMessage = "El código es requerido")]
        [MaxLength(10, ErrorMessage = "Máximo 10 caracteres")]
        [Display(Name = "Código")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(200)]
        [Display(Name = "Nombre completo")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El usuario es requerido")]
        [MaxLength(100)]
        [Display(Name = "Nombre de usuario")]
        public string Username { get; set; } = string.Empty;

        [MaxLength(15)]
        [Display(Name = "Teléfono")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "El rol es requerido")]
        [Display(Name = "Rol")]
        public int IdRol { get; set; }

        [EmailAddress(ErrorMessage = "Correo inválido")]
        [Display(Name = "Correo electrónico")]
        public string? Email { get; set; }
    }
}
