using Microsoft.Data.SqlClient;
using MvcCoreLinqToSql.Models;
using System.Data;

namespace MvcCoreLinqToSql.Repositories
{
    public class RepositoryEmpleados
    {
        //SOLO TENDREMOS UNA TABLA A NIVEL DE CLASE PARA NUESTRAS CONSULTAS
        private DataTable tablaEmpleados;

        public RepositoryEmpleados()
        {
            string connectionString = "Data Source=LOCALHOST\\DEVELOPER;Initial Catalog=HOSPITAL;User ID=SA;Encrypt=True;Trust Server Certificate=True";
            string sql = "select * from EMP";
            //CREAMOS EL ADAPTADOR PUENTE ENTRE SQL SERVER Y LINQ
            SqlDataAdapter ad = new SqlDataAdapter(sql, connectionString);
            this.tablaEmpleados = new DataTable();
            //TRAEMOS LOS DATOS PARA LINQ
            ad.Fill(this.tablaEmpleados);
        }

        //METODO PARA RECUPERAR TODOS LOS EMPLEADOS
        public List<Empleado> GetEmpleados()
        {
            //LAS CONSULTAS SE ALMACENAN EN GENÉRICOS
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           select datos;
            //AHORA MISMO TENEMOS DENTRO DE CONSULTA LA INFORMACION DE LOS EMPLEADOS
            //LOAS DATOS VIENEN EN FORMATO TABLA
            //CADA ELEMENTO DE UNA TABLA ES UNA FILA LLAMADA (DataRow)
            //DEBEMOS RECORRER LAS FILAS, EXTRAERLAS Y CONVERTIRLASA NUESTRO MODEL Empleado
            List<Empleado> empleados = new List<Empleado>();
            //RECORREMOS CADA FILA DE LAS CONSULTAS
            foreach(var row in consulta)
            {
                //PARA EXTRAER DATOS DE UN DataRow
                //DataRow.Field<tipodato>("COLUMNA")
                Empleado emp = new Empleado();
                emp.IdEmpleado = row.Field<int>("EMP_NO");
                emp.Apellido = row.Field<string>("APELLIDO");
                emp.Oficio = row.Field<string>("OFICIO");
                emp.Salario = row.Field<int>("SALARIO");
                emp.IdDepartamento = row.Field<int>("DEPT_NO");
                empleados.Add(emp);
            }
            return empleados;
        }

        public Empleado FindEmpleado(int id)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<int>("EMP_NO") == id
                           select datos;
            //NOSOTROS SABEMOS QUE ESTA CONSULTA DEVUELVE UNA FILA
            //PERO linq SIEMPRE DEVUELVE UN CONJUNTO
            //DENTRO DE ESTE CONJUNTO TENEMOS METODOS LAMBDA PARA HACER COSITAS
            //POR EJEMPLO, PODRIAMOS CONTAR, PODRIAMOS SABEL EL MAXIMO
            //O RECUPERAR EL PRIMER ELEMENTO DEL CONJUNTO
            var row = consulta.First();
            Empleado empleado = new Empleado();
            empleado.IdEmpleado = row.Field<int>("EMP_NO");
            empleado.Apellido = row.Field<string>("APELLIDO");
            empleado.Oficio = row.Field<string>("OFICIO");
            empleado.Salario = row.Field<int>("SALARIO");
            empleado.IdDepartamento = row.Field<int>("DEPT_NO");
            return empleado;
        }

    }
}
