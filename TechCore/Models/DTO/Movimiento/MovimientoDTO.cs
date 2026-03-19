namespace TechCore.Models.DTO.Movimiento
{
    public class MovimientoDTO
    {
        public string Referencia { get; set; } = string.Empty; 
        public string Tipo { get; set; } = string.Empty;
        public string CodProd { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
        public DateTime Fecha { get; set; }
        public string Origen { get; set; } = string.Empty;
    }
}
