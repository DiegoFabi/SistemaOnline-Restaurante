using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaOnline.ViewModels
{
    public class PedidoVM
    {
        public int ID_Pedido { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        public string Estado_Pedido { get; set; }

        public string Detalle_Pedido { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Total { get; set; }

        public int ID_Empleado { get; set; }

        public int ID_Cliente { get; set; }

        public int ID_Mesa { get; set; }

        // Para mostrar en Lista
        public string? EmpleadoNombre { get; set; }
        public string? ClienteNombre { get; set; }
        public string? MesaNumero { get; set; }

        // Para los selects en Nuevo/Editar
        public List<SelectListItem> EmpleadosDisponibles { get; set; } = new();
        public List<SelectListItem> ClientesDisponibles { get; set; } = new();
        public List<SelectListItem> MesasDisponibles { get; set; } = new();
    }
}
