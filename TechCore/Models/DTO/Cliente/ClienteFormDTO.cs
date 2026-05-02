using System.ComponentModel.DataAnnotations;

namespace TechCore.Models.DTO.Cliente
{
    public class ClienteFormDTO
    {
        public string? Codclien { get; set; }
        public int IdCreador { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(200, ErrorMessage = "Máximo 200 caracteres.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Teléfono no válido.")]
        [MaxLength(15, ErrorMessage = "Máximo 15 caracteres.")]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "Correo electrónico no válido.")]
        [MaxLength(200, ErrorMessage = "Máximo 200 caracteres.")]
        [Display(Name = "Correo electrónico")]
        public string? Email { get; set; }

        [MaxLength(300, ErrorMessage = "Máximo 300 caracteres.")]
        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }
    }
}
