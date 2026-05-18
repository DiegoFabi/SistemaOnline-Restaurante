using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace SistemaOnline.Models
{
    public class Empleado
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Empleado { get; set; }

        [StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string Apellidos { get; set; }

        [StringLength(100)]
        public string Direccion { get; set; }

        [StringLength(100)]
        public string Cargo { get; set; }

        [StringLength(9)]
        public string Telefono { get; set; }

        [StringLength(15)]
        public string Estado { get; set; }

        [StringLength(8)]
        public string DNI { get; set; }

        // Un empleado se vincula a muchos registros en estas tablas
        public ICollection<Usuario> Usuarios { get; set; }
        public ICollection<Contrato> Contratos { get; set; }
        public ICollection<Empleado_Turno> Empleado_Turnos { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
    }
}