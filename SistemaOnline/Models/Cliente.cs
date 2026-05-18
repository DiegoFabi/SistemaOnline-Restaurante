using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaOnline.Models
{
    public class Cliente
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Cliente { get; set; }

        [StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string Apellidos { get; set; }

        [StringLength(9)]
        public string Telefono { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha_Nacimiento { get; set; }

        [StringLength(150)]
        public string Direccion { get; set; }

        [StringLength(8)]
        public string DNI { get; set; }

        [StringLength(11)]
        public string RUC { get; set; }

        // un cliente puede tener muchos pedidos y reservaciones
        public ICollection<Pedido> Pedidos { get; set; }
        public ICollection<Reservacion> Reservaciones { get; set; }
    }
}