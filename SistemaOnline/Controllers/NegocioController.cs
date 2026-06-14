using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SistemaOnline.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class NegocioController : Controller
    {
        [HttpGet]
        public IActionResult Advertencia()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Confirmar(bool aceptaTerminos)
        {
            if (!aceptaTerminos)
            {
                ViewData["Msg"] = "Debes aceptar los términos para continuar.";
                return View("Advertencia");
            }
            return RedirectToAction("Gestion");
        }

        [HttpGet]
        public IActionResult Gestion()
        {
            return View();
        }
    }
}
