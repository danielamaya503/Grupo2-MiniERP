using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using TechCore.Models.DTO.Autentificacion;
using TechCore.Services.Concretes.Login;
using TechCore.Services.Interfaces.Login;

namespace TechCore.Controllers
{
    public class AccountController : Controller
    {
      

        private readonly ILogin login;
        private readonly ILogger<LoginService> logger;

        public AccountController(ILogin login, ILogger<LoginService> logger)
        {
            this.login = login;
            this.logger = logger;
        }

        //GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View();

        }

        ///Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken] //hace que el token de validación antifalsificación sea obligatorio para esta acción
        public async Task<IActionResult> Login(LoginDTO request)
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning("Datos de login no válidos para el usuario {NombreUsuario}", request.NombreUsuario);
                return View(request);
            }

            try
            {
                var usuario = await login.LoginAsync(request);

                if (usuario is null)
                {
                    logger.LogWarning("Intento de login fallido para el usuario {NombreUsuario}", request.NombreUsuario);
                    ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");
                    return View(request);
                }

                // Firma la cookie con las propiedades de expiración
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, usuario, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = request.Recordatorio ? DateTime.UtcNow.AddMinutes(5) : DateTimeOffset.UtcNow.AddMinutes(3)
                });

                logger.LogInformation("Usuario {NombreUsuario} ha iniciado sesión exitosamente", request.NombreUsuario);

                return RedirectToAction("Index", "Home");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en el proceso de login para el usuario {NombreUsuario}", request.NombreUsuario);
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado. Por favor, inténtalo de nuevo más tarde.");
                return View(request);
            }
        }

        //POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var user = User.FindFirst("Username")?.Value;

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                logger.LogInformation("Usuario {NombreUsuario} ha cerrado sesión exitos", user);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al intentar cerrar sesión para el usuario {NombreUsuario}", User.Identity?.Name);
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }



        public IActionResult GenerarHash()
        {
            string hash = BCrypt.Net.BCrypt.HashPassword("admin123");
            return Content(hash);
        }
    }
}
