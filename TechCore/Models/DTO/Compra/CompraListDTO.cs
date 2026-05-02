namespace TechCore.Models.DTO.Compra
{
    public class CompraListDTO
    {
        public string NOrden { get; set; } = string.Empty;
        public int OrdenN { get; set; }
        public string CodProv { get; set; } = string.Empty;
        public string Proveedor { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public int Estado { get; set; }
        public List<CompraDetalleDTO> Detalle { get; set; } = new();
    }
}
