using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaOnline.ViewModels
{
    public class IngredienteVM
    {
        public int ID_Ingrediente { get; set; }

        public string Nombre_Ingrediente { get; set; }

        public string Unidad_Medida { get; set; }

        public string Descripcion { get; set; }

        public decimal Costo_Unitario { get; set; }

        public bool Estado { get; set; }

        public int ID_Cat_Ingrediente { get; set; }

        // Para mostrar en Lista
        public string? CategoriaNombre { get; set; }

        // Para los selects en Nuevo/Editar
        public List<SelectListItem> CategoriasDisponibles { get; set; } = new();
    }
}
