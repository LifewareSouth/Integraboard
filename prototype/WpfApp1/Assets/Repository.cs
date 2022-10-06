using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using WpfApp1.Model;
using WpfApp1.Pages.Pictogramas;

namespace WpfApp1.Assets
{
    public class Repository
    {
        private static string SqliteConnection = ConfigurationManager.ConnectionStrings["SqliteConnection"].ConnectionString;
        private static readonly Repository instance = new Repository();
        public static Repository Instance
        {
            get
            {
                return instance;
            }
        }

        public static string PictogramCategoryToString(Pictogram.PictogramCategory pCategory)
        {
            switch (pCategory)
            {
                case (Pictogram.PictogramCategory.Educacion):
                    {
                        return "Educación";
                    }
                case (Pictogram.PictogramCategory.Comunicacion):
                    {
                        return "Comunicación";
                    }
                case (Pictogram.PictogramCategory.Verbos):
                    {
                        return "Verbos";
                    }
                case (Pictogram.PictogramCategory.Objetos):
                    {
                        return "Objetos";
                    }
                case (Pictogram.PictogramCategory.AVD):
                    {
                        return "AVD";
                    }
                case (Pictogram.PictogramCategory.Comidas):
                    {
                        return "Comidas";
                    }
                case (Pictogram.PictogramCategory.Lugares):
                    {
                        return "Lugares";
                    }
                case (Pictogram.PictogramCategory.Naturaleza):
                    {
                        return "Naturaleza";
                    }
                case (Pictogram.PictogramCategory.Otros):
                    {
                        return "Otros";
                    }

            }
            return "";
        }
        public void CreacionDeTablas()
        {
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "CREATE TABLE IF NOT EXISTS imagenes (idImagen INTEGER NOT NULL, nombreImagen TEXT,blobImagen BLOB ,alfaImagen TEXT, PRIMARY key( idImagen AUTOINCREMENT));" +
                    "CREATE TABLE IF NOT EXISTS pictogramas (idPict INTEGER NOT NULL,idAlfaPict TEXT, nombrePict TEXT, textoPict Text, categoriaPict TEXT, idImagen int, idSonido int, PRIMARY KEY(idPict AUTOINCREMENT));";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
        }
        public int GuardarImagen(string pathImagen)
        {
            byte[] pic = File.ReadAllBytes(pathImagen);
            string imageName = System.IO.Path.GetFileNameWithoutExtension(pathImagen);
            string alfaIdImagen = Guid.NewGuid().ToString();
            int idImagen = 0;
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                //AÑADE LA IMAGEN A LA BASE DE DATOS LOCAL
                conexion.Open();
                string query = "insert into imagenes(nombreImagen, blobImagen,alfaImagen) values (@nombreImagen, @blobImagen,@alfaImagen)";

                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                SQLiteParameter parametro = new SQLiteParameter("@blobImagen", System.Data.DbType.Binary);
                cmd.Parameters.Add(new SQLiteParameter("@nombreImagen", imageName));
                cmd.Parameters.Add(new SQLiteParameter("@alfaImagen", alfaIdImagen));
                parametro.Value = pic;
                cmd.Parameters.Add(parametro);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();

                conexion.Close();
                //BUSCA LA ID DE LA IMAGEN RECIEN AGREGADA
                query = "select idImagen from imagenes where alfaImagen = @alfaImagen";
                conexion.Open();
                cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@alfaImagen", alfaIdImagen));
                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        idImagen = int.Parse(dr["idImagen"].ToString());
                    }
                }
                conexion.Close();

            }
            return idImagen;
        }

        public void CrearPictograma(Pictogram pict)
        {
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                //AÑADE LA IMAGEN A LA BASE DE DATOS LOCAL
                conexion.Open();
                string query = "insert into pictogramas(idAlfaPict, nombrePict,textoPict,categoriaPict,idImagen,idSonido) values (@idAlfaPict, @nombrePict,@textoPict,@categoriaPict,@idImagen,@idSonido)";

                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idAlfaPict", Guid.NewGuid().ToString()));
                cmd.Parameters.Add(new SQLiteParameter("@nombrePict", pict.Nombre));
                cmd.Parameters.Add(new SQLiteParameter("@textoPict", pict.Texto));
                cmd.Parameters.Add(new SQLiteParameter("@categoriaPict", pict.Categoria));
                cmd.Parameters.Add(new SQLiteParameter("@idImagen", pict.idImagen));
                cmd.Parameters.Add(new SQLiteParameter("@idSonido", pict.idSonido));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();

                conexion.Close();

            }
        }
    }
}
