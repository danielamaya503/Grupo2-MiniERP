using TechCore.Models.DTO.Movimiento;

namespace TechCore.Services.Interfaces.Movimiento
{
    public interface IMovimiento
    {
        Task<List<MovimientoDTO>> ObtenerTodosAsync(MovimientoFiltroDTO filtro);
    }
}
