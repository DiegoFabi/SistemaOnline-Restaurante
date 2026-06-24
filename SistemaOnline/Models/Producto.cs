using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaOnline.Models
{
    public class Producto
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Producto { get; set; }

        [Required, MaxLength(100)]
        public string Nombre_Plato { get; set; }

        [Required, MaxLength(255)]
        public string Descripcion { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El tiempo de preparación no puede ser negativo.")]
        public double Tiempo_Preparacion { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El precio no puede ser negativo.")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Precio { get; set; }

        public bool Disponibilidad { get; set; }

        [Required, MaxLength(50)]
        public string Categoria { get; set; }

        // fk y relacion para producto_categoria
        public int ID_Categoria { get; set; }
        public Producto_Categoria Producto_Categoria { get; set; }

        // fk con las tablas dependientes e intermedias
        public ICollection<Producto_Ingrediente> Producto_Ingredientes { get; set; }
        public ICollection<Pedido_Detalle> Pedido_Detalles { get; set; }
        public ICollection<Producto_Promocion> Producto_Promociones { get; set; }
    }
}