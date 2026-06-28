using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaOnline.Data;
using SistemaOnline.Services;
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

        public async Task<IActionResult> Index(int page = 1, int pageSize = PaginationExtensions.DefaultPageSize)
        {
            var query = _dbcontext.Mesas.OrderBy(m => m.Numero_Mesa);
            var resultado = await query.ToPagedListAsync(page, pageSize);

            var vm = new MeseroDashboardVM
            {
                Mesas = resultado.Items,
                MesasLibres = await _dbcontext.Mesas.CountAsync(m => m.Estado == "Disponible"),
                MesasOcupadas = await _dbcontext.Mesas.CountAsync(m => m.Estado == "Ocupada"),
                PedidosActivos = await _dbcontext.Pedidos.CountAsync(p => p.Estado_Pedido != "Completado" && p.Estado_Pedido != "Pagado" && p.Estado_Pedido != "Cancelado")
            };

            vm.PedidosListos = await _dbcontext.Pedidos
                .Include(p => p.Mesa_Restaurante)
                .Where(p => p.Estado_Pedido == "Listo")
                .ToListAsync();

            ViewBag.Page = resultado.Page;
            ViewBag.PageSize = resultado.PageSize;
            ViewBag.TotalPages = resultado.TotalPages;
            ViewBag.TotalCount = resultado.TotalCount;
            return View(vm);
        }

        public async Task<IActionResult> Mesa(int id)
        {
            var mesa = await _dbcontext.Mesas.FirstOrDefaultAsync(m => m.ID_Mesa == id);
            if (mesa == null) return NotFound();

            var pedidos = await _dbcontext.Pedidos
                .Include(p => p.Empleado)
                .Include(p => p.Pedido_Detalles)
                    .ThenInclude(pd => pd.Producto)
                .Where(p => p.ID_Mesa == id && p.Estado_Pedido != "Completado" && p.Estado_Pedido != "Pagado" && p.Estado_Pedido != "Cancelado")
                .OrderByDescending(p => p.ID_Pedido)
                .ToListAsync();

            ViewBag.Mesa = mesa;
            return View(pedidos);
        }

        public async Task<IActionResult> Pedidos(int page = 1, int pageSize = PaginationExtensions.DefaultPageSize)
        {
            var query = _dbcontext.Pedidos
                .Include(p => p.Mesa_Restaurante)
                .Include(p => p.Pedido_Detalles)
                    .ThenInclude(pd => pd.Producto)
                .Where(p => p.Estado_Pedido != "Completado" && p.Estado_Pedido != "Pagado" && p.Estado_Pedido != "Cancelado")
                .OrderByDescending(p => p.ID_Pedido);

            var resultado = await query.ToPagedListAsync(page, pageSize);
            ViewBag.Page = resultado.Page;
            ViewBag.PageSize = resultado.PageSize;
            ViewBag.TotalPages = resultado.TotalPages;
            ViewBag.TotalCount = resultado.TotalCount;
            return View(resultado.Items);
        }

        public async Task<IActionResult> Notificaciones(int page = 1, int pageSize = PaginationExtensions.DefaultPageSize)
        {
            var query = _dbcontext.Pedidos
                .Include(p => p.Mesa_Restaurante)
                .Where(p => p.Estado_Pedido == "Listo")
                .OrderByDescending(p => p.ID_Pedido);

            var resultado = await query.ToPagedListAsync(page, pageSize);
            ViewBag.Page = resultado.Page;
            ViewBag.PageSize = resultado.PageSize;
            ViewBag.TotalPages = resultado.TotalPages;
            ViewBag.TotalCount = resultado.TotalCount;
            return View(resultado.Items);
        }
    }
}