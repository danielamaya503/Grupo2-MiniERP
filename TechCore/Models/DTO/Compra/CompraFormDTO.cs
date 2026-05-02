using System.ComponentModel.DataAnnotations;

namespace TechCore.Models.DTO.Compra
{
    public class CompraFormDTO
    {
        [Required(ErrorMessage = "Selecciona un proveedor")]
        public string CodProv { get; set; } = string.Empty;

        [Required]
        [MinLength(1, ErrorMessage = "Agrega al menos un producto")]
        public List<CompraDetalleFormDTO> Detalle { get; set; } = new();
    }

    public class CompraDetalleFormDTO
    {
        [Required]
        public string CodProd { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Cantidad mínima: 1")]
        public int Cantidad { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Precio { get; set; }
    }
}
