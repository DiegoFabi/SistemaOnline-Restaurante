using System.ComponentModel.DataAnnotations;

namespace SistemaOnline.ViewModels
{
    public class PromocionVM
    {
        public int ID_Promocion { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; }

        [Required, MaxLength(300)]
        public string Descripcion { get; set; }

        public decimal Porcentaje_Descuento { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha_Inicio { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha_Fin { get; set; }

        public bool Estado { get; set; }
    }
}
