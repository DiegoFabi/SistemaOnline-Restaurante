using System.ComponentModel.DataAnnotations;

namespace SistemaOnline.ViewModels
{
    public class CartaVM
    {
        public int ID_Carta { get; set; }

        [Required, MaxLength(100)]
        public string Nombre_Carta { get; set; }

        public int Cantidad_Platos { get; set; }

        [Required, MaxLength(100)]
        public string Descripcion { get; set; }

        public decimal Precio { get; set; }
    }
}
