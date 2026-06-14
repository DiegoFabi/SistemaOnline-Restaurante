using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaOnline.Data;
using SistemaOnline.ViewModels;

namespace SistemaOnline.Controllers
{
    [Authorize(Roles = "Mesero")]
    public class MeseroController : Controller
    {
        private readonly APPDBContext _dbcontext;
        public MeseroController(APPDBContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var mesas = await _dbcontext.Mesas.OrderBy(m => m.Numero_Mesa).ToListAsync();

            var vm = new MeseroDashboardVM
            {
                Mesas = mesas,
                MesasLibres = mesas.Count(m => m.Estado == "Libre"),
                MesasOcupadas = mesas.Count(m => m.Estado == "Ocupada"),
                PedidosActivos = await _dbcontext.Pedidos.CountAsync(p => p.Estado_Pedido != "Completado" && p.Estado_Pedido != "Pagado" && p.Estado_Pedido != "Cancelado")
            };

            vm.PedidosListos = await _dbcontext.Pedidos
                .Include(p => p.Mesa_Restaurante)
                .Where(p => p.Estado_Pedido == "Listo")
                .ToListAsync();

            return View(vm);
        }

        public async Task<IActionResult> Pedidos()
        {
            var pedidos = await _dbcontext.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Mesa_Restaurante)
                .Include(p => p.Pedido_Detalles)
                    .ThenInclude(pd => pd.Producto)
                .Where(p => p.Estado_Pedido != "Completado" && p.Estado_Pedido != "Pagado" && p.Estado_Pedido != "Cancelado")
                .OrderByDescending(p => p.ID_Pedido)
                .ToListAsync();
            return View(pedidos);
        }

        public async Task<IActionResult> Notificaciones()
        {
            var pedidosListos = await _dbcontext.Pedidos
                .Include(p => p.Mesa_Restaurante)
                .Where(p => p.Estado_Pedido == "Listo")
                .ToListAsync();
            return View(pedidosListos);
        }
    }
}
