namespace TechCore.Models.DTO.Compra
{
    public class CompraDetalleDTO
    {
        public int Id { get; set; }
        public string CodProd { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
    }
}
