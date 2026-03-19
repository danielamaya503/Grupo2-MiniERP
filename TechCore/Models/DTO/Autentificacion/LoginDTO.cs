using System.ComponentModel.DataAnnotations;

namespace TechCore.Models.DTO.Autentificacion
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El usuario es requierido")]
        public required string NombreUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        public required string Contrasenia { get; set; }

        public bool Recordatorio { get; set; }
    }
}
