namespace TechCore.Models.DTO.Producto;

public class ProductoListDTO
{
    public string CodProd { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int? CodCategoria { get; set; }
    public string? Categoria { get; set; }
    public decimal PrecioCompra { get; set; }
    public decimal PrecioVenta { get; set; }
    public int Stock { get; set; }
    public int StockMinimo { get; set; }
    public bool Estado { get; set; }
    public DateTime CreatedDate { get; set; }


    public bool StockBajo => Stock <= StockMinimo;
}
