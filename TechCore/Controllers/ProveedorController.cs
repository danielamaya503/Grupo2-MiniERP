using Humanizer;
using Microsoft.AspNetCore.Mvc;
using TechCore.Models.DTO.Proveedor;
using TechCore.Services.Interfaces;

namespace TechCore.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly IProveedor proveedor;
        private readonly ILogger<ProveedorController> logger;

        public ProveedorController(IProveedor proveedor, ILogger<ProveedorController> logger)
        {
            this.proveedor = proveedor;
            this.logger = logger;
        }

        public async Task<IActionResult> Index(string? busqueda, int pagina = 1)
        {
            ViewData["Title"] = "Proveedores";
            ViewData["Active"] = "Proveedores";
            ViewData["Buscar"] = busqueda;

            if (pagina < 1) 
                pagina = 1;

            var resultado = await proveedor.ObtenerPaginadoAsync(busqueda, pagina);

            if (pagina > resultado.TotalPaginas && resultado.TotalPaginas > 0)
                return RedirectToAction(nameof(Index),
                    new { busqueda, pagina = resultado.TotalPaginas });

            ViewBag.NuevoCodigo = await proveedor.GenerarCodigoAsync();

            return View(resultado);
        }

 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(ProveedorFormDTO dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Corrige los errores del formulario.";
                return RedirectToAction(nameof(Index));
            }

            var (ok, mensaje) = await proveedor.CrearAsync(dto);
            TempData[ok ? "Exito" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(ProveedorFormDTO dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Corrige los errores del formulario.";
                return RedirectToAction(nameof(Index));
            }

            var (ok, mensaje) = await proveedor.EditarAsync(dto);
            TempData[ok ? "Exito" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Desactivar(string codprovee)
        {
            var (ok, mensaje) = await proveedor.DesactivarAsync(codprovee);
            TempData[ok ? "Exito" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }


    }
}
