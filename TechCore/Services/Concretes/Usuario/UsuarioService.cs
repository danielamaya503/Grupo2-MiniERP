using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Configuration;
using TechCore.Datos;
using TechCore.Models;
using TechCore.Models.DTO.Usuario;
using TechCore.Services.Interfaces.Usuario;

namespace TechCore.Services.Concretes.Usuario
{
    public class UsuarioService : IUsuario
    {
        private readonly TechCoreContext context;
        private readonly ILogger<UsuarioService> logger;
        private readonly string passwordDefault;
        private const string WebhookUrl = "https://hook.us2.make.com/1v077qzpt7v4vlg3seb4bd9ybeqnqbw3";

        public UsuarioService(TechCoreContext context, ILogger<UsuarioService> logger, IConfiguration configuration)
        {
            this.context = context;
            this.logger = logger;
            passwordDefault = configuration["PasswordDefecto"]!;
        }

        public async Task<List<UsuarioListDTO>> ObtenerTodosAsync()
        {
            try
            {
                return await context.Users.Include(x => x.IdrolNavigation)
                    .OrderBy(x => x.Nombre)
                    .Select(x => new UsuarioListDTO
                    {
                        Id = x.Idrol,
                        Code = x.Code,
                        Nombre = x.Nombre,
                        Username = x.Username,
                        Email = x.Email,
                        Phone = x.Phone,
                        IdRol = x.Idrol,
                        NombreRol = x.IdrolNavigation.NombreRol,
                        CreatedDate = x.CreatedDate ?? null,
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener todos los usuarios");
                return new List<UsuarioListDTO>();
            }
        }

        public async Task<List<UsuarioListDTO>> BuscarAsync(string busqueda)
        {
            try
            {
                var query = busqueda.ToLower();

                return await context.Users.Include(x => x.IdrolNavigation)
                    .Where(x => x.Nombre.ToLower().Contains(busqueda) || x.Username.ToLower().Contains(busqueda) || x.Email.ToLower().Contains(busqueda))
                    .OrderBy(x => x.Nombre)
                    .Select(x => new UsuarioListDTO
                    {
                        Id = x.Idrol,
                        Code = x.Code,
                        Nombre = x.Nombre,
                        Username = x.Username,
                        Email = x.Email,
                        Phone = x.Phone,
                        IdRol = x.Idrol,
                        NombreRol = x.IdrolNavigation.NombreRol,
                        CreatedDate = x.CreatedDate ?? null,
                    })
                    .ToListAsync();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error en búsqueda de usuarios con término {busqueda}", busqueda);
                return new List<UsuarioListDTO>();
            }
        }

        public async Task<List<UsuarioListDTO>> ObtenerTop10RecientesAsync()
        {
            try
            {
                return await context.Users
                    .Include(u => u.IdrolNavigation)
                    .OrderByDescending(u => u.CreatedDate)
                    .Take(10) //REALIZAR UN TOP 10
                    .Select(u => new UsuarioListDTO
                    {
                        Id = u.Idrol,
                        Code = u.Code,
                        Nombre = u.Nombre,
                        Username = u.Username,
                        Email = u.Email,
                        NombreRol = u.IdrolNavigation.NombreRol,
                        CreatedDate = u.CreatedDate ?? DateTime.UtcNow
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener top 10 usuarios recientes");
                return new List<UsuarioListDTO>();
            }
        }

        public async Task<UsuarioEditAdminDTO?> ObtenerParaEditarAdminAsync(int id)
        {
            try
            {
                var usuario = await context.Users.FirstOrDefaultAsync(x => x.IdUser == id);

                if (usuario is null)
                    return null;

                return new UsuarioEditAdminDTO
                {
                    Id = usuario.IdUser,
                    Code = usuario.Code,
                    Nombre = usuario.Nombre,
                    Username = usuario.Username,
                    Email = usuario.Email,
                    Phone = usuario.Phone,
                    IdRol = usuario.Idrol
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener usuario ID {Id} para edición admin", id);
                return null;
            }
        }

        public async Task<UsuarioPerfilDTO?> ObtenerPerfilAsync(int id)
        {
            try
            {
                var usuario = await context.Users.FirstOrDefaultAsync(x => x.IdUser == id);

                if (usuario is null)
                    return null;

                return new UsuarioPerfilDTO
                {
                    Id = usuario.IdUser,
                    Username = usuario.Username,
                    Nombre = usuario.Nombre,
                    Email = usuario.Email,
                    Phone = usuario.Phone
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener perfil del usuario ID {Id}", id);
                return null;
            }
        }

        public async Task<(bool exito, string mensaje)> CrearAsync(UsuarioCrearDTO dto)
        {
            try
            {
                bool existeUsername = await context.Users
                    .AnyAsync(x => x.Username.ToLower() == dto.Username.ToLower());

                if (existeUsername)
                    return (false, $"El usuario {dto.Username} ya existe");

                var codigo = await GenerarCodigoAsync();

                context.Add(new User
                {
                    Code = codigo,
                    Nombre = dto.Nombre,
                    Username = dto.Username,
                    password = BCrypt.Net.BCrypt.HashPassword(passwordDefault),
                    Phone = dto.Phone,
                    Idrol = dto.IdRol,
                    Email = dto.Email,
                    CreatedDate = DateTime.UtcNow
                });

                await context.SaveChangesAsync();

                logger.LogInformation("Usuario {Username} creado exitosamente", dto.Username);

                return (true, $"Usuario {dto.Username} creado creado correctamete");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al crear usuario {Username}", dto.Username);
                return (false, "Ocurrio un erorr al crear el usuario");
            }
        }

        public async Task<string> GenerarCodigoAsync()
        {
            var codigos = await context.Users
               .Where(c => c.Code.StartsWith("USR-"))
               .Select(c => c.Code)
               .ToListAsync();

            int maxNum = 0;

            foreach (var cod in codigos)
            {
                var parte = cod[4..];
                if (int.TryParse(parte, out int n) && n > maxNum)
                    maxNum = n;
            }

            return $"USR-{(maxNum + 1):D3}";
        }


        public async Task<(bool exito, string mensaje)> EditarAdminAsync(UsuarioEditAdminDTO dto)
        {
            try
            {
                var usuario = await context.Users.FirstOrDefaultAsync(x => x.IdUser == dto.Id);

                if (usuario is null)
                    return (false, "Usuario no encontrado");

                usuario.Nombre = dto.Nombre;
                usuario.Email = dto.Email;
                usuario.Phone = dto.Phone;
                usuario.Idrol = dto.IdRol;

                await context.SaveChangesAsync();
                logger.LogInformation("Usuario ID {Id} editado exitosamente", dto.Id);

                return (true, "Usuario actualizado correctamente");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al editar el usuario ID {Id}", dto.Id);
                return (false, "Ocurrio un error al editar el usuario");
            }
        }
        public async Task<(bool exito, string mensaje)> RestablecerPasswordAsync(int id)
        {
            try
            {
                var usuario = await context.Users.FindAsync(id);

                if (usuario is null) 
                    return (false, "Usuario no encontrado");

                // Restablece a la contraseña por defecto de la compañía
                usuario.password = BCrypt.Net.BCrypt.HashPassword(passwordDefault);
                await context.SaveChangesAsync();

                logger.LogInformation("Contraseña del usuario ID {Id} restablecida", id);
                return (true, $"Contraseña restablecida");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al restablecer contraseña del usuario ID {Id}", id);
                return (false, "Ocurrió un error al restablecer la contraseña");
            }
        }

        public async Task<(bool exito, string mensaje)> ActualizarPerfilAsync(UsuarioPerfilDTO dto)
        {
            try
            {
                var usuario = await context.Users.FindAsync(dto.Id);

                if (usuario is null)
                    return (false, "Usuario no encontrado");

                // Actualiza datos personales
                usuario.Nombre = dto.Nombre;
                usuario.Email = dto.Email;
                usuario.Phone = dto.Phone;

                if (!string.IsNullOrWhiteSpace(dto.NuevaPassword))
                {
                    bool actualCorrecta = BCrypt.Net.BCrypt.Verify(dto.PasswordActual, usuario.password);

                    if (!actualCorrecta)
                        return (false, "La contraseña actual es incorrecta");

                    usuario.password = BCrypt.Net.BCrypt.HashPassword(dto.NuevaPassword);

                    logger.LogInformation("Usuario ID {Id} cambió su contraseña", dto.Id);
                }

                await context.SaveChangesAsync();

                return (true, "Perfil actualizado correctamente");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al actualizar perfil del usuario ID {Id}", dto.Id);
                return (false, "Ocurrió un error al actualizar el perfil");
            }
        }

        public async Task<(bool exito, string mensaje)> CambiarPasswordAsync(UsuarioCambiarPasswordDTO dto)
        {
            try
            {
                var usuario = await context.Users
                 .FirstOrDefaultAsync(u => u.IdUser == dto.Id);

                if (usuario == null)
                    return (false, "Usuario no encontrado.");

                if (!BCrypt.Net.BCrypt.Verify(dto.PasswordActual, usuario.password))
                    return (false, "La contraseña actual es incorrecta.");

                if (BCrypt.Net.BCrypt.Verify(dto.NuevaPassword, usuario.password))
                    return (false, "La nueva contraseña no puede ser igual a la actual.");

                if (dto.NuevaPassword != dto.ConfirmarPassword)
                    return (false, "La nueva contraseña y la confirmación no coinciden.");

                usuario.password = BCrypt.Net.BCrypt.HashPassword(dto.NuevaPassword);
                await context.SaveChangesAsync();

                return (true, "Contraseña actualizada correctamente.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al restablecer contraseña del usuario ID {Id}", dto.Id);
                return (false, "Ocurrió un error al restablecer la contraseña");
            }
        }
    }
}
