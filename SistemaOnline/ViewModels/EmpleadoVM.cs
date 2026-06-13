using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaOnline.ViewModels
{
    public class EmpleadoVM
    {
        public int ID_Empleado { get; set; }

        [Required, MaxLength(50)]
        public string Nombre { get; set; }

        [Required, MaxLength(50)]
        public string Apellidos { get; set; }

        [Required, MaxLength(100)]
        public string Direccion { get; set; }

        [Required, MaxLength(100)]
        public string Cargo { get; set; }

        [Required, MaxLength(9)]
        public string Telefono { get; set; }

        [Required, MaxLength(15)]
        public string Estado { get; set; }

        [Required, MaxLength(8)]
        public string DNI { get; set; }

        public int? ID_Usuario { get; set; }

        // Para mostrar en Lista
        public string? UsuarioNombre { get; set; }

        // Para los selects en Nuevo/Editar
        public List<SelectListItem> UsuariosDisponibles { get; set; } = new();
    }
}
