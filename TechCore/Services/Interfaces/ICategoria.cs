using TechCore.Models.DTO.Categoria;

namespace TechCore.Services.Interfaces
{
    public interface ICategoria
    {
        Task<List<CategoriaListDTO>> ObtenerTodosAsync();
        Task<CategoriaFormDTO?> ObtenerParaEditarAsync(int id);
        Task<(bool exito, string mensaje)> CrearAsync(CategoriaFormDTO dto);
        Task<(bool exito, string mensaje)> EditarAsync(CategoriaFormDTO dto);
        Task<(bool exito, string mensaje)> EliminarAsync(int id);
    }
}
