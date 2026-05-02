using System.ComponentModel.DataAnnotations;

namespace TechCore.Models.DTO.Categoria
{
    public class CategoriaFormDTO
    {
        public int CodCategoria { get; set; }

        [Required(ErrorMessage = "El código es requerido")]
        [MaxLength(20)]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? Descripcion { get; set; }
    }
}
