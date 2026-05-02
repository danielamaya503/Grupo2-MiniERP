using TechCore.Models.DTO.Proveedor;

namespace TechCore.Services.Interfaces.Proveedor
{
    public interface IProveedor
    {
        Task<ProveedorPaginadoDTO> ObtenerPaginadoAsync(
           string? busqueda, int pagina, int tamanoPagina = 10);

        Task<ProveedorFormDTO?> ObtenerPorCodigoAsync(string codprovee);

        Task<string> GenerarCodigoAsync();

        Task<(bool exito, string mensaje)> CrearAsync(ProveedorFormDTO dto);

        Task<(bool exito, string mensaje)> EditarAsync(ProveedorFormDTO dto);

        Task<(bool exito, string mensaje)> DesactivarAsync(string codprovee);
    }
}
