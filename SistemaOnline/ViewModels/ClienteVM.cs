using System.ComponentModel.DataAnnotations;

namespace SistemaOnline.ViewModels
{
    public class ClienteVM
    {
        public int ID_Cliente { get; set; }

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha_Nacimiento { get; set; }

        public string Direccion { get; set; }

        public string DNI { get; set; }

        public string RUC { get; set; }
    }
}
