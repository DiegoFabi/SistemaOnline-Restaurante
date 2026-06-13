using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaOnline.ViewModels
{
    public class InventarioVM
    {
        public int ID_Inventario { get; set; }

        public decimal Cantidad_Stock { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha_Ultima_Reposicion { get; set; }

        public decimal Stock_Minimo { get; set; }

        public decimal Stock_Maximo { get; set; }

        public int ID_Ingrediente { get; set; }

        // Para mostrar en Lista
        public string? IngredienteNombre { get; set; }

        // Para los selects en Nuevo/Editar
        public List<SelectListItem> IngredientesDisponibles { get; set; } = new();
    }
}
