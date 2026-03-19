using System.ComponentModel.DataAnnotations;

namespace TechCore.Models.DTO.Usuario
{
    public class UsuarioEditAdminDTO
    {
        public int Id { get; set; }

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

        [Required(ErrorMessage = "El rol es requerido")]
        [Display(Name = "Rol")]
        public int IdRol { get; set; }

        // mostrar en la vista
        public string Username { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
