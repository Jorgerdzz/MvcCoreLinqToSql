using Microsoft.Data.SqlClient;
using MvcCoreLinqToSql.Models;
using System.Data;

namespace MvcCoreLinqToSql.Repositories
{
    public class RepositoryEnfermos
    {
        private DataTable tablaEnfermos;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryEnfermos()
        {
            string connectionString = "Data Source=LOCALHOST\\DEVELOPER;Initial Catalog=HOSPITAL;User ID=SA;Encrypt=True;Trust Server Certificate=True";
            string sql = "select * from ENFERMO";
            SqlDataAdapter ad = new SqlDataAdapter(sql, connectionString);
            this.tablaEnfermos = new DataTable();
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            ad.Fill(tablaEnfermos);
        }

        public List<Enfermo> GetEnfermos()
        {
            var consulta = from datos in this.tablaEnfermos.AsEnumerable()
                           select datos;
            List<Enfermo> enfermos = new List<Enfermo>();
            foreach(var row in consulta)
            {
                Enfermo enf = new Enfermo
                {
                    Inscripcion = row.Field<string>("INSCRIPCION"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Direccion = row.Field<string>("DIRECCION"),
                    FechaNacimiento = row.Field<DateTime>("FECHA_NAC"),
                    Sexo = row.Field<string>("S"),
                    NumeroSeguridadSocial = row.Field<string>("NSS")
                };
                enfermos.Add(enf);
            }
            return enfermos;
        }

        public Enfermo FindEnfermo(string inscripcion)
        {
            var consulta = from datos in this.tablaEnfermos.AsEnumerable()
                           where datos.Field<string>("INSCRIPCION") == inscripcion
                           select datos;
            var row = consulta.First();
            Enfermo enf = new Enfermo
            {
                Inscripcion = row.Field<string>("INSCRIPCION"),
                Apellido = row.Field<string>("APELLIDO"),
                Direccion = row.Field<string>("DIRECCION"),
                FechaNacimiento = row.Field<DateTime>("FECHA_NAC"),
                Sexo = row.Field<string>("S"),
                NumeroSeguridadSocial = row.Field<string>("NSS")
            };
            return enf;
        }

        public async Task DeleteEnfermo(string inscripcion)
        {
            string sql = "delete from ENFERMO where INSCRIPCION=@inscripcion";
            this.com.Parameters.AddWithValue("@inscripcion", inscripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

    }
}
