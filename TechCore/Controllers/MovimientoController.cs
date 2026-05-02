using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechCore.Models.DTO.Movimiento;
using TechCore.Services.Interfaces;

namespace TechCore.Controllers
{
    public class MovimientoController : Controller
    {
        private readonly IMovimiento movimiento;
        private readonly ILogger<MovimientoController> logger;

        public MovimientoController(IMovimiento movimiento, ILogger<MovimientoController> logger)
        {
            this.movimiento = movimiento;
            this.logger = logger;
        }

        // GET: MovimientoController
        public async Task<IActionResult> Index(
            int? mes = null,
            int? anio = null,
            string? tipo = null,
            int pagina = 1
            )
        {
            var filtro = new MovimientoFiltroDTO
            {
                Mes = mes ?? DateTime.Now.Month,
                Anio = anio ?? DateTime.Now.Year,
                Tipo = tipo ?? "",
                Pagina = pagina < 1 ? 1 : pagina,
                TamanoPagina = 25
            };

            var resultado = await movimiento.ObtenerHistorialAsync(filtro);

            // Redirigir si la página supera el total
            if (filtro.Pagina > resultado.TotalPaginas && resultado.TotalPaginas > 0)
                return RedirectToAction(nameof(Index), new
                {
                    mes = filtro.Mes,
                    anio = filtro.Anio,
                    tipo = filtro.Tipo,
                    pagina = resultado.TotalPaginas
                });

            ViewBag.AniosDisponibles = await movimiento.ObtenerAniosDisponiblesAsync();
            return View(resultado);
        }


        public async Task<IActionResult> ExportarPDF(
            int? mes,
            int? anio,
            string tipo = "")
        {
            var filtro = new MovimientoFiltroDTO
            {
                Mes = mes ?? DateTime.Now.Month,
                Anio = anio ?? DateTime.Now.Year,
                Tipo = tipo ?? "",
                Pagina = 1,
                TamanoPagina = int.MaxValue  // Sin paginación para PDF
            };

            var resultado = await movimiento.ObtenerHistorialAsync(filtro);

            var nombreMes = new DateTime(filtro.Anio, filtro.Mes, 1)
                .ToString("MMMM yyyy", new System.Globalization.CultureInfo("es-ES"));
            ViewData["NombreMes"] = char.ToUpper(nombreMes[0]) + nombreMes[1..];

            return View(resultado);
        }

    }
}
