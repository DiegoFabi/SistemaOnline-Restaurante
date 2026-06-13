using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaOnline.ViewModels
{
    public class ProductoVM
    {
        public int ID_Producto { get; set; }

        [Required, MaxLength(100)]
        public string Nombre_Plato { get; set; }

        [Required, MaxLength(255)]
        public string Descripcion { get; set; }

        public double Tiempo_Preparacion { get; set; }

        public decimal Precio { get; set; }

        public bool Disponibilidad { get; set; }

        [Required, MaxLength(50)]
        public string Categoria { get; set; }

        public int ID_Categoria { get; set; }

        // Para mostrar en Lista
        public string? CategoriaRelNombre { get; set; }

        // Para los selects en Nuevo/Editar
        public List<SelectListItem> CategoriasDisponibles { get; set; } = new();
    }
}
