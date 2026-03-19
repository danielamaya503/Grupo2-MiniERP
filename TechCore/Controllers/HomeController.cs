using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TechCore.Models;

namespace TechCore.Controllers;

[Authorize]
public class HomeController : Controller
{


    public IActionResult Index()
    {
        if (User.IsInRole("Administrador"))
            return RedirectToAction("Index", "Usuario");

        if (User.IsInRole("Bodega"))
            return RedirectToAction("Index", "Bodega");

        if (User.IsInRole("Vendedor"))
            return RedirectToAction("Perfil", "Usuario");

        if (User.IsInRole("Contador"))
            return RedirectToAction("Perfil", "Usuario");

        return RedirectToAction("Perfil", "Usuario"); 
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    
}
