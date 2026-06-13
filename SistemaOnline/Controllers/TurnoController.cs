using SistemaOnline.Data;
using SistemaOnline.Models;
using SistemaOnline.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemaOnline.Controllers
{
    public class TurnoController : Controller
    {
        private readonly APPDBContext _context;
        public TurnoController(APPDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Turno> lista = await _context.Turnos.ToListAsync();
            List<TurnoVM> modelo = lista.Select(t => new TurnoVM
            {
                ID_Turno = t.ID_Turno,
                Nombre_Turno = t.Nombre_Turno,
                Hora_Inicio = t.Hora_Inicio,
                Hora_Fin = t.Hora_Fin,
                Dias_Semana = t.Dias_Semana
            }).ToList();
            return View(modelo);
        }
        [HttpGet]
        public IActionResult Nuevo()
        {
            return View(new TurnoVM());
        }
        [HttpPost]
        public async Task<IActionResult> Nuevo(TurnoVM modelo)
        {
            Turno turno = new Turno
            {
                ID_Turno = modelo.ID_Turno,
                Nombre_Turno = modelo.Nombre_Turno,
                Hora_Inicio = modelo.Hora_Inicio,
                Hora_Fin = modelo.Hora_Fin,
                Dias_Semana = modelo.Dias_Semana
            };
            await _context.Turnos.AddAsync(turno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Turno turno = await _context.Turnos.FirstAsync(t => t.ID_Turno == id);
            TurnoVM modelo = new TurnoVM
            {
                ID_Turno = turno.ID_Turno,
                Nombre_Turno = turno.Nombre_Turno,
                Hora_Inicio = turno.Hora_Inicio,
                Hora_Fin = turno.Hora_Fin,
                Dias_Semana = turno.Dias_Semana
            };
            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(TurnoVM modelo)
        {
            Turno turno = await _context.Turnos.FirstAsync(t => t.ID_Turno == modelo.ID_Turno);
            turno.Nombre_Turno = modelo.Nombre_Turno;
            turno.Hora_Inicio = modelo.Hora_Inicio;
            turno.Hora_Fin = modelo.Hora_Fin;
            turno.Dias_Semana = modelo.Dias_Semana;
            _context.Turnos.Update(turno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }
        [HttpGet]
        public async Task<ActionResult> Eliminar(int id)
        {
            Turno turno = await _context.Turnos.FirstAsync(t => t.ID_Turno == id);
            _context.Turnos.Remove(turno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }
    }
}
