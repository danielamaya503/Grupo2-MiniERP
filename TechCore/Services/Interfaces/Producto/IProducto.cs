using TechCore.Models.DTO.Compra;
using TechCore.Models.DTO.Producto;

namespace TechCore.Services.Interfaces.Producto
{
    public interface IProducto
    {
        Task<List<ProductoListDTO>> ObtenerTodosAsync();
        Task<List<ProductoListDTO>> BuscarAsync(string termino);
        Task<List<ProductoListDTO>> ObtenerStockBajoAsync();
        Task<ProductoFormDTO?> ObtenerParaEditarAsync(string codProd);

        Task<(bool exito, string mensaje)> CrearAsync(ProductoFormDTO dto);
        Task<(bool exito, string mensaje)> EditarAsync(ProductoFormDTO dto);
        Task<(bool exito, string mensaje)> EliminarAsync(string codProd);

        Task<List<ProductoListDTO>> BuscarParaCompraAsync(string termino);
    }
}
