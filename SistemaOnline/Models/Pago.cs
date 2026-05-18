using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaOnline.Models
{
    public class Pago
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Pago { get; set; }

        public DateTime Fecha_Hora_Pago { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Monto { get; set; }

        [StringLength(50)]
        public string Metodo_Pago { get; set; }

        [StringLength(100)]
        public string Detalles_Tarjeta { get; set; }

        [StringLength(20)]
        public string Estado { get; set; }

        // fk y objeto de relacion para pedido
        public int ID_Pedido { get; set; }
        public Pedido Pedido { get; set; }
    }
}