using SistemaOnline.Data;
using SistemaOnline.Models;
using SistemaOnline.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace SistemaOnline.Controllers
{
    public class Comprobante_PagoController : Controller
    {
        private readonly APPDBContext _context;
        public Comprobante_PagoController(APPDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Comprobante_Pago> lista = await _context.Comprobantes_Pagos.Include(c => c.Pedido).ToListAsync();
            List<Comprobante_PagoVM> modelo = lista.Select(c => new Comprobante_PagoVM
            {
                ID_Comprobante = c.ID_Comprobante,
                Tipo_Comprobante = c.Tipo_Comprobante,
                Numero_Comprobante = c.Numero_Comprobante,
                Serie = c.Serie,
                Fecha_Emision = c.Fecha_Emision,
                Sub_Total = c.Sub_Total,
                Monto_Total = c.Monto_Total,
                IGV = c.IGV,
                Estado_Comprobante = c.Estado_Comprobante,
                Metodo_Pago = c.Metodo_Pago,
                Razon_Social = c.Razon_Social,
                RUC = c.RUC,
                Direccion_Fiscal = c.Direccion_Fiscal,
                ID_Pedido = c.ID_Pedido,
                PedidoInfo = $"Pedido #{c.Pedido.ID_Pedido} - {c.Pedido.Estado_Pedido}"
            }).ToList();
            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> Nuevo()
        {
            Comprobante_PagoVM modelo = new Comprobante_PagoVM
            {
                PedidosDisponibles = await ObtenerPedidos()
            };
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Nuevo(Comprobante_PagoVM modelo)
        {
            Comprobante_Pago comprobante = new Comprobante_Pago
            {
                Tipo_Comprobante = modelo.Tipo_Comprobante,
                Numero_Comprobante = modelo.Numero_Comprobante,
                Serie = modelo.Serie,
                Fecha_Emision = modelo.Fecha_Emision,
                Sub_Total = modelo.Sub_Total,
                Monto_Total = modelo.Monto_Total,
                IGV = modelo.IGV,
                Estado_Comprobante = modelo.Estado_Comprobante,
                Metodo_Pago = modelo.Metodo_Pago,
                Razon_Social = modelo.Razon_Social,
                RUC = modelo.RUC,
                Direccion_Fiscal = modelo.Direccion_Fiscal,
                ID_Pedido = modelo.ID_Pedido
            };
            await _context.Comprobantes_Pagos.AddAsync(comprobante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            Comprobante_Pago comprobante = await _context.Comprobantes_Pagos.FirstAsync(c => c.ID_Comprobante == id);
            Comprobante_PagoVM modelo = new Comprobante_PagoVM
            {
                ID_Comprobante = comprobante.ID_Comprobante,
                Tipo_Comprobante = comprobante.Tipo_Comprobante,
                Numero_Comprobante = comprobante.Numero_Comprobante,
                Serie = comprobante.Serie,
                Fecha_Emision = comprobante.Fecha_Emision,
                Sub_Total = comprobante.Sub_Total,
                Monto_Total = comprobante.Monto_Total,
                IGV = comprobante.IGV,
                Estado_Comprobante = comprobante.Estado_Comprobante,
                Metodo_Pago = comprobante.Metodo_Pago,
                Razon_Social = comprobante.Razon_Social,
                RUC = comprobante.RUC,
                Direccion_Fiscal = comprobante.Direccion_Fiscal,
                ID_Pedido = comprobante.ID_Pedido,
                PedidosDisponibles = await ObtenerPedidos()
            };
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Comprobante_PagoVM modelo)
        {
            Comprobante_Pago comprobante = await _context.Comprobantes_Pagos.FirstAsync(c => c.ID_Comprobante == modelo.ID_Comprobante);
            comprobante.Tipo_Comprobante = modelo.Tipo_Comprobante;
            comprobante.Numero_Comprobante = modelo.Numero_Comprobante;
            comprobante.Serie = modelo.Serie;
            comprobante.Fecha_Emision = modelo.Fecha_Emision;
            comprobante.Sub_Total = modelo.Sub_Total;
            comprobante.Monto_Total = modelo.Monto_Total;
            comprobante.IGV = modelo.IGV;
            comprobante.Estado_Comprobante = modelo.Estado_Comprobante;
            comprobante.Metodo_Pago = modelo.Metodo_Pago;
            comprobante.Razon_Social = modelo.Razon_Social;
            comprobante.RUC = modelo.RUC;
            comprobante.Direccion_Fiscal = modelo.Direccion_Fiscal;
            comprobante.ID_Pedido = modelo.ID_Pedido;
            _context.Comprobantes_Pagos.Update(comprobante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        [HttpGet]
        public async Task<ActionResult> Eliminar(int id)
        {
            Comprobante_Pago comprobante = await _context.Comprobantes_Pagos.FirstAsync(c => c.ID_Comprobante == id);
            _context.Comprobantes_Pagos.Remove(comprobante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Lista));
        }

        private async Task<List<SelectListItem>> ObtenerPedidos()
        {
            var lista = await _context.Pedidos.Select(p => new SelectListItem
            {
                Value = p.ID_Pedido.ToString(),
                Text = "Pedido #" + p.ID_Pedido + " - " + p.Estado_Pedido
            }).ToListAsync();
            lista.Insert(0, new SelectListItem { Value = "", Text = "Selecciona un pedido" });
            return lista;
        }
    }
}
