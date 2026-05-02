using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechCore.Services.Interfaces.Dashboard;

namespace TechCore.Controllers
{
    [Authorize(Roles = "Bodega, Administrador")]
    public class BodegaController : Controller
    {
        private readonly IBodegaDashboard dashboard;
        private readonly ILogger<BodegaController> logger;

        public BodegaController(IBodegaDashboard dashboard, ILogger<BodegaController> logger)
        {
            this.dashboard = dashboard;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Dashboard Bodega";
            ViewData["Active"] = "Dashboard";

            var dto = await dashboard.ObtenerDashboardAsync();
            return View(dto);
        }
    }
}
