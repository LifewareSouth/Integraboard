using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SQLite;
using WpfApp1.Model;
namespace WpfApp1
{
    internal class DatabaseInteraction
    {
        static DatabaseInteraction instance;

        public static DatabaseInteraction Instance
        {
            get
            {
                if (instance == null)
                    instance = new DatabaseInteraction();
                return instance;
            }
        }
        private static string rutadb = ConfigurationManager.ConnectionStrings["rutadb"].ConnectionString;
        public void CreacionDeTablas()
        {
            using (SQLiteConnection conexion = new SQLiteConnection(rutadb))
            {
                conexion.Open();
                string query = "CREATE TABLE IF NOT EXISTS names (id_name INTEGER NOT NULL, nombre TEXT, PRIMARY key( id_name AUTOINCREMENT)); ";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
        }
        public void GuardarNombre(string nombre)
        {
            using (SQLiteConnection conexion = new SQLiteConnection(rutadb))
            {
                conexion.Open();
                string query = "insert into names (nombre) values (@nombre)";

                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@nombre", nombre));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
        }
        public List<Nombre> GetNombres()
        {
            List < Nombre > ListaNombres = new List<Nombre>(); 
            using (SQLiteConnection conexion = new SQLiteConnection(rutadb))
            {
                conexion.Open();
                string query = "select id_name, nombre from names " +
                    "ORDER by id_name DESC";

                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ListaNombres.Add(new Nombre()
                        {
                            ID = int.Parse(dr["id_name"].ToString()),
                            Name = dr["nombre"].ToString(),
                        });
                    }
                }
                conexion.Close();
            }
            return ListaNombres;
        }

    }
}
