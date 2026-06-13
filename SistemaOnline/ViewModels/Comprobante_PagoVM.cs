using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaOnline.ViewModels
{
    public class Comprobante_PagoVM
    {
        public int ID_Comprobante { get; set; }

        [Required, MaxLength(30)]
        public string Tipo_Comprobante { get; set; }

        [Required, MaxLength(10)]
        public string Numero_Comprobante { get; set; }

        [Required, MaxLength(10)]
        public string Serie { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha_Emision { get; set; }

        public decimal Sub_Total { get; set; }

        public decimal Monto_Total { get; set; }

        public decimal IGV { get; set; }

        [Required, MaxLength(30)]
        public string Estado_Comprobante { get; set; }

        [Required, MaxLength(30)]
        public string Metodo_Pago { get; set; }

        [Required, MaxLength(200)]
        public string Razon_Social { get; set; }

        [Required, MaxLength(11)]
        public string RUC { get; set; }

        [Required, MaxLength(200)]
        public string Direccion_Fiscal { get; set; }

        public int ID_Pedido { get; set; }

        // Para mostrar en Lista
        public string? PedidoInfo { get; set; }

        // Para los selects en Nuevo/Editar
        public List<SelectListItem> PedidosDisponibles { get; set; } = new();
    }
}
