using SistemaOnline.Data;
using SistemaOnline.Models;
using SistemaOnline.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemaOnline.Controllers
{
    public class CartaController : Controller
    {
        private readonly APPDBContext _context;
        public CartaController(APPDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Carta> lista = await _context.Cartas.ToListAsync();
            List<CartaVM> modelo = lista.Select(c => new CartaVM
            {
                ID_Carta = c.ID_Carta,
                Nombre_Carta = c.Nombre_Carta,
                Cantidad_Platos = c.Cantidad_Platos,
                Descripcion = c.Descripcion,
                Precio = c.Precio
            }).ToList();
            return View(modelo);
        }
        [HttpGet]
        public IActionResult Nuevo()
        {
            return View(new CartaVM());
        }
        [HttpPost]
        public async Task<IActionResult> Nuevo(CartaVM modelo)
        {
            Carta carta = new Carta
            {
                ID_Carta = modelo.ID_Carta,
                Nombre_Carta = modelo.Nombre_Carta,
                Cantidad_Platos = modelo.Cantidad_Platos,
                Descripcion = modelo.Descripcion,
                Precio = modelo.Precio
            };
            await _context.Cartas.AddAsync(carta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Carta carta = await _context.Cartas.FirstAsync(c => c.ID_Carta == id);
            CartaVM modelo = new CartaVM
            {
                ID_Carta = carta.ID_Carta,
                Nombre_Carta = carta.Nombre_Carta,
                Cantidad_Platos = carta.Cantidad_Platos,
                Descripcion = carta.Descripcion,
                Precio = carta.Precio
            };
            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(CartaVM modelo)
        {
            Carta carta = await _context.Cartas.FirstAsync(c => c.ID_Carta == modelo.ID_Carta);
            carta.Nombre_Carta = modelo.Nombre_Carta;
            carta.Cantidad_Platos = modelo.Cantidad_Platos;
            carta.Descripcion = modelo.Descripcion;
            carta.Precio = modelo.Precio;
            _context.Cartas.Update(carta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }
        [HttpGet]
        public async Task<ActionResult> Eliminar(int id)
        {
            Carta carta = await _context.Cartas.FirstAsync(c => c.ID_Carta == id);
            _context.Cartas.Remove(carta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }
    }
}
