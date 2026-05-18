namespace SistemaOnline.Models
{
    public class Empleado_Turno
    {
        // tabla intermedia para relacion muchos a muchos entre Empleado y Turno
        public int ID_Turno { get; set; }
        public Turno Turno { get; set; }

        public int ID_Empleado { get; set; }
        public Empleado Empleado { get; set; }
    }
}