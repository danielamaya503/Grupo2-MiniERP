using Microsoft.EntityFrameworkCore;
using TechCore.Datos;
using TechCore.Models.DTO.Cliente;
using TechCore.Services.Interfaces.Cliente;

namespace TechCore.Services.Concretes.Cliente
{
    public class ClienteService : ICliente
    {
        private readonly TechCoreContext context;

        private readonly ILogger<ClienteService> logger;

        public ClienteService(TechCoreContext context, ILogger<ClienteService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<(bool exito, string mensaje)> CrearAsync(ClienteFormDTO dto)
        {
            try
            {
                var codigo = await GenerarCodigoAsync();

                context.Clientes.Add(new Models.Cliente
                {
                    Codclien = codigo,
                    Nombre = dto.Nombre.Trim(),
                    Telefono = dto.Telefono?.Trim(),
                    Email = dto.Email?.Trim(),
                    Direccion = dto.Direccion?.Trim(),
                    Estado = true,
                    idCreador = dto.IdCreador,
                    CreatedDate = DateTime.UtcNow
                });

                await context.SaveChangesAsync();
                return (true, $"Cliente {codigo} registrado correctamente.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al crear cliente");
                return (false, "Ocurrió un error al guardar el cliente.");
            }
        }

        public async Task<(bool exito, string mensaje)> DesactivarAsync(string codclien, int? idCreador = null)
        {
            try
            {
                var query = context.Clientes
                    .Where(c => c.Codclien == codclien);

                if (idCreador.HasValue)
                    query = query.Where(c => c.idCreador == idCreador.Value);

                var cliente = await query.FirstOrDefaultAsync();

                if (cliente is null)
                    return (false, "Cliente no encontrado o sin permisos.");

                cliente.Estado = false;
                await context.SaveChangesAsync();
                return (true, $"Cliente {codclien} desactivado correctamente.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al desactivar cliente {Cod}", codclien);
                return (false, "Ocurrió un error al desactivar el cliente.");
            }
        }

        public async Task<(bool exito, string mensaje)> EditarAsync(ClienteFormDTO dto, int? idCreador = null)
        {
            try
            {
                var query = context.Clientes
                    .Where(c => c.Codclien == dto.Codclien);

                if (idCreador.HasValue)
                    query = query.Where(c => c.idCreador == idCreador.Value);

                var cliente = await query.FirstOrDefaultAsync();

                if (cliente is null)
                    return (false, "Cliente no encontrado o sin permisos.");

                cliente.Nombre = dto.Nombre.Trim();
                cliente.Telefono = dto.Telefono?.Trim();
                cliente.Email = dto.Email?.Trim();
                cliente.Direccion = dto.Direccion?.Trim();

                await context.SaveChangesAsync();
                return (true, $"Cliente {dto.Codclien} actualizado correctamente.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al editar cliente {Cod}", dto.Codclien);
                return (false, "Ocurrió un error al actualizar el cliente.");
            }
        }

        public async Task<string> GenerarCodigoAsync()
        {
            var codigos = await context.Clientes
               .Where(c => c.Codclien.StartsWith("CLI-"))
               .Select(c => c.Codclien)
               .ToListAsync();

            int maxNum = 0;

            foreach (var cod in codigos)
            {
                var parte = cod[4..];
                if (int.TryParse(parte, out int n) && n > maxNum)
                    maxNum = n;
            }

            return $"CLI-{(maxNum + 1):D3}";
        }

        public async Task<ClientePaginadoDTO> ObtenerPaginadoAsync(string? busqueda, int pagina, int? idCreador = null, int tamanoPagina = 10)
        {
            try
            {
                var query = context.Clientes
                    .Include(x => x.CreadorNavigation)
                    .Where(x => x.Estado)
                    .AsQueryable();

                if (idCreador.HasValue)
                {
                    query = query.Where(x => x.idCreador == idCreador.Value);
                }

                if (!string.IsNullOrWhiteSpace(busqueda))
                {
                    var buscar = busqueda.Trim().ToLower();
                    query = query.Where(x =>
                        x.Codclien.ToLower().Contains(buscar) ||
                        x.Nombre.ToLower().Contains(buscar) ||
                        x.CreadorNavigation.Nombre.ToLower().Contains(buscar));
                }

                var total = await query.CountAsync();

                var lista = await query
                    .OrderBy(c => c.Codclien)
                    .Skip((pagina - 1) * tamanoPagina)
                    .Take(tamanoPagina)
                    .Select(c => new ClienteListDTO
                    {
                        Codclien = c.Codclien,
                        Nombre = c.Nombre,
                        Telefono = c.Telefono,
                        Email = c.Email,
                        Direccion = c.Direccion,
                        Estado = c.Estado,
                        CreatedDate = c.CreatedDate,
                        IdCreador = c.idCreador,
                        NombreCreador = c.CreadorNavigation != null
                                        ? c.CreadorNavigation.Nombre
                                        : "Sistema"
                    })
                    .ToListAsync();

                return new ClientePaginadoDTO
                {
                    Clientes = lista,
                    TotalRegistros = total,
                    PaginaActual = pagina,
                    TamanoPagina = tamanoPagina,
                    Busqueda = busqueda ?? string.Empty
                };
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, "Error al paginar clientes");
                return new ClientePaginadoDTO { PaginaActual = pagina };
            }
        }

        public async Task<ClienteFormDTO?> ObtenerPorCodigoAsync(string codclien, int? idCreador = null)
        {
            var query = context.Clientes
               .Where(c => c.Codclien == codclien);

            if (idCreador.HasValue)
                query = query.Where(c => c.idCreador == idCreador.Value);

            var clientes = await query.FirstOrDefaultAsync();

            if (clientes is null) return null;

            return new ClienteFormDTO
            {
                Codclien = clientes.Codclien,
                IdCreador = clientes.idCreador,
                Nombre = clientes.Nombre,
                Telefono = clientes.Telefono,
                Email = clientes.Email,
                Direccion = clientes.Direccion
            };
        }
    }
}
