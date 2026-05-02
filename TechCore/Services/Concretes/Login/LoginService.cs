using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechCore.Datos;
using TechCore.Models.DTO.Autentificacion;
using TechCore.Services.Interfaces.Login;

namespace TechCore.Services.Concretes.Login
{
    public class LoginService : ILogin
    {
        private readonly TechCoreContext context;
        private readonly ILogger<LoginService> logger;

        public LoginService(TechCoreContext context, ILogger<LoginService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<ClaimsPrincipal?> LoginAsync(LoginDTO request)
        {
            try 
            {
                var existeUsuario = await context.Users
                    .Include(u => u.IdrolNavigation)
                    .FirstOrDefaultAsync(u => u.Username == request.NombreUsuario);

                if (existeUsuario is null)
                {
                    logger.LogWarning("Intento de login fallido para el usuario {Username}", request.NombreUsuario);
                    return null;
                }

                bool passValida = BCrypt.Net.BCrypt.Verify(request.Contrasenia, existeUsuario.password);

                if (!passValida)
                {
                    logger.LogWarning("Contraseña incorrecta para el usuario {Username}", request.NombreUsuario);
                    return null;
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, existeUsuario.IdUser.ToString()),
                    new Claim(ClaimTypes.Name, existeUsuario.Nombre),
                    new Claim("UserName", existeUsuario.Username),
                    new Claim(ClaimTypes.Email, existeUsuario.Email ?? ""),
                    new Claim(ClaimTypes.Role, existeUsuario.IdrolNavigation.NombreRol)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                return new ClaimsPrincipal(identity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al intentar autenticar al usuario {NombreUsuario}", request.NombreUsuario);
                return null;
            }
        }

        
    }
}
