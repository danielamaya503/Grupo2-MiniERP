using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechCore.Datos;
using TechCore.Models.DTO.Usuario;
using TechCore.Services.Concretes.Usuario;
using TechCore.Services.Interfaces.Usuario;

namespace TechCore.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuario usuario;
        private readonly TechCoreContext context;
        private readonly ILogger<UsuarioService> logger;

        public UsuarioController(IUsuario usuario, TechCoreContext context, ILogger<UsuarioService> logger)
        {
            this.usuario = usuario;
            this.context = context;
            this.logger = logger;
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index(string? buscar)
        {
            ViewData["Title"] = "Gestión de Usuarios";
            ViewData["Active"] = "Usuarios";
            ViewData["Buscar"] = buscar;

            var usuarios = string.IsNullOrWhiteSpace(buscar)
                ? await usuario.ObtenerTodosAsync()
                : await usuario.BuscarAsync(buscar);

            ViewBag.Top10 = await usuario.ObtenerTop10RecientesAsync();
            ViewBag.NuevoCodigo = await usuario.GenerarCodigoAsync();

            await CargarRolesAsync();

            return View(usuarios);
        }

        private async Task CargarRolesAsync(int? idSeleccionado = null)
        {
            var roles = await context.Rols
                .Where(r => r.Habilitado == true)
                .OrderBy(r => r.NombreRol)
                .ToListAsync();

            ViewBag.Roles = new SelectList(roles, "IdRol", "NombreRol", idSeleccionado);
        }


        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear( UsuarioCrearDTO dto)
        {
            if (!ModelState.IsValid)
            {
                await CargarRolesAsync(dto.IdRol);
                TempData["Error"] = "Completa todos los campos requeridos.";
                return RedirectToAction(nameof(Index));
            }

            var (exito, mensaje) = await usuario.CrearAsync(dto);

            TempData[exito ? "Exito" : "Error"] = mensaje;
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(UsuarioEditAdminDTO dto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Completa todos los campos requeridos.";
                return RedirectToAction(nameof(Index));
            }

            var (exito, mensaje) = await usuario.EditarAdminAsync(dto);

            if (exito) TempData["Exito"] = mensaje;
            else TempData["Error"] = mensaje;

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restablecer(int id)
        {
            var (exito, mensaje) = await usuario.RestablecerPasswordAsync(id);

            if (exito) TempData["Exito"] = mensaje;
            else TempData["Error"] = mensaje;

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Perfil()
        {
            ViewData["Title"] = "Mi Perfil";
            ViewData["Active"] = "Perfil";

            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 

            if (!int.TryParse(idClaim, out int id))
                return Unauthorized();

            var dto = await usuario.ObtenerPerfilAsync(id);
            if (dto == null) return NotFound();

            ViewBag.CambiarPasswordDTO = new UsuarioCambiarPasswordDTO { Id = id };

            return View(dto);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarPerfil(UsuarioPerfilDTO dto)
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(idClaim, out int idAutenticado) || dto.Id != idAutenticado)
                return Unauthorized();


            if (!ModelState.IsValid)
            {
                TempData["Error"] = ObtenerPrimerError();
                return RedirectToAction(nameof(Perfil));
            }

            var (exito, mensaje) = await usuario.ActualizarPerfilAsync(dto);


            if (exito) TempData["Exito"] = mensaje;
            else TempData["Error"] = mensaje;

            return RedirectToAction(nameof(Perfil));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarPassword(UsuarioCambiarPasswordDTO dto)
        {
            if (!EsElMismoUsuario(dto.Id))
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                TempData["Error"] = ObtenerPrimerError();
                return RedirectToAction(nameof(Perfil));
            }

            var (exito, mensaje) = await usuario.CambiarPasswordAsync(dto);

            if (exito) TempData["Exito"] = mensaje;
            else TempData["Error"] = mensaje;

            return RedirectToAction(nameof(Perfil));
        }

        // Obtiene el Id del usuario autenticado desde los Claims
        private int? ObtenerIdAutenticado()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out int id) ? id : null;
        }

        // Verifica que el Id del form sea el mismo que el autenticado
        private bool EsElMismoUsuario(int idDto)
            => ObtenerIdAutenticado() == idDto;

        // Retorna el primer error del ModelState
        private string ObtenerPrimerError()
            => ModelState.Values
                .SelectMany(v => v.Errors)
                .FirstOrDefault()?.ErrorMessage ?? "Datos inválidos.";

    }
}
