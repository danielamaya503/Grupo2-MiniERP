using TechCore.Models.DTO.Usuario;

namespace TechCore.Services.Interfaces
{
    public interface IUsuario
    {
        Task<List<UsuarioListDTO>> ObtenerTodosAsync();

        Task<List<UsuarioListDTO>> BuscarAsync(string busqueda);

        Task<List<UsuarioListDTO>> ObtenerTop10RecientesAsync();

        Task<UsuarioEditAdminDTO?> ObtenerParaEditarAdminAsync(int id);

        Task<UsuarioPerfilDTO?> ObtenerPerfilAsync(int id);

        Task<(bool exito, string mensaje)> CrearAsync(UsuarioCrearDTO dto);

        Task<(bool exito, string mensaje)> EditarAdminAsync(UsuarioEditAdminDTO dto);

        Task<(bool exito, string mensaje)> RestablecerPasswordAsync(int id);

        Task<(bool exito, string mensaje)> ActualizarPerfilAsync(UsuarioPerfilDTO dto);
        Task<(bool exito, string mensaje)> CambiarPasswordAsync(UsuarioCambiarPasswordDTO dto);

        Task<string> GenerarCodigoAsync();
    }
}
