using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaOnline.Models
{
    public class Mesa_Restaurante
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_Mesa { get; set; }

        public int Numero_Mesa { get; set; }

        public int Capacidad { get; set; }

        [StringLength(100)]
        public string Ubicacion { get; set; }

        [StringLength(20)]
        public string Estado { get; set; }

        // una mesa recibe muchos pedidos y reservaciones
        public ICollection<Pedido> Pedidos { get; set; }
        public ICollection<Reservacion> Reservaciones { get; set; }
    }
}