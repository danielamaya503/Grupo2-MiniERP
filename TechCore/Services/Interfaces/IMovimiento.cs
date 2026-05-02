using TechCore.Models.DTO.Movimiento;

namespace TechCore.Services.Interfaces
{
    public interface IMovimiento
    {
        Task<MovimientoPaginadoDTO> ObtenerHistorialAsync(MovimientoFiltroDTO filtro);
        Task<List<MovimientoDTO>> ObtenerTodosAsync(MovimientoFiltroDTO filtro);
        Task<List<int>> ObtenerAniosDisponiblesAsync();
    }
}
