using System.ComponentModel.DataAnnotations;

namespace TechCore.Models.DTO.Producto
{
    public class ProductoFormDTO
    {
        public string? CodProdOriginal { get; set; }

        [Required(ErrorMessage = "El código del producto es requerido")]
        [MaxLength(50, ErrorMessage = "Máximo 50 caracteres")]
        public string CodProd { get; set; } = string.Empty;

        public int? CodCategoria { get; set; }

        [MaxLength(500, ErrorMessage = "Máximo 500 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio de compra es requerido")]
        [Range(0.01, 999999.99, ErrorMessage = "Precio de compra inválido")]
        public decimal PrecioCompra { get; set; }

        [Required(ErrorMessage = "El precio de venta es requerido")]
        [Range(0.01, 999999.99, ErrorMessage = "Precio de venta inválido")]
        public decimal PrecioVenta { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock inválido")]
        public int Stock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock mínimo inválido")]
        public int StockMinimo { get; set; } = 5;

        // Diferencia creación vs edición en la vista
        public bool EsEdicion { get; set; }
    }
}
