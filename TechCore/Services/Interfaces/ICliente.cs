using TechCore.Models.DTO.Cliente;

namespace TechCore.Services.Interfaces
{
    public interface ICliente
    {
        Task<ClientePaginadoDTO> ObtenerPaginadoAsync(
             string? busqueda,
             int pagina,
             int? idCreador = null,
             int tamanoPagina = 10);

        Task<ClienteFormDTO?> ObtenerPorCodigoAsync(
            string codclien,
            int? idCreador = null);

        Task<string> GenerarCodigoAsync();

        Task<(bool exito, string mensaje)> CrearAsync(ClienteFormDTO dto);

        Task<(bool exito, string mensaje)> EditarAsync(
            ClienteFormDTO dto,
            int? idCreador = null); 

        Task<(bool exito, string mensaje)> DesactivarAsync(
            string codclien,
            int? idCreador = null); 
    }
}
