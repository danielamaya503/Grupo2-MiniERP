using Humanizer;
using Microsoft.EntityFrameworkCore;
using TechCore.Datos;
using TechCore.Models.DTO.Proveedor;
using TechCore.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TechCore.Services.Concretes
{
    public class ProveedorService : IProveedor
    {
        private readonly ILogger<ProveedorService> logger;
        private readonly TechCoreContext context;

        public ProveedorService(ILogger<ProveedorService> logger, TechCoreContext context)
        {
            this.logger = logger;
            this.context = context;       
        }

        public async Task<(bool exito, string mensaje)> CrearAsync(ProveedorFormDTO dto)
        {
            try
            {
                var codigo = await GenerarCodigoAsync();

                context.Proveedores.Add(new Models.Proveedore
                {
                    Codprovee = codigo,
                    Nombre = dto.Nombre.Trim(),
                    Telefono = dto.Telefono?.Trim(),
                    Email = dto.Email?.Trim(),
                    Direccion = dto.Direccion?.Trim(),
                    Estado = 1,
                    CreatedDate = DateTime.UtcNow
                });

                await context.SaveChangesAsync();

                return (true, $"Proveedor {codigo} registrado correctamente.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al crear proveedor");
                return (false, "Ocurrió un error al guardar el proveedor.");
            }
        }

        public async Task<(bool exito, string mensaje)> DesactivarAsync(string codprovee)
        {
            try
            {
                var proveedor = await context.Proveedores.FirstOrDefaultAsync(x => x.Codprovee == codprovee && x.Estado == 1);

                if (proveedor is null)
                {
                    return (false, $"No se encontró el proveedor {codprovee}.");
                }

                proveedor.Estado = 0;
              

                await context.SaveChangesAsync();

                return (true, $"Proveedor {codprovee} desactivado correctamente.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al desactivar proveedor {Cod}", codprovee);
                return (false, "Ocurrió un error al actualizar el proveedor.");
            }
        }

        public async Task<(bool exito, string mensaje)> EditarAsync(ProveedorFormDTO dto)
        {
            try
            {
                var proveedor = await context.Proveedores.FirstOrDefaultAsync(x => x.Codprovee == dto.Codprovee && x.Estado == 1);

                if (proveedor is null)
                { 
                    return (false, $"No se encontró el proveedor {dto.Codprovee}.");
                }

                proveedor.Nombre = dto.Nombre.Trim();
                proveedor.Telefono = dto.Telefono?.Trim();
                proveedor.Email = dto.Email?.Trim();
                proveedor.Direccion = dto.Direccion?.Trim();

                await context.SaveChangesAsync();

                return (true, $"Proveedor {dto.Codprovee} actualizado correctamente.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al editar proveedor {Cod}", dto.Codprovee);
                return (false, "Ocurrió un error al actualizar el proveedor.");
            }
        }

        public async Task<string> GenerarCodigoAsync()
        {
            var codigo = await context.Proveedores
                .Where(p => p.Codprovee.StartsWith("PROV-"))
                .Select(p => p.Codprovee)
                .ToListAsync();

            int maxNumero = 0;

            foreach (var cod in codigo)
            {
                var parte = cod[5..];
                if (int.TryParse(parte, out int n) && n > maxNumero)
                    maxNumero = n;
            }

            return $"PROV-{(maxNumero + 1):D3}";
        }

        public async Task<ProveedorPaginadoDTO> ObtenerPaginadoAsync(string? busqueda, int pagina, int tamanoPagina = 10)
        {
            try 
            {
                var query = context.Proveedores
                    .Where(x => x.Estado == 1)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(busqueda))
                {
                    var b = busqueda.Trim().ToLower();
                    query = query.Where(p =>
                        p.Codprovee.ToLower().Contains(b) ||
                        p.Nombre.ToLower().Contains(b));
                }

                var total = query.Count();

                var lista = await query
                    .OrderBy(p => p.Codprovee)
                    .Skip((pagina - 1) * tamanoPagina)
                    .Take(tamanoPagina)
                    .Select(p => new ProveedorListDTO
                    {
                        Codprovee = p.Codprovee,
                        Nombre = p.Nombre,
                        Telefono = p.Telefono,
                        Email = p.Email,
                        Direccion = p.Direccion,
                        Estado = p.Estado,
                        CreatedDate = p.CreatedDate
                    })
                    .ToListAsync();

                return new ProveedorPaginadoDTO
                {
                    Proveedores = lista,
                    TotalRegistros = total,
                    PaginaActual = pagina,
                    TamanoPagina = tamanoPagina,
                    Busqueda = busqueda ?? string.Empty
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener proveedores paginados");
                return new ProveedorPaginadoDTO { PaginaActual = pagina };
            }
        }

        public async Task<ProveedorFormDTO?> ObtenerPorCodigoAsync(string codprovee)
        {
            var existeProveedor = await context.Proveedores.FirstOrDefaultAsync(x => x.Codprovee == codprovee && x.Estado == 1);

            if (existeProveedor is null)
            {
                return null;
            }

            return new ProveedorFormDTO
            {
                Codprovee = existeProveedor.Codprovee,
                Nombre = existeProveedor.Nombre,
                Telefono = existeProveedor.Telefono,
                Email = existeProveedor.Email,
                Direccion = existeProveedor.Direccion
            };

        }

  
    }
}
