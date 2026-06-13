using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaOnline.Data;
using SistemaOnline.Models;
using SistemaOnline.ViewModels;
using System.Security.Claims;

namespace SistemaOnline.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class LoginController : Controller
    {
        private readonly APPDBContext _dbcontext;
        public LoginController(APPDBContext dbContext)
        {
            _dbcontext = dbContext;
        }
        [HttpGet]
        public IActionResult Registro()
        {
            ViewBag.Roles = _dbcontext.Roles.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registro(UsuarioVM modelo)
        {
            if (modelo.Password != modelo.RepeatPassword)
            {
                ViewBag.Rol = _dbcontext.Roles.ToList();
                ViewData["Msg"] = "La contraseñas no coinciden, escribe denuevo";
                return View();
            }
            Usuario user = new Usuario()
            {
                Nombre_Usuario = modelo.Nombre_Usuario,
                Email = modelo.Email,
                Password = modelo.Password,
                ID_Rol = modelo.ID_Rol
            };
            await _dbcontext.Usuarios.AddAsync(user);
            await _dbcontext.SaveChangesAsync();
            if (user.ID_Usuario != 0) return RedirectToAction("Login", "Login");
            ViewData["Msg"] = "El usuario no se creo";
            ViewBag.Roles = _dbcontext.Roles.ToList();
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM modelo)
        {
            Usuario? existe = await _dbcontext.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(
                u => u.Email == modelo.Email &&
                u.Password == modelo.Password);

            if (existe == null)
            {
                ViewData["Msg"] = "El usuario no exise o esta mal escrito";
                return View();
            }
            //Claims
            var claim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, existe.Nombre_Usuario),
                new Claim(ClaimTypes.Email, existe.Email),
                new Claim(ClaimTypes.Role, existe.Rol.Nombre_Rol),
                new Claim("idUser", existe.ID_Usuario.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
            var propiedad = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true
            };
            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            propiedad
            );
            //Sesiones por rol
            HttpContext.Session.SetString("User", existe.Nombre_Usuario);
            HttpContext.Session.SetString("Rol", existe.Rol.Nombre_Rol);
            HttpContext.Session.SetInt32("idUser", existe.ID_Rol);
            switch (existe.Rol.Nombre_Rol)
            {
                case "Administrador":
                    return RedirectToAction("Index", "Administrador");
                case "Cocinero":
                    return RedirectToAction("Cocinero", "Login");
                case "Mesero":
                    return RedirectToAction("Mesero", "Login");
                case "Cajero":
                    return RedirectToAction("Cajero", "Login");
                default:
                    return RedirectToAction("Cliente", "Login");
            }
        }
        [HttpGet]
        public IActionResult Administrador()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Mesero()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Cajero()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Cliente()
        {
            return View();
        }

    }
}
