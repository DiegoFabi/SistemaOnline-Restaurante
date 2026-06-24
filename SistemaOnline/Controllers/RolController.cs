using SistemaOnline.Data;
using SistemaOnline.Models;
using SistemaOnline.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemaOnline.Controllers
{
    public class RolController : Controller
    {
        private readonly APPDBContext _context;
        public RolController(APPDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Rol> lista = await _context.Roles.ToListAsync();
            List<RolVM> modelo = lista.Select(r => new RolVM
            {
                ID_Rol = r.ID_Rol,
                Nombre_Rol = r.Nombre_Rol
            }).ToList();
            return View(modelo);
        }
        [HttpGet]
        public IActionResult Nuevo()
        {
            return View(new RolVM());
        }
        [HttpPost]
        public async Task<IActionResult> Nuevo(RolVM modelo)
        {
            Rol rol = new Rol
            {
                Nombre_Rol = modelo.Nombre_Rol
            };
            await _context.Roles.AddAsync(rol);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Rol rol = await _context.Roles.FirstAsync(r => r.ID_Rol == id);
            RolVM modelo = new RolVM
            {
                ID_Rol = rol.ID_Rol,
                Nombre_Rol = rol.Nombre_Rol
            };
            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(RolVM modelo)
        {
            Rol rol = await _context.Roles.FirstAsync(r => r.ID_Rol == modelo.ID_Rol);
            rol.Nombre_Rol = modelo.Nombre_Rol;
            _context.Roles.Update(rol);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }
        [HttpGet]
        public async Task<ActionResult> Eliminar(int id)
        {
            Rol rol = await _context.Roles.FirstAsync(r => r.ID_Rol == id);
            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }
    }
}