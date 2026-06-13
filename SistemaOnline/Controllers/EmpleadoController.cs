using SistemaOnline.Data;
using SistemaOnline.Models;
using SistemaOnline.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace SistemaOnline.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly APPDBContext _context;
        public EmpleadoController(APPDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Empleado> lista = await _context.Empleados.Include(e => e.Usuario).ToListAsync();
            List<EmpleadoVM> modelo = lista.Select(e => new EmpleadoVM
            {
                ID_Empleado = e.ID_Empleado,
                Nombre = e.Nombre,
                Apellidos = e.Apellidos,
                Direccion = e.Direccion,
                Cargo = e.Cargo,
                Telefono = e.Telefono,
                Estado = e.Estado,
                DNI = e.DNI,
                ID_Usuario = e.ID_Usuario,
                UsuarioNombre = e.Usuario != null ? e.Usuario.Nombre_Usuario : "-"
            }).ToList();
            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> Nuevo()
        {
            EmpleadoVM modelo = new EmpleadoVM
            {
                UsuariosDisponibles = await ObtenerUsuarios()
            };
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Nuevo(EmpleadoVM modelo)
        {
            Empleado empleado = new Empleado
            {
                Nombre = modelo.Nombre,
                Apellidos = modelo.Apellidos,
                Direccion = modelo.Direccion,
                Cargo = modelo.Cargo,
                Telefono = modelo.Telefono,
                Estado = modelo.Estado,
                DNI = modelo.DNI,
                ID_Usuario = modelo.ID_Usuario
            };
            await _context.Empleados.AddAsync(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Empleado empleado = await _context.Empleados.FirstAsync(e => e.ID_Empleado == id);
            EmpleadoVM modelo = new EmpleadoVM
            {
                ID_Empleado = empleado.ID_Empleado,
                Nombre = empleado.Nombre,
                Apellidos = empleado.Apellidos,
                Direccion = empleado.Direccion,
                Cargo = empleado.Cargo,
                Telefono = empleado.Telefono,
                Estado = empleado.Estado,
                DNI = empleado.DNI,
                ID_Usuario = empleado.ID_Usuario,
                UsuariosDisponibles = await ObtenerUsuarios()
            };
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(EmpleadoVM modelo)
        {
            Empleado empleado = await _context.Empleados.FirstAsync(e => e.ID_Empleado == modelo.ID_Empleado);
            empleado.Nombre = modelo.Nombre;
            empleado.Apellidos = modelo.Apellidos;
            empleado.Direccion = modelo.Direccion;
            empleado.Cargo = modelo.Cargo;
            empleado.Telefono = modelo.Telefono;
            empleado.Estado = modelo.Estado;
            empleado.DNI = modelo.DNI;
            empleado.ID_Usuario = modelo.ID_Usuario;
            _context.Empleados.Update(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        [HttpGet]
        public async Task<ActionResult> Eliminar(int id)
        {
            Empleado empleado = await _context.Empleados.FirstAsync(e => e.ID_Empleado == id);
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        private async Task<List<SelectListItem>> ObtenerUsuarios()
        {
            List<SelectListItem> lista = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "-- Ninguno --" }
            };
            lista.AddRange(await _context.Usuarios.Select(u => new SelectListItem
            {
                Value = u.ID_Usuario.ToString(),
                Text = u.Nombre_Usuario
            }).ToListAsync());
            return lista;
        }
    }
}
