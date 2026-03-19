using Azure.Core;
using Microsoft.EntityFrameworkCore;
using TechCore.Datos;
using TechCore.Models.DTO.Dashboards;
using TechCore.Models.DTO.Usuario;
using TechCore.Services.Interfaces.Dashboard;

namespace TechCore.Services.Concretes.Dashboard
{
    public class BodegaDashboardService : IBodegaDashboard
    {
        private readonly TechCoreContext _context;
        private readonly ILogger<BodegaDashboardDTO> logger;

        public BodegaDashboardService(TechCoreContext context, ILogger<BodegaDashboardDTO> logger)
        { 
            this._context = context;
            this.logger = logger;
        }

        public async Task<BodegaDashboardDTO> ObtenerDashboardAsync()
        {
            try
            {
                var productos = await _context.Productos
                    .Where(p => p.Estado == true)
                    .ToListAsync();

                int totalProductos = productos.Count;
                decimal valorInventario = productos.Sum(p => p.PrecioCompra * (p.Stock ?? 0));

                var alertas = productos
                    .Where(p => (p.Stock ?? 0) <= (p.StockMinimo ?? 5))
                    .OrderBy(p => p.Stock)
                    .Select(p => new StockAlertaDTO
                    {
                        CodProd = p.Codprod,
                        Descripcion = p.Descripcion ?? p.Codprod,
                        Stock = p.Stock ?? 0,
                        StockMinimo = p.StockMinimo ?? 5
                    })
                    .ToList();

                var comprasRecientes = await _context.Compras
                    .Include(c => c.CodprovNavigation)
                    .Where(c => c.Estado == 1)
                    .OrderByDescending(c => c.Fecha)
                    .Take(8)
                    .Select(c => new CompraResumenDTO
                    {
                        NOrden = c.Norden,
                        OrdenN = c.OrdenN,
                        Proveedor = c.CodprovNavigation.Nombre,
                        Fecha = c.Fecha ?? DateTime.Now,
                        Total = c.Total,
                        Estado = c.Estado ?? 1
                    })
                    .ToListAsync();

                return new BodegaDashboardDTO
                {
                    TotalProductos = totalProductos,
                    ValorInventario = valorInventario,
                    ItemsStockBajo = alertas.Count,
                    AlertasStock = alertas,
                    ComprasRecientes = comprasRecientes
                };
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error al obtener dashboard de bodega");
                return new BodegaDashboardDTO();
            }
        }
    }
}
