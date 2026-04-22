using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechCore.Datos;
using TechCore.Models;
using TechCore.Models.DTO.Producto;
using TechCore.Services.Interfaces.Producto;
using TechCore.Services.Interfaces.Usuario;

namespace TechCore.Controllers
{
    [Authorize(Roles = "Bodega, Administrador")]
    public class ProductoController : Controller
    {
        private readonly IProducto producto;
        private readonly TechCoreContext context;
        private readonly ILogger<ProductoController> logger;

        public ProductoController(IProducto producto, TechCoreContext context, ILogger<ProductoController> logger)
        {
            this.producto = producto;
            this.context = context;
            this.logger = logger;
        }

        public async Task<IActionResult> Index(string? buscar)
        {
            ViewData["Title"] = "Gestión de Productos";
            ViewData["Active"] = "Productos";
            ViewData["Buscar"] = buscar;

            var lista = string.IsNullOrWhiteSpace(buscar)
                ? await producto.ObtenerTodosAsync()
                : await producto.BuscarAsync(buscar);

            ViewBag.NuevoCodigo = await producto.GenerarCodigoAsync();

            await CargarCategoriasAsync();

            return View(lista);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(ProductoFormDTO dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = ObtenerPrimerError();
                return RedirectToAction(nameof(Index));
            }

            var (exito, mensaje) = await producto.CrearAsync(dto);

            if (exito) TempData["Exito"] = mensaje;
            else TempData["Error"] = mensaje;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(ProductoFormDTO dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = ObtenerPrimerError();
                return RedirectToAction(nameof(Index));
            }

            var (exito, mensaje) = await producto.EditarAsync(dto);

            if (exito) TempData["Exito"] = mensaje;
            else TempData["Error"] = mensaje;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(string codProd)
        {
            var (exito, mensaje) = await producto.EliminarAsync(codProd);

            if (exito) TempData["Exito"] = mensaje;
            else TempData["Error"] = mensaje;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Buscar(string? termino)
        {
            var lista = await producto.BuscarParaCompraAsync(termino ?? string.Empty);

            return Json(lista.Select(p => new
            {
                p.CodProd,
                p.Descripcion,
                p.PrecioCompra,
                p.Stock,
                p.Categoria
            }));
        }

        private async Task CargarCategoriasAsync(int? idSeleccionado = null)
        {
            var categorias = await context.Categoria
              .Where(c => c.Estado == true)
              .OrderBy(c => c.Nombre)
              .ToListAsync();

            ViewBag.Categorias = new SelectList(categorias, "CodCategoria", "Nombre", idSeleccionado);
        }

        private string ObtenerPrimerError()
            => ModelState.Values
                .SelectMany(v => v.Errors)
                .FirstOrDefault()?.ErrorMessage ?? "Datos inválidos.";
    }
}
