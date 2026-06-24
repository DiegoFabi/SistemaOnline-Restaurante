using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaOnline.Data;
using SistemaOnline.ViewModels;

namespace SistemaOnline.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class DashboardController : Controller
    {
        private readonly APPDBContext _dbcontext;
        public DashboardController(APPDBContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var hoy = DateTime.Today;

            var vm = new AdminDashboardVM
            {
                VentasTotales = await _dbcontext.Pedidos.SumAsync(p => (decimal?)p.Total) ?? 0,
                VentasHoy = await _dbcontext.Pedidos.Where(p => p.Fecha == hoy).SumAsync(p => (decimal?)p.Total) ?? 0,
                PedidosActivos = await _dbcontext.Pedidos.CountAsync(p => p.Estado_Pedido != "Completado" && p.Estado_Pedido != "Pagado" && p.Estado_Pedido != "Cancelado"),
                ClientesTotales = await _dbcontext.Clientes.CountAsync(),
                AlertasInventario = await _dbcontext.Inventarios.CountAsync(i => i.Cantidad_Stock <= i.Stock_Minimo)
            };

            vm.InventarioCritico = await _dbcontext.Inventarios
                .Include(i => i.Ingrediente)
                .Where(i => i.Cantidad_Stock <= i.Stock_Minimo)
                .OrderBy(i => i.Cantidad_Stock)
                .Take(5)
                .ToListAsync();

            vm.PedidosRecientes = await _dbcontext.Pedidos
                .Include(p => p.Mesa_Restaurante)
                .OrderByDescending(p => p.ID_Pedido)
                .Take(5)
                .ToListAsync();

            vm.TopEmpleados = await _dbcontext.Pedidos
                .Include(p => p.Empleado)
                .Where(p => p.Empleado != null)
                .GroupBy(p => new { p.ID_Empleado, p.Empleado.Nombre, p.Empleado.Apellidos })
                .Select(g => new EmpleadoVentaVM
                {
                    NombreCompleto = g.Key.Nombre + " " + g.Key.Apellidos,
                    TotalVendido = g.Sum(p => p.Total),
                    CantidadPedidos = g.Count()
                })
                .OrderByDescending(e => e.TotalVendido)
                .Take(3)
                .ToListAsync();

            var ventasAgrupadas = await _dbcontext.Pedidos
                .Where(p => p.Fecha >= hoy.AddMonths(-5))
                .GroupBy(p => new { p.Fecha.Year, p.Fecha.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, Total = g.Sum(p => p.Total) })
                .ToListAsync();

            var culturaEs = new System.Globalization.CultureInfo("es-ES");
            vm.VentasPorMes = Enumerable.Range(0, 6)
                .Select(i => hoy.AddMonths(-5 + i))
                .Select(fecha =>
                {
                    var datoMes = ventasAgrupadas.FirstOrDefault(v => v.Year == fecha.Year && v.Month == fecha.Month);
                    return new VentaMensualVM
                    {
                        Mes = culturaEs.DateTimeFormat.GetAbbreviatedMonthName(fecha.Month),
                        Total = datoMes?.Total ?? 0
                    };
                })
                .ToList();

            var coloresPorEstado = new Dictionary<string, string>
            {
                ["Pendiente"] = "#f5a623",
                ["Servido"] = "#4f9d69",
                ["Pagado"] = "#3b7dd8",
                ["Cancelado"] = "#d8463b"
            };

            var pedidosAgrupados = await _dbcontext.Pedidos
                .GroupBy(p => p.Estado_Pedido)
                .Select(g => new { Estado = g.Key, Cantidad = g.Count() })
                .ToListAsync();

            vm.PedidosPorEstado = pedidosAgrupados
                .Select(g => new EstadoPedidoVM
                {
                    Estado = g.Estado,
                    Cantidad = g.Cantidad,
                    Color = coloresPorEstado.TryGetValue(g.Estado, out var color) ? color : "#9aa0a6"
                })
                .ToList();

            return View(vm);
        }

        public async Task<IActionResult> Usuarios()
        {
            var usuarios = await _dbcontext.Usuarios.Include(u => u.Rol).ToListAsync();
            return View(usuarios);
        }

        public async Task<IActionResult> Inventario()
        {
            var inventario = await _dbcontext.Inventarios.Include(i => i.Ingrediente).ToListAsync();
            return View(inventario);
        }

        public async Task<IActionResult> Reportes()
        {
            var pedidos = await _dbcontext.Pedidos
                .OrderByDescending(p => p.Fecha)
                .Take(50)
                .ToListAsync();
            return View(pedidos);
        }
    }
}