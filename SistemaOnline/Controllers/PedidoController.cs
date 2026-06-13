using SistemaOnline.Data;
using SistemaOnline.Models;
using SistemaOnline.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace SistemaOnline.Controllers
{
    public class PedidoController : Controller
    {
        private readonly APPDBContext _context;
        public PedidoController(APPDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Pedido> lista = await _context.Pedidos
                .Include(p => p.Empleado)
                .Include(p => p.Cliente)
                .Include(p => p.Mesa_Restaurante)
                .ToListAsync();
            List<PedidoVM> modelo = lista.Select(p => new PedidoVM
            {
                ID_Pedido = p.ID_Pedido,
                Fecha = p.Fecha,
                Estado_Pedido = p.Estado_Pedido,
                Detalle_Pedido = p.Detalle_Pedido,
                SubTotal = p.SubTotal,
                Total = p.Total,
                ID_Empleado = p.ID_Empleado,
                ID_Cliente = p.ID_Cliente,
                ID_Mesa = p.ID_Mesa,
                EmpleadoNombre = $"{p.Empleado.Nombre} {p.Empleado.Apellidos}",
                ClienteNombre = $"{p.Cliente.Nombre} {p.Cliente.Apellidos}",
                MesaNumero = p.Mesa_Restaurante.Numero_Mesa.ToString()
            }).ToList();
            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> Nuevo()
        {
            PedidoVM modelo = new PedidoVM
            {
                EmpleadosDisponibles = await ObtenerEmpleados(),
                ClientesDisponibles = await ObtenerClientes(),
                MesasDisponibles = await ObtenerMesas()
            };
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Nuevo(PedidoVM modelo)
        {
            Pedido pedido = new Pedido
            {
                Fecha = modelo.Fecha,
                Estado_Pedido = modelo.Estado_Pedido,
                Detalle_Pedido = modelo.Detalle_Pedido,
                SubTotal = modelo.SubTotal,
                Total = modelo.Total,
                ID_Empleado = modelo.ID_Empleado,
                ID_Cliente = modelo.ID_Cliente,
                ID_Mesa = modelo.ID_Mesa
            };
            await _context.Pedidos.AddAsync(pedido);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Pedido pedido = await _context.Pedidos.FirstAsync(p => p.ID_Pedido == id);
            PedidoVM modelo = new PedidoVM
            {
                ID_Pedido = pedido.ID_Pedido,
                Fecha = pedido.Fecha,
                Estado_Pedido = pedido.Estado_Pedido,
                Detalle_Pedido = pedido.Detalle_Pedido,
                SubTotal = pedido.SubTotal,
                Total = pedido.Total,
                ID_Empleado = pedido.ID_Empleado,
                ID_Cliente = pedido.ID_Cliente,
                ID_Mesa = pedido.ID_Mesa,
                EmpleadosDisponibles = await ObtenerEmpleados(),
                ClientesDisponibles = await ObtenerClientes(),
                MesasDisponibles = await ObtenerMesas()
            };
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(PedidoVM modelo)
        {
            Pedido pedido = await _context.Pedidos.FirstAsync(p => p.ID_Pedido == modelo.ID_Pedido);
            pedido.Fecha = modelo.Fecha;
            pedido.Estado_Pedido = modelo.Estado_Pedido;
            pedido.Detalle_Pedido = modelo.Detalle_Pedido;
            pedido.SubTotal = modelo.SubTotal;
            pedido.Total = modelo.Total;
            pedido.ID_Empleado = modelo.ID_Empleado;
            pedido.ID_Cliente = modelo.ID_Cliente;
            pedido.ID_Mesa = modelo.ID_Mesa;
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        [HttpGet]
        public async Task<ActionResult> Eliminar(int id)
        {
            Pedido pedido = await _context.Pedidos.FirstAsync(p => p.ID_Pedido == id);
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        private async Task<List<SelectListItem>> ObtenerEmpleados()
        {
            var lista = await _context.Empleados.Select(e => new SelectListItem
            {
                Value = e.ID_Empleado.ToString(),
                Text = $"{e.Nombre} {e.Apellidos}"
            }).ToListAsync();
            lista.Insert(0, new SelectListItem { Value = "", Text = "Selecciona un empleado" });
            return lista;
        }

        private async Task<List<SelectListItem>> ObtenerClientes()
        {
            var lista = await _context.Clientes.Select(c => new SelectListItem
            {
                Value = c.ID_Cliente.ToString(),
                Text = $"{c.Nombre} {c.Apellidos}"
            }).ToListAsync();
            lista.Insert(0, new SelectListItem { Value = "", Text = "Selecciona un cliente" });
            return lista;
        }

        private async Task<List<SelectListItem>> ObtenerMesas()
        {
            var lista = await _context.Mesas.Select(m => new SelectListItem
            {
                Value = m.ID_Mesa.ToString(),
                Text = "Mesa " + m.Numero_Mesa
            }).ToListAsync();
            lista.Insert(0, new SelectListItem { Value = "", Text = "Selecciona una mesa" });
            return lista;
        }
    }
}
