namespace TechCore.Models.DTO.Movimiento
{
    public class MovimientoPaginadoDTO
    {
        public List<MovimientoDTO> Movimientos { get; set; } = new();

        // Paginación
        public int TotalRegistros { get; set; }
        public int PaginaActual { get; set; }
        public int TamanoPagina { get; set; } = 25;
        public int TotalPaginas => (int)Math.Ceiling((double)TotalRegistros / TamanoPagina);
        public bool TienePaginaAnterior => PaginaActual > 1;
        public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;

        // "Mostrando X–Y de Z"
        public int DesdeRegistro => TotalRegistros == 0 ? 0 : (PaginaActual - 1) * TamanoPagina + 1;
        public int HastaRegistro => Math.Min(PaginaActual * TamanoPagina, TotalRegistros);

        // Filtros activos
        public int Mes { get; set; }
        public int Anio { get; set; }
        public string FiltroTipo { get; set; } = "";

        // Resumen del período
        public int TotalEntradas { get; set; }
        public int TotalSalidas { get; set; }
        public decimal MontoEntradas { get; set; }
        public decimal MontoSalidas { get; set; }
    }
}
