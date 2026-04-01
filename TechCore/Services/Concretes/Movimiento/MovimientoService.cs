using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using TechCore.Datos;
using TechCore.Models.DTO.Movimiento;
using TechCore.Services.Interfaces.Movimiento;

namespace TechCore.Services.Concretes.Movimiento
{
    public class MovimientoService : IMovimiento
    {
        private readonly TechCoreContext _context;
        private readonly ILogger<MovimientoService> _logger;

        public MovimientoService(TechCoreContext context, ILogger<MovimientoService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<MovimientoPaginadoDTO> ObtenerHistorialAsync(MovimientoFiltroDTO filtro)
        {
            try
            {
                var entradas = await ObtenerEntradasAsync(filtro);
                var salidas = await ObtenerSalidasAsync(filtro);

                // Resumen siempre sobre el período completo (sin filtro de tipo)
                var totalEntradas = entradas.Count;
                var totalSalidas = salidas.Count;
                var montoEntradas = entradas.Sum(e => e.Subtotal);
                var montoSalidas = salidas.Sum(s => s.Subtotal);

                // Unir y ordenar
                IEnumerable<MovimientoDTO> todos = entradas.Concat(salidas)
                    .OrderByDescending(m => m.Fecha);

                // Aplicar filtro de tipo si viene seleccionado
                if (!string.IsNullOrEmpty(filtro.Tipo))
                    todos = todos.Where(m => m.Tipo == filtro.Tipo);

                var lista = todos.ToList();
                var totalRegistros = lista.Count;

                // Paginación segura
                var pagina = filtro.Pagina < 1 ? 1 : filtro.Pagina;
                var totalPaginas = totalRegistros == 0
                    ? 1
                    : (int)Math.Ceiling((double)totalRegistros / filtro.TamanoPagina);

                if (pagina > totalPaginas) pagina = totalPaginas;

                var movPaginados = lista
                    .Skip((pagina - 1) * filtro.TamanoPagina)
                    .Take(filtro.TamanoPagina)
                    .ToList();

                return new MovimientoPaginadoDTO
                {
                    Movimientos = movPaginados,
                    TotalRegistros = totalRegistros,
                    PaginaActual = pagina,
                    TamanoPagina = filtro.TamanoPagina,
                    Mes = filtro.Mes,
                    Anio = filtro.Anio,
                    FiltroTipo = filtro.Tipo ?? "",
                    TotalEntradas = totalEntradas,
                    TotalSalidas = totalSalidas,
                    MontoEntradas = montoEntradas,
                    MontoSalidas = montoSalidas
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error al obtener historial mes:{Mes} año:{Anio}", filtro.Mes, filtro.Anio);
                return new MovimientoPaginadoDTO { Mes = filtro.Mes, Anio = filtro.Anio };
            }
        }

        public async Task<List<MovimientoDTO>> ObtenerTodosAsync(MovimientoFiltroDTO filtro)
        {
            try
            {
                var entradas = await ObtenerEntradasAsync(filtro);
                var salidas = await ObtenerSalidasAsync(filtro);

                IEnumerable<MovimientoDTO> todos = entradas.Concat(salidas)
                    .OrderByDescending(m => m.Fecha);

                if (!string.IsNullOrEmpty(filtro.Tipo))
                    todos = todos.Where(m => m.Tipo == filtro.Tipo);

                return todos.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los movimientos");
                return new List<MovimientoDTO>();
            }
        }

        public async Task<List<int>> ObtenerAniosDisponiblesAsync()
        {
            try
            {
                var aniosC = await _context.Compras
                    .Where(c => c.Fecha.HasValue)
                    .Select(c => c.Fecha!.Value.Year)
                    .Distinct()
                    .ToListAsync();

                var aniosV = await _context.Ventas
                    .Where(v => v.Fecha.HasValue)
                    .Select(v => v.Fecha!.Value.Year)
                    .Distinct()
                    .ToListAsync();

                return aniosC.Concat(aniosV)
                    .Distinct()
                    .OrderByDescending(a => a)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener años disponibles");
                return new List<int> { DateTime.Now.Year };
            }
        }

        private async Task<List<MovimientoDTO>> ObtenerEntradasAsync(MovimientoFiltroDTO filtro)
        {
            try
            {
                var query = _context.ComprasDetalles
                    .Include(d => d.NordenNavigation)
                        .ThenInclude(c => c.CodprovNavigation)
                    .Include(d => d.NordenNavigation)
                        .ThenInclude(c => c.CodusuNavigation) // ← para UsuarioNombre
                    .Include(d => d.CodprodNavigation)
                    .Where(d => d.NordenNavigation.Estado == 1);

                // Filtro por mes y año usando FechaDesde/FechaHasta del DTO
                if (filtro.FechaDesde.HasValue)
                    query = query.Where(d => d.NordenNavigation.Fecha >= filtro.FechaDesde);

                if (filtro.FechaHasta.HasValue)
                    query = query.Where(d => d.NordenNavigation.Fecha <= filtro.FechaHasta);

                if (!string.IsNullOrEmpty(filtro.CodProd))
                    query = query.Where(d => d.Codprod == filtro.CodProd);

                return await query.Select(d => new MovimientoDTO
                {
                    Tipo = "Entrada",
                    Referencia = d.Norden,
                    CodProd = d.Codprod,
                    Descripcion = d.CodprodNavigation.Descripcion ?? d.Codprod,
                    Cantidad = d.Cantidad,
                    Precio = d.Precio,
                    Subtotal = d.Subtotal,
                    Fecha = d.NordenNavigation.Fecha ?? DateTime.Now,
                    Origen = d.NordenNavigation.CodprovNavigation != null
                                    ? d.NordenNavigation.CodprovNavigation.Nombre
                                    : "Sin proveedor",
                    // ← AGREGADO
                    UsuarioNombre = d.NordenNavigation.CodusuNavigation != null
                                    ? d.NordenNavigation.CodusuNavigation.Nombre
                                    : "Sistema"
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener entradas");
                return new List<MovimientoDTO>();
            }
        }

        private async Task<List<MovimientoDTO>> ObtenerSalidasAsync(MovimientoFiltroDTO filtro)
        {
            try
            {
                var query = _context.VentasDetalles
                    .Include(d => d.NordenNavigation)
                        .ThenInclude(v => v.CodclienNavigation)
                    .Include(d => d.NordenNavigation)
                        .ThenInclude(v => v.CodvendNavigation) // ← para UsuarioNombre
                    .Include(d => d.CodprodNavigation)
                    .Where(d => d.NordenNavigation.Nula == false);

                // Filtro por mes y año
                if (filtro.FechaDesde.HasValue)
                    query = query.Where(d => d.NordenNavigation.Fecha >= filtro.FechaDesde);

                if (filtro.FechaHasta.HasValue)
                    query = query.Where(d => d.NordenNavigation.Fecha <= filtro.FechaHasta);

                if (!string.IsNullOrEmpty(filtro.CodProd))
                    query = query.Where(d => d.Codprod == filtro.CodProd);

                return await query.Select(d => new MovimientoDTO
                {
                    Tipo = "Salida",
                    Referencia = d.Norden,
                    CodProd = d.Codprod,
                    Descripcion = d.CodprodNavigation.Descripcion ?? d.Codprod,
                    Cantidad = d.Cantidad,
                    Precio = d.Pventa,
                    Subtotal = d.Subtotal,
                    Fecha = d.NordenNavigation.Fecha ?? DateTime.Now,
                    Origen = d.NordenNavigation.CodclienNavigation != null
                                    ? d.NordenNavigation.CodclienNavigation.Nombre
                                    : "Consumidor final",
                    // ← AGREGADO
                    UsuarioNombre = d.NordenNavigation.CodclienNavigation != null
                                    ? d.NordenNavigation.CodclienNavigation.Nombre
                                    : "Sistema"
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener salidas");
                return new List<MovimientoDTO>();
            }
        }
    }


}

