namespace TechCore.Models.DTO.Movimiento
{
    public class MovimientoFiltroDTO
    {
        public string? Tipo { get; set; } // null = todos
        public string? CodProd { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
    }
}
