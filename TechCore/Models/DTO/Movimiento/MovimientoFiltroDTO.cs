namespace TechCore.Models.DTO.Movimiento
{
    public class MovimientoFiltroDTO
    {
        public int Mes { get; set; } = DateTime.Now.Month;
        public int Anio { get; set; } = DateTime.Now.Year;
        public string Tipo { get; set; } = ""; 
        public int Pagina { get; set; } = 1;
        public int TamanoPagina { get; set; } = 25;
        public string? CodProd { get; set; }

        // Se calculan automáticamente desde Mes/Anio
        public DateTime? FechaDesde => new DateTime(Anio, Mes, 1);
        public DateTime? FechaHasta => new DateTime(Anio, Mes,
                                           DateTime.DaysInMonth(Anio, Mes), 23, 59, 59);
    }
}
