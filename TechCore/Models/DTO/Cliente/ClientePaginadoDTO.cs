namespace TechCore.Models.DTO.Cliente
{
    public class ClientePaginadoDTO
    {
        public List<ClienteListDTO> Clientes { get; set; } = new();
        public int TotalRegistros { get; set; }
        public int PaginaActual { get; set; } = 1;
        public int TamanoPagina { get; set; } = 10;
        public string Busqueda { get; set; } = string.Empty;

        public int TotalPaginas => (int)Math.Ceiling((double)TotalRegistros / TamanoPagina);
        public bool TienePaginaAnterior => PaginaActual > 1;
        public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
    }
}
