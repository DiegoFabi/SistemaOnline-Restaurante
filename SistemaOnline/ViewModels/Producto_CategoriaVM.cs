using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaOnline.ViewModels
{
    public class Producto_CategoriaVM
    {
        public int ID_Categoria { get; set; }

        [Required, MaxLength(50)]
        public string Nombre_Categoria { get; set; }

        [Required, MaxLength(200)]
        public string Descripcion { get; set; }

        public int ID_Carta { get; set; }

        // Para mostrar en Lista
        public string? CartaNombre { get; set; }

        // Para los selects en Nuevo/Editar
        public List<SelectListItem> CartasDisponibles { get; set; } = new();
    }
}
