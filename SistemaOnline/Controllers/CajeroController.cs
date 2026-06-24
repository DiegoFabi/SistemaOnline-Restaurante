using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaOnline.Data;
using SistemaOnline.ViewModels;

namespace SistemaOnline.Controllers
{
    [Authorize(Roles = "Cajero")]
    public class CajeroController : Controller
    {
        private readonly APPDBContext _dbcontext;
        public CajeroController(APPDBContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var hoy = DateTime.Today;
            var manana = hoy.AddDays(1);

            var vm = new CajeroDashboardVM
            {
                TotalRecaudadoHoy = await _dbcontext.Pagos
                    .Where(pg => pg.Fecha_Hora_Pago >= hoy && pg.Fecha_Hora_Pago < manana && pg.Estado == "Aprobado")
                    .SumAsync(pg => (decimal?)pg.Monto) ?? 0,
                PagosHoyCount = await _dbcontext.Pagos.CountAsync(pg => pg.Fecha_Hora_Pago >= hoy && pg.Fecha_Hora_Pago < manana),
                PedidosPorCobrar = await _dbcontext.Pedidos.CountAsync(p => p.Estado_Pedido != "Pagado" && p.Estado_Pedido != "Cancelado")
            };

            vm.PedidosPendientes = await _dbcontext.Pedidos
                .Include(p => p.Mesa_Restaurante)
                .Where(p => p.Estado_Pedido != "Pagado" && p.Estado_Pedido != "Cancelado")
                .OrderByDescending(p => p.ID_Pedido)
                .Take(10)
                .ToListAsync();

            vm.PagosRecientes = await _dbcontext.Pagos
                .Include(pg => pg.Pedido)
                .OrderByDescending(pg => pg.Fecha_Hora_Pago)
                .Take(6)
                .ToListAsync();

            vm.MetodosPago = await _dbcontext.Pagos
                .Where(pg => pg.Fecha_Hora_Pago >= hoy && pg.Fecha_Hora_Pago < manana)
                .GroupBy(pg => pg.Metodo_Pago)
                .Select(g => new MetodoPagoVM
                {
                    Metodo = g.Key,
                    Total = g.Sum(pg => pg.Monto),
                    Cantidad = g.Count()
                })
                .ToListAsync();

            return View(vm);
        }

        public async Task<IActionResult> Facturacion()
        {
            var pedidos = await _dbcontext.Pedidos
                .Include(p => p.Mesa_Restaurante)
                .Where(p => p.Estado_Pedido != "Cancelado")
                .OrderByDescending(p => p.ID_Pedido)
                .ToListAsync();
            return View(pedidos);
        }

        public async Task<IActionResult> Pagos()
        {
            var pagos = await _dbcontext.Pagos
                .Include(pg => pg.Pedido)
                .OrderByDescending(pg => pg.Fecha_Hora_Pago)
                .ToListAsync();
            return View(pagos);
        }

        public async Task<IActionResult> Clientes(string? buscar)
        {
            var query = _dbcontext.Clientes.AsQueryable();
            if (!string.IsNullOrWhiteSpace(buscar))
            {
                query = query.Where(c => c.Nombre.Contains(buscar) || c.Apellidos.Contains(buscar) || c.DNI.Contains(buscar) || c.Email.Contains(buscar));
            }
            var clientes = await query.OrderBy(c => c.Nombre).ToListAsync();
            ViewBag.Buscar = buscar;
            return View(clientes);
        }
    }
}