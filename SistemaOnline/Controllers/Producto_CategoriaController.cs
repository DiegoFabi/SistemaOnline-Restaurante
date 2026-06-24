using SistemaOnline.Data;
using SistemaOnline.Models;
using SistemaOnline.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace SistemaOnline.Controllers
{
    public class Producto_CategoriaController : Controller
    {
        private readonly APPDBContext _context;
        public Producto_CategoriaController(APPDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Producto_Categoria> lista = await _context.Productos_Categorias.Include(pc => pc.Carta).ToListAsync();
            List<Producto_CategoriaVM> modelo = lista.Select(pc => new Producto_CategoriaVM
            {
                ID_Categoria = pc.ID_Categoria,
                Nombre_Categoria = pc.Nombre_Categoria,
                Descripcion = pc.Descripcion,
                ID_Carta = pc.ID_Carta,
                CartaNombre = pc.Carta.Nombre_Carta
            }).ToList();
            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> Nuevo()
        {
            if (!await _context.Cartas.AnyAsync())
            {
                ViewData["Msg"] = "Debes crear al menos una Carta antes de registrar Categorías de Producto.";
                return View("~/Views/Negocio/Advertencia.cshtml");
            }

            Producto_CategoriaVM modelo = new Producto_CategoriaVM
            {
                CartasDisponibles = await ObtenerCartas()
            };
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Nuevo(Producto_CategoriaVM modelo)
        {
            if (!ModelState.IsValid)
            {
                modelo.CartasDisponibles = await ObtenerCartas();
                return View(modelo);
            }

            Producto_Categoria categoria = new Producto_Categoria
            {
                Nombre_Categoria = modelo.Nombre_Categoria,
                Descripcion = modelo.Descripcion,
                ID_Carta = modelo.ID_Carta
            };
            bool eraPrimeraCategoria = !await _context.Productos_Categorias.AnyAsync();
            await _context.Productos_Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();
            if (eraPrimeraCategoria)
            {
                Services.NotificacionStore.Agregar("lock_open", "Módulo desbloqueado", "El módulo de Productos ya está disponible.");
            }
            return RedirectToAction(nameof(Lista));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Producto_Categoria categoria = await _context.Productos_Categorias.FirstAsync(pc => pc.ID_Categoria == id);
            Producto_CategoriaVM modelo = new Producto_CategoriaVM
            {
                ID_Categoria = categoria.ID_Categoria,
                Nombre_Categoria = categoria.Nombre_Categoria,
                Descripcion = categoria.Descripcion,
                ID_Carta = categoria.ID_Carta,
                CartasDisponibles = await ObtenerCartas()
            };
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Producto_CategoriaVM modelo)
        {
            if (!ModelState.IsValid)
            {
                modelo.CartasDisponibles = await ObtenerCartas();
                return View(modelo);
            }

            Producto_Categoria categoria = await _context.Productos_Categorias.FirstAsync(pc => pc.ID_Categoria == modelo.ID_Categoria);
            categoria.Nombre_Categoria = modelo.Nombre_Categoria;
            categoria.Descripcion = modelo.Descripcion;
            categoria.ID_Carta = modelo.ID_Carta;
            _context.Productos_Categorias.Update(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        [HttpGet]
        public async Task<ActionResult> Eliminar(int id)
        {
            Producto_Categoria categoria = await _context.Productos_Categorias.FirstAsync(pc => pc.ID_Categoria == id);
            _context.Productos_Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        private async Task<List<SelectListItem>> ObtenerCartas()
        {
            var lista = await _context.Cartas.Select(c => new SelectListItem
            {
                Value = c.ID_Carta.ToString(),
                Text = c.Nombre_Carta
            }).ToListAsync();
            lista.Insert(0, new SelectListItem { Value = "", Text = "Selecciona una carta" });
            return lista;
        }
    }
}