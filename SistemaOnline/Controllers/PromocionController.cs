using SistemaOnline.Data;
using SistemaOnline.Models;
using SistemaOnline.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemaOnline.Controllers
{
    public class PromocionController : Controller
    {
        private readonly APPDBContext _context;
        public PromocionController(APPDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Promocion> lista = await _context.Promociones.ToListAsync();
            List<PromocionVM> modelo = lista.Select(p => new PromocionVM
            {
                ID_Promocion = p.ID_Promocion,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Porcentaje_Descuento = p.Porcentaje_Descuento,
                Fecha_Inicio = p.Fecha_Inicio,
                Fecha_Fin = p.Fecha_Fin,
                Estado = p.Estado
            }).ToList();
            return View(modelo);
        }

        [HttpGet]
        public IActionResult Nuevo()
        {
            PromocionVM modelo = new PromocionVM();
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Nuevo(PromocionVM modelo)
        {
            Promocion promocion = new Promocion
            {
                Nombre = modelo.Nombre,
                Descripcion = modelo.Descripcion,
                Porcentaje_Descuento = modelo.Porcentaje_Descuento,
                Fecha_Inicio = modelo.Fecha_Inicio,
                Fecha_Fin = modelo.Fecha_Fin,
                Estado = modelo.Estado
            };
            await _context.Promociones.AddAsync(promocion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Promocion promocion = await _context.Promociones.FirstAsync(p => p.ID_Promocion == id);
            PromocionVM modelo = new PromocionVM
            {
                ID_Promocion = promocion.ID_Promocion,
                Nombre = promocion.Nombre,
                Descripcion = promocion.Descripcion,
                Porcentaje_Descuento = promocion.Porcentaje_Descuento,
                Fecha_Inicio = promocion.Fecha_Inicio,
                Fecha_Fin = promocion.Fecha_Fin,
                Estado = promocion.Estado
            };
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(PromocionVM modelo)
        {
            Promocion promocion = await _context.Promociones.FirstAsync(p => p.ID_Promocion == modelo.ID_Promocion);
            promocion.Nombre = modelo.Nombre;
            promocion.Descripcion = modelo.Descripcion;
            promocion.Porcentaje_Descuento = modelo.Porcentaje_Descuento;
            promocion.Fecha_Inicio = modelo.Fecha_Inicio;
            promocion.Fecha_Fin = modelo.Fecha_Fin;
            promocion.Estado = modelo.Estado;
            _context.Promociones.Update(promocion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        [HttpGet]
        public async Task<ActionResult> Eliminar(int id)
        {
            Promocion promocion = await _context.Promociones.FirstAsync(p => p.ID_Promocion == id);
            _context.Promociones.Remove(promocion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }
    }
}
