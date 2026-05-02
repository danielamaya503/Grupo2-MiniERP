using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechCore.Models.DTO.Cliente;
using TechCore.Services.Interfaces;

namespace TechCore.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ICliente cliente;
        private readonly ILogger<ClienteController> logger;

        public ClienteController(ICliente cliente, ILogger<ClienteController> logger)
        {
            this.cliente = cliente;
            this.logger = logger;
        }

        private int GetIdCreador()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(claim, out int id) ? id : 0;
        }

        private bool EsAdmin()
        {
            return User.IsInRole("Administrador");
        }

        public async Task<IActionResult> Index(string? busqueda, int pagina = 1)
        {
            ViewData["Title"] = "Clientes";
            ViewData["Active"] = "Clientes";
            ViewData["Buscar"] = busqueda;
            if (pagina < 1) pagina = 1;

            int? filtroCreador = EsAdmin() ? null : GetIdCreador();

            var resultado = await cliente.ObtenerPaginadoAsync(
                busqueda, pagina, filtroCreador);

            if (pagina > resultado.TotalPaginas && resultado.TotalPaginas > 0)
                return RedirectToAction(nameof(Index),
                    new { busqueda, pagina = resultado.TotalPaginas });

            ViewBag.NuevoCodigo = await cliente.GenerarCodigoAsync();
            ViewBag.EsAdmin = EsAdmin();

            return View(resultado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(ClienteFormDTO dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Corrige los errores del formulario.";
                return RedirectToAction(nameof(Index));
            }

            dto.IdCreador = GetIdCreador();

            var (ok, mensaje) = await cliente.CrearAsync(dto);
            TempData[ok ? "Exito" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(ClienteFormDTO dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Corrige los errores del formulario.";
                return RedirectToAction(nameof(Index));
            }

            int? filtroCreador = EsAdmin() ? null : GetIdCreador();

            var (ok, mensaje) = await cliente.EditarAsync(dto, filtroCreador);
            TempData[ok ? "Exito" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Desactivar(string codclien)
        {
            int? filtroCreador = EsAdmin() ? null : GetIdCreador();

            var (ok, mensaje) = await cliente.DesactivarAsync(codclien, filtroCreador);
            TempData[ok ? "Exito" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }
    }
}
