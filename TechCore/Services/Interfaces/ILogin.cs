using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechCore.Models.DTO.Autentificacion;

namespace TechCore.Services.Interfaces
{
    public interface ILogin
    {
        Task<ClaimsPrincipal?> LoginAsync(LoginDTO dto);
    }
}
