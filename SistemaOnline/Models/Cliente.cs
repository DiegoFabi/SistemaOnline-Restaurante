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

        [Required, MaxLength(50)]
        public string Nombre { get; set; }

        [Required, MaxLength(50)]
        public string Apellidos { get; set; }

        [MaxLength(9)]
        [RegularExpression(@"^[0-9+\-() ]*$", ErrorMessage = "El teléfono solo puede contener números y los símbolos + - ( ).")]
        public string? Telefono { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha_Nacimiento { get; set; }

        [Required, MaxLength(150)]
        public string Direccion { get; set; }

        [Required, MaxLength(8)]
        public string DNI { get; set; }

        [Required, MaxLength(11)]
        public string RUC { get; set; }

        // un cliente puede tener muchas reservaciones
        public ICollection<Reservacion> Reservaciones { get; set; }
    }
}