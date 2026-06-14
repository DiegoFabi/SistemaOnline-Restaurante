using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaOnline.Data;
using SistemaOnline.ViewModels;

namespace SistemaOnline.Controllers
{
    [Authorize(Roles = "Cocina")]
    public class CocinaController : Controller
    {
        private readonly APPDBContext _dbcontext;
        public CocinaController(APPDBContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var pedidos = await _dbcontext.Pedidos
                .Include(p => p.Mesa_Restaurante)
                .Include(p => p.Pedido_Detalles)
                    .ThenInclude(pd => pd.Producto)
                .Where(p => p.Estado_Pedido == "Pendiente" || p.Estado_Pedido == "Preparando" || p.Estado_Pedido == "Listo")
                .OrderBy(p => p.ID_Pedido)
                .ToListAsync();

            var vm = new CocinaDashboardVM
            {
                Pendientes = pedidos.Where(p => p.Estado_Pedido == "Pendiente").ToList(),
                Preparando = pedidos.Where(p => p.Estado_Pedido == "Preparando").ToList(),
                Listos = pedidos.Where(p => p.Estado_Pedido == "Listo").ToList(),
                AlertasInventario = await _dbcontext.Inventarios.CountAsync(i => i.Cantidad_Stock <= i.Stock_Minimo)
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AvanzarEstado(int id, string nuevoEstado)
        {
            var pedido = await _dbcontext.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                pedido.Estado_Pedido = nuevoEstado;
                await _dbcontext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Ingredientes()
        {
            var inventario = await _dbcontext.Inventarios
                .Include(i => i.Ingrediente)
                    .ThenInclude(ing => ing.Categoria_Ingrediente)
                .OrderBy(i => i.Ingrediente.Nombre_Ingrediente)
                .ToListAsync();
            return View(inventario);
        }

        public async Task<IActionResult> Detalle()
        {
            var pedidos = await _dbcontext.Pedidos
                .Include(p => p.Mesa_Restaurante)
                .Include(p => p.Pedido_Detalles)
                    .ThenInclude(pd => pd.Producto)
                .OrderByDescending(p => p.ID_Pedido)
                .Take(20)
                .ToListAsync();
            return View(pedidos);
        }
    }
}
