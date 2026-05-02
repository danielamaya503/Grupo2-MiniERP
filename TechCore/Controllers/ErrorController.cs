using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TechCore.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public ActionResult Error()
        {
            return View("Error");
        }

        
    }
}
