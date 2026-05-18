using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace SistemaOnline.Models
{
    public class Proveedor
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Proveedor { get; set; }

        [StringLength(200)]
        public string Nombre_Empresa { get; set; }

        [StringLength(11)]
        public string RUC { get; set; }

        [StringLength(100)]
        public string Email_Contacto { get; set; }

        [StringLength(9)]
        public string Telefono { get; set; }

        [StringLength(250)]
        public string Direccion { get; set; }

        [StringLength(200)]
        public string Tipo_Suministro { get; set; }

        [StringLength(50)]
        public string Estado { get; set; }

        // Un proveedor tiene muchos contratos y proveen diferentes tipos de productos (ingredientes)
        public ICollection<Contrato> Contratos { get; set; }
        public ICollection<Proveedor_Ingrediente> Proveedor_Ingredientes { get; set; }
    }
}