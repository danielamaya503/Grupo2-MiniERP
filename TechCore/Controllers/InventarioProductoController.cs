using Microsoft.AspNetCore.Mvc;
using TechCore.Services.Interfaces.Producto;

namespace TechCore.Controllers
{
    public class InventarioController : Controller
    {
        private readonly IProducto producto;
        private readonly ILogger<InventarioController> logger;

        public InventarioController(IProducto producto, ILogger<InventarioController> logger)
        {
            this.producto = producto;
            this.logger = logger;
        }

        public async Task<IActionResult> Index(string? buscar)
        {
            ViewData["Title"] = "Inventario";
            ViewData["Active"] = "Inventario";
            ViewData["Buscar"] = buscar;

            var lista = string.IsNullOrWhiteSpace(buscar)
                ? await producto.ObtenerTodosAsync()
                : await producto.BuscarAsync(buscar);

            return View("~/Views/Producto/Index.cshtml", lista);
        }
    }
}
