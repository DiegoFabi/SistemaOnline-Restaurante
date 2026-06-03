using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaOnline.Models
{
    public class Carta
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_Carta { get; set; }

        [Required, MaxLength(100)]
        public string Nombre_Carta { get; set; }

        public int Cantidad_Platos { get; set; }

        [Required, MaxLength(100)]
        public string Descripcion { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal Precio { get; set; }

        // una carta tiene muchas categorias de productos
        public ICollection<Producto_Categoria> Producto_Categorias { get; set; }
    }
}