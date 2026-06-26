using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaOnline.ViewModels
{
    public class ContratoVM
    {
        public int ID_Contrato { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha_Inicio { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha_Fin { get; set; }

        [Required, MaxLength(50)]
        public string Tipo_Contrato { get; set; }

        [Range(100.01, double.MaxValue, ErrorMessage = "El salario debe ser mayor a 100.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Salario { get; set; }

        [Required, MaxLength(500)]
        public string Clausula { get; set; }

        public int ID_Empleado { get; set; }

        public int ID_Proveedor { get; set; }

        // Para mostrar en Lista
        public string? EmpleadoNombre { get; set; }
        public string? ProveedorNombre { get; set; }

        // Para los selects en Nuevo/Editar
        public List<SelectListItem> EmpleadosDisponibles { get; set; } = new();
        public List<SelectListItem> ProveedoresDisponibles { get; set; } = new();
    }
}