namespace MvcCoreLinqToSql.Models
{
    public class ResumenEmpleados
    {
        public int Personas { get; set; }
        public int MaxSalario { get; set; }
        public double MediaSalarial { get; set; }
        public List<Empleado> Empleados { get; set; }
    }
}
