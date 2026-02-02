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


        public List<Empleado> GetEmpleadosOficioSalario(string oficio, int salario)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<string>("OFICIO") == oficio
                           && datos.Field<int>("SALARIO") >= salario
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                List<Empleado> empleados = new List<Empleado>();
                foreach (var row in consulta)
                {
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
        }

        public ResumenEmpleados GetEmpleadosOficio(string oficio)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<string>("OFICIO") == oficio
                           select datos;
            //QUIERO ORDENAR EMPLEADOS POR SU SALRIO
            consulta = consulta.OrderBy(x => x.Field<int>("SALARIO"));
            if (consulta.Count() == 0)
            {
                ResumenEmpleados model = new ResumenEmpleados();
                model.Personas = 0;
                model.MaxSalario = 0;
                model.MediaSalarial = 0;
                model.Empleados = null;
                return model;
            }
            else
            {
                int personas = consulta.Count();
                int maximo = consulta.Max(x => x.Field<int>("SALARIO"));
                double media = consulta.Average(x => x.Field<int>("SALARIO"));
                List<Empleado> empleados = new List<Empleado>();
                foreach (var row in consulta)
                {
                    Empleado emp = new Empleado
                    {
                        IdEmpleado = row.Field<int>("EMP_NO"),
                        Apellido = row.Field<string>("APELLIDO"),
                        Oficio = row.Field<string>("OFICIO"),
                        Salario = row.Field<int>("SALARIO"),
                        IdDepartamento = row.Field<int>("DEPT_NO")
                    };
                    empleados.Add(emp);
                }
                ResumenEmpleados model = new ResumenEmpleados();
                model.Personas = personas;
                model.MaxSalario = maximo;
                model.MediaSalarial = media;
                model.Empleados = empleados;
                return model;
            }
        }

        public List<string> GetOficios()
        {
            var consulta = (from datos in this.tablaEmpleados.AsEnumerable()
                            select datos.Field<string>("OFICIO")).Distinct();
            List<string> oficios = consulta.ToList();
            return oficios;
        } 

    }
}
