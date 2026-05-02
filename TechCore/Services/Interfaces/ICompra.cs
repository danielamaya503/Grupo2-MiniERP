using TechCore.Models.DTO.Compra;

namespace TechCore.Services.Interfaces
{
    public interface ICompra
    {
        Task<List<CompraListDTO>> ObtenerTodosAsync();
        Task<List<CompraListDTO>> ObtenerDelMesAsync();
        Task<CompraListDTO?> ObtenerDetalleAsync(string nOrden);
        Task<CompraStatsDTO> ObtenerStatsAsync();
        Task<(bool exito, string mensaje, string? nOrden)> RegistrarAsync(CompraFormDTO dto, int idUsuario);
    }
}
