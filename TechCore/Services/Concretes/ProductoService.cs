using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechCore.Datos;
using TechCore.Models.DTO.Compra;
using TechCore.Models.DTO.Producto;
using TechCore.Services.Interfaces;

namespace TechCore.Services.Concretes
{
    public class ProductoService : IProducto
    {
        private readonly TechCoreContext context;
        private readonly ILogger<ProductoService> logger;

        public ProductoService(TechCoreContext context, ILogger<ProductoService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<List<ProductoListDTO>> BuscarAsync(string termino)
        {

            try
            {
                var t = termino.Trim().ToLower();

                return await context.Productos
                    .Include(p => p.CodCategoriaNavigation)
                    .Where(p => p.Estado == true && (
                        p.Codprod.ToLower().Contains(t) ||
                        (p.Descripcion != null && p.Descripcion.ToLower().Contains(t)) ||
                        (p.CodCategoriaNavigation != null &&
                         p.CodCategoriaNavigation.Nombre.ToLower().Contains(t))
                    ))
                    .OrderBy(p => p.Descripcion)
                    .Select(p => new ProductoListDTO
                    {
                        CodProd = p.Codprod,
                        Descripcion = p.Descripcion ?? p.Codprod,
                        CodCategoria = p.CodCategoria,
                        Categoria = p.CodCategoriaNavigation != null
                                        ? p.CodCategoriaNavigation.Nombre
                                        : "Sin categoría",
                        PrecioCompra = p.PrecioCompra,
                        PrecioVenta = p.PrecioVenta,
                        Stock = p.Stock ?? 0,
                        StockMinimo = p.StockMinimo ?? 5,
                        Estado = p.Estado,
                        CreatedDate = p.CreatedDate ?? DateTime.UtcNow
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al buscar productos con término {Termino}", termino);
                return new List<ProductoListDTO>();
            }
        }

        public async Task<List<ProductoListDTO>> BuscarParaCompraAsync(string termino)
        {
            try
            {
                var t = termino.Trim().ToLower();

                return await context.Productos
                    .Where(p => p.Estado == true && (
                        p.Codprod.ToLower().Contains(t) ||
                        (p.Descripcion != null && p.Descripcion.ToLower().Contains(t))
                    ))
                    .OrderBy(p => p.Descripcion)
                    .Select(p => new ProductoListDTO
                    {
                        CodProd = p.Codprod,
                        Descripcion = p.Descripcion ?? p.Codprod,
                        CodCategoria = p.CodCategoria,
                        Categoria = p.CodCategoriaNavigation != null
                                        ? p.CodCategoriaNavigation.Nombre
                                        : "Sin categoría",
                        PrecioCompra = p.PrecioCompra,
                        PrecioVenta = p.PrecioVenta,
                        Stock = p.Stock ?? 0,
                        StockMinimo = p.StockMinimo ?? 5,
                        Estado = p.Estado,
                        CreatedDate = p.CreatedDate ?? DateTime.UtcNow
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al buscar productos para compra");
                return new List<ProductoListDTO>();
            }
        }

        public async Task<(bool exito, string mensaje)> CrearAsync(ProductoFormDTO dto)
        {
            try
            {
                var codigo = await GenerarCodigoAsync();

                var nuevoProducto = new Models.Producto
                {
                    Codprod = codigo,
                    CodCategoria = dto.CodCategoria,
                    Descripcion = dto.Descripcion?.Trim(),
                    PrecioCompra = dto.PrecioCompra,
                    PrecioVenta = dto.PrecioVenta,
                    Stock = dto.Stock,
                    StockMinimo = dto.StockMinimo,
                    Estado = true,
                    CreatedDate = DateTime.UtcNow
                };

                context.Productos.Add(nuevoProducto);
                await context.SaveChangesAsync();
                return (true, $"Producto '{dto.Descripcion}' creado correctamente.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al crear producto {CodProd}", dto.CodProd);
                return (false, "Ocurrió un error al crear el producto.");
            }
        }

        public async Task<string> GenerarCodigoAsync() 
        {

            var codigos = await context.Productos
              .Where(c => c.Codprod.StartsWith("PROD-"))
              .Select(c => c.Codprod)
              .ToListAsync();

            int maxNum = 0;

            foreach (var cod in codigos)
            {
                var parte = cod[5..];
                if (int.TryParse(parte, out int n) && n > maxNum)
                    maxNum = n;
            }

            return $"PROD-{(maxNum + 1):D3}";
        }

        public async Task<(bool exito, string mensaje)> EditarAsync(ProductoFormDTO dto)
        {
            try
            {
                var producto = await context.Productos
                    .FirstOrDefaultAsync(p => p.Codprod == dto.CodProdOriginal && p.Estado == true);

                if (producto == null)
                    return (false, "Producto no encontrado.");

                producto.CodCategoria = dto.CodCategoria;
                producto.Descripcion = dto.Descripcion?.Trim();
                producto.PrecioCompra = dto.PrecioCompra;
                producto.PrecioVenta = dto.PrecioVenta;
                producto.StockMinimo = dto.StockMinimo;

                await context.SaveChangesAsync();
                return (true, $"Producto '{producto.Codprod}' actualizado correctamente.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al editar producto {CodProd}", dto.CodProdOriginal);
                return (false, "Ocurrió un error al actualizar el producto.");
            }
        }

        public async Task<(bool exito, string mensaje)> EliminarAsync(string codProd)
        {
            try
            {
                var producto = await context.Productos
                    .FirstOrDefaultAsync(p => p.Codprod == codProd && p.Estado == true);

                if (producto == null)
                    return (false, "Producto no encontrado.");

                var tieneVentas = await context.VentasDetalles
                    .AnyAsync(v => v.Codprod == codProd);

                var tieneCompras = await context.ComprasDetalles
                    .AnyAsync(c => c.Codprod == codProd);

                if (tieneVentas || tieneCompras)
                    return (false, "No puedes eliminar un producto con movimientos registrados.");

                producto.Estado = false;
                await context.SaveChangesAsync();

                return (true, $"Producto '{codProd}' eliminado correctamente.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al eliminar producto {CodProd}", codProd);
                return (false, "Ocurrió un error al eliminar el producto.");
            }
        }

        public  async Task<ProductoFormDTO?> ObtenerParaEditarAsync(string codProd)
        {
            try
            {
                var p = await context.Productos
                    .FirstOrDefaultAsync(p => p.Codprod == codProd && p.Estado == true);

                if (p == null) return null;

                return new ProductoFormDTO
                {
                    CodProdOriginal = p.Codprod,
                    CodProd = p.Codprod,
                    CodCategoria = p.CodCategoria,
                    Descripcion = p.Descripcion,
                    PrecioCompra = p.PrecioCompra,
                    PrecioVenta = p.PrecioVenta,
                    Stock = p.Stock ?? 0,
                    StockMinimo = p.StockMinimo ?? 5,
                    EsEdicion = true
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener producto {CodProd} para editar", codProd);
                return null;
            }
        }

        public async Task<List<ProductoListDTO>> ObtenerStockBajoAsync()
        {
            try
            {
                return await context.Productos
                    .Include(p => p.CodCategoriaNavigation)
                    .Where(p => p.Estado == true && p.Stock <= p.StockMinimo)
                    .OrderBy(p => p.Stock)
                    .Select(p => new ProductoListDTO
                    {
                        CodProd = p.Codprod,
                        Descripcion = p.Descripcion ?? p.Codprod,
                        CodCategoria = p.CodCategoria,
                        Categoria = p.CodCategoriaNavigation != null
                                        ? p.CodCategoriaNavigation.Nombre
                                        : "Sin categoría",
                        PrecioCompra = p.PrecioCompra,
                        PrecioVenta = p.PrecioVenta,
                        Stock = p.Stock ?? 0,
                        StockMinimo = p.StockMinimo ?? 5,
                        Estado = p.Estado,
                        CreatedDate = p.CreatedDate ?? DateTime.UtcNow
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener productos con stock bajo");
                return new List<ProductoListDTO>();
            }
        }

        public async Task<List<ProductoListDTO>> ObtenerTodosAsync()
        {
            try
            {
                return await context.Productos
                    .Include(p => p.CodCategoriaNavigation)
                    .Where(p => p.Estado == true)
                    .OrderBy(p => p.Descripcion)
                    .Select(p => new ProductoListDTO
                    {
                        CodProd = p.Codprod,
                        Descripcion = p.Descripcion ?? p.Codprod,
                        CodCategoria = p.CodCategoria,
                        Categoria = p.CodCategoriaNavigation != null
                                        ? p.CodCategoriaNavigation.Nombre
                                        : "Sin categoría",
                        PrecioCompra = p.PrecioCompra,
                        PrecioVenta = p.PrecioVenta,
                        Stock = p.Stock ?? 0,
                        StockMinimo = p.StockMinimo ?? 5,
                        Estado = p.Estado,
                        CreatedDate = p.CreatedDate ?? DateTime.UtcNow
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener productos");
                return new List<ProductoListDTO>();
            }
        }


    }
}
