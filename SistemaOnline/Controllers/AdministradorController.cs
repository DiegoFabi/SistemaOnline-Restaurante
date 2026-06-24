using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaOnline.Data;
using SistemaOnline.ViewModels;

namespace SistemaOnline.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdministradorController : Controller
    {
        private readonly APPDBContext _dbcontext;
        public AdministradorController(APPDBContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var hoy = DateTime.Today;
            var manana = hoy.AddDays(1);
            var limiteContrato = hoy.AddDays(30);

            var vm = new AdminDashboardVM
            {
                ReservacionesHoy = await _dbcontext.Reservaciones.CountAsync(r => r.Fecha_Hora >= hoy && r.Fecha_Hora < manana),
                PedidosActivos = await _dbcontext.Pedidos.CountAsync(p => p.Estado_Pedido != "Completado" && p.Estado_Pedido != "Pagado" && p.Estado_Pedido != "Cancelado"),
                MesasOcupadas = await _dbcontext.Mesas.CountAsync(m => m.Estado == "Ocupada"),
                ContratosPorVencer = await _dbcontext.Contratos.CountAsync(c => c.Fecha_Fin >= hoy && c.Fecha_Fin <= limiteContrato)
            };

            vm.ProximasReservaciones = await _dbcontext.Reservaciones
                .Include(r => r.Cliente)
                .Include(r => r.Mesa_Restaurante)
                .Where(r => r.Fecha_Hora >= hoy)
                .OrderBy(r => r.Fecha_Hora)
                .Take(6)
                .ToListAsync();

            vm.Turnos = await _dbcontext.Turnos
                .Include(t => t.Empleado_Turnos)
                    .ThenInclude(et => et.Empleado)
                .ToListAsync();

            vm.AlertasContratos = await _dbcontext.Contratos
                .Include(c => c.Empleado)
                .Where(c => c.Fecha_Fin >= hoy && c.Fecha_Fin <= limiteContrato)
                .OrderBy(c => c.Fecha_Fin)
                .ToListAsync();

            return View(vm);
        }

        public async Task<IActionResult> Reservaciones()
        {
            var reservaciones = await _dbcontext.Reservaciones
                .Include(r => r.Cliente)
                .Include(r => r.Mesa_Restaurante)
                .OrderBy(r => r.Fecha_Hora)
                .ToListAsync();
            return View(reservaciones);
        }

        public async Task<IActionResult> Turnos()
        {
            var turnos = await _dbcontext.Turnos
                .Include(t => t.Empleado_Turnos)
                    .ThenInclude(et => et.Empleado)
                .ToListAsync();
            return View(turnos);
        }

        public async Task<IActionResult> Pedidos()
        {
            var pedidos = await _dbcontext.Pedidos
                .Include(p => p.Mesa_Restaurante)
                .Include(p => p.Empleado)
                .Where(p => p.Estado_Pedido != "Completado" && p.Estado_Pedido != "Pagado" && p.Estado_Pedido != "Cancelado")
                .OrderByDescending(p => p.ID_Pedido)
                .ToListAsync();

            ViewBag.TotalPedidos = pedidos.Count;
            ViewBag.MesasOcupadas = pedidos.Select(p => p.ID_Mesa).Distinct().Count();
            ViewBag.Pendientes = pedidos.Count(p => p.Estado_Pedido == "Pendiente");
            ViewBag.Servidos = pedidos.Count(p => p.Estado_Pedido == "Servido");

            return View(pedidos);
        }
    }
}