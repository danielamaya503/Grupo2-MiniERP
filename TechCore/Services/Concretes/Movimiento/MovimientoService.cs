using Microsoft.EntityFrameworkCore;
using TechCore.Datos;
using TechCore.Models.DTO.Movimiento;

namespace TechCore.Services.Concretes.Movimiento
{
    public class MovimientoService
    {
        private readonly TechCoreContext _context;
        private readonly ILogger<MovimientoService> _logger;

        public MovimientoService(TechCoreContext context, ILogger<MovimientoService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<MovimientoDTO>> ObtenerTodosAsync(MovimientoFiltroDTO filtro)
        {
            try
            {
                var entradas = await ObtenerEntradasAsync(filtro);
                var salidas = await ObtenerSalidasAsync(filtro);

                var todos = entradas.Concat(salidas)
                    .OrderByDescending(m => m.Fecha)
                    .ToList();

                if (!string.IsNullOrEmpty(filtro.Tipo))
                    todos = todos.Where(m => m.Tipo == filtro.Tipo).ToList();

                return todos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener movimientos");
                return new List<MovimientoDTO>();
            }
        }

        private async Task<List<MovimientoDTO>> ObtenerEntradasAsync(MovimientoFiltroDTO filtro)
        {
            try
            {
                var query = _context.ComprasDetalles
                    .Include(d => d.NordenNavigation)
                        .ThenInclude(c => c.CodprovNavigation)
                    .Include(d => d.CodprodNavigation)
                    .Where(d => d.NordenNavigation.Estado == 1);

                if (!string.IsNullOrEmpty(filtro.CodProd))
                    query = query.Where(d => d.Codprod == filtro.CodProd);

                if (filtro.FechaDesde.HasValue)
                    query = query.Where(d => d.NordenNavigation.Fecha >= filtro.FechaDesde);

                if (filtro.FechaHasta.HasValue)
                    query = query.Where(d => d.NordenNavigation.Fecha <= filtro.FechaHasta);

                return await query.Select(d => new MovimientoDTO
                {
                    Referencia = d.Norden,
                    Tipo = "Entrada",
                    CodProd = d.Codprod,
                    Descripcion = d.CodprodNavigation.Descripcion ?? d.Codprod,
                    Cantidad = d.Cantidad,
                    Precio = d.Precio,
                    Subtotal = d.Subtotal,
                    Fecha = d.NordenNavigation.Fecha ?? DateTime.UtcNow,
                    Origen = d.NordenNavigation.CodprovNavigation.Nombre
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener entradas de movimientos");
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
                    .Include(d => d.CodprodNavigation)
                    .Where(d => d.NordenNavigation.Nula == false);

                if (!string.IsNullOrEmpty(filtro.CodProd))
                    query = query.Where(d => d.Codprod == filtro.CodProd);

                if (filtro.FechaDesde.HasValue)
                    query = query.Where(d => d.NordenNavigation.Fecha >= filtro.FechaDesde);

                if (filtro.FechaHasta.HasValue)
                    query = query.Where(d => d.NordenNavigation.Fecha <= filtro.FechaHasta);

                return await query.Select(d => new MovimientoDTO
                {
                    Referencia = d.Norden,
                    Tipo = "Salida",
                    CodProd = d.Codprod,
                    Descripcion = d.CodprodNavigation.Descripcion ?? d.Codprod,
                    Cantidad = d.Cantidad,
                    Precio = d.Pventa,
                    Subtotal = d.Subtotal,
                    Fecha = d.NordenNavigation.Fecha ?? DateTime.UtcNow,
                    Origen = d.NordenNavigation.CodclienNavigation.Nombre
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener salidas de movimientos");
                return new List<MovimientoDTO>();
            }
        }

    }
}
