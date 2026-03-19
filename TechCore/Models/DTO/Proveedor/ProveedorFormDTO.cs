using System.ComponentModel.DataAnnotations;

namespace TechCore.Models.DTO.Proveedor
{
    public class ProveedorFormDTO
    {
        [Required]
        [MaxLength(50)]
        public string CodProvee { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(15)]
        public string? Telefono { get; set; }

        [MaxLength(200)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(300)]
        public string? Direccion { get; set; }
    }
}
