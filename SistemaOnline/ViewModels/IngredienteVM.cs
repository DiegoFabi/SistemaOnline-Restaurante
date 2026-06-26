using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaOnline.ViewModels
{
    public class IngredienteVM
    {
        public int ID_Ingrediente { get; set; }

        [Required, MaxLength(20)]
        public string Nombre_Ingrediente { get; set; }

        [Required, MaxLength(20)]
        public string Unidad_Medida { get; set; }

        [Required, MaxLength(100)]
        public string Descripcion { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "El costo unitario debe ser mayor o igual a 1.")]
        public decimal Costo_Unitario { get; set; }

        public bool Estado { get; set; }

        public int ID_Cat_Ingrediente { get; set; }

        // Para mostrar en Lista
        public string? CategoriaNombre { get; set; }

        // Para los selects en Nuevo/Editar
        public List<SelectListItem> CategoriasDisponibles { get; set; } = new();
    }
}