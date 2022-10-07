using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
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
        public BitmapImage ImageFromBuffer(Byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }
        public void CreacionDeTablas()
        {
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "CREATE TABLE IF NOT EXISTS imagenes (idImagen INTEGER NOT NULL, idAlfaImagen TEXT, nombreImagen TEXT,blobImagen BLOB ,alfaImagen TEXT, PRIMARY key( idImagen AUTOINCREMENT));" +
                    "CREATE TABLE IF NOT EXISTS pictogramas (idPict INTEGER NOT NULL,idAlfaPict TEXT, nombrePict TEXT, textoPict Text, categoriaPict TEXT, idImagen int, idSonido int, PRIMARY KEY(idPict AUTOINCREMENT));" +
                    "CREATE TABLE IF NOT EXISTS etiquetas (idEtiqueta INTEGER NOT NULL, idAlfaEtiqueta TEXT, nombreEtiqueta TEXT, PRIMARY KEY(idEtiqueta AUTOINCREMENT));" +
                    "CREATE TABLE IF NOT EXISTS pictEtiqueta(IdPictEtiqueta INTEGER NOT NULL,idEtiqueta INTEGER,idPictograma INTEGER,PRIMARY KEY(IdPictEtiqueta AUTOINCREMENT));";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
        }
        public void GuardarImagen(string pathImagen)
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
                /*
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
                */
            }
            //return idImagen;
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
        public int getLatestPictogram()
        {
            int idPict = 0;
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                //OBTENER LA ULTIMA ID CREADA
                string query = "SELECT idPict FROM pictogramas ORDER by idPict DESC limit 1";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (int.Parse(dr["idPict"].ToString()) != 0)
                        {
                            idPict = int.Parse(dr["idPict"].ToString());
                        }
                    }
                }
                conexion.Close();
            }
            return idPict;
        }

        public List<ImagenModel> getAllImages()
        {
            List<ImagenModel> listaImagenes = new List<ImagenModel>();
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "select idImagen, idAlfaImagen, nombreImagen, blobImagen  from imagenes order by idImagen desc;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                
                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        listaImagenes.Add(new ImagenModel
                        {
                            ID = int.Parse(dr["idImagen"].ToString()),
                            idAlfaImagen = dr["nombreImagen"].ToString(),
                            Nombre = dr["nombreImagen"].ToString(),
                            Imagen = ImageFromBuffer((System.Byte[])dr["blobImagen"])
                        });

                        
                    }
                }
                conexion.Close();
            }
            return listaImagenes;
        }
        public List<Etiqueta> getAllEtiquetas()
        {
            List<Etiqueta> listaEtiquetas = new List<Etiqueta>();
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "select idEtiqueta, idAlfaEtiqueta, nombreEtiqueta from etiquetas;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                
                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        listaEtiquetas.Add(new Etiqueta
                        {
                            ID = int.Parse(dr["idEtiqueta"].ToString()),
                            idAlfaEtiqueta = dr["idAlfaEtiqueta"].ToString(),
                            NombreEtiqueta = dr["nombreEtiqueta"].ToString()
                        });

                        
                    }
                }
                conexion.Close();
            }

            return listaEtiquetas;
        }
        public void InsertEtiqueta(string NombreEtiqueta)
        {
            int IdEtiqueta = 0;
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                //AÑADE LA ETIQUETA A LA BASE DE DATOS LOCAL
                conexion.Open();
                string query = "insert into etiquetas(idAlfaEtiqueta, nombreEtiqueta) values (@idAlfaEtiqueta, @nombreEtiqueta)";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idAlfaEtiqueta", Guid.NewGuid().ToString()));
                cmd.Parameters.Add(new SQLiteParameter("@nombreEtiqueta", NombreEtiqueta));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
        }
        public int GetIdEtiqueta(string NombreEtiqueta)
        {
            int IdEtiqueta = 0;
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                //BUSCA LA ID DE LA ETIQUETA
                conexion.Open();
                string query = "select idEtiqueta from etiquetas where nombreEtiqueta = @nombreEtiqueta";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@nombreEtiqueta", NombreEtiqueta));
                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        IdEtiqueta = int.Parse(dr["idEtiqueta"].ToString());
                    }
                }
                conexion.Close();
            }
            return IdEtiqueta;
        }

        public void AsociarEtiquetasPict(List<int>ListaEtiquetas,int idPict)
        {
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                //ASOCIA LAS ETIQUETAS CON EL PICTOGRAMA
                foreach (int idEtiqueta in ListaEtiquetas)
                {
                    conexion.Open();
                    string query = "insert into pictEtiqueta( IdEtiqueta,IdPictograma) values (@IdEtiqueta, @IdPictograma)";
                    SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                    cmd.Parameters.Add(new SQLiteParameter("@IdEtiqueta", idEtiqueta));
                    cmd.Parameters.Add(new SQLiteParameter("@IdPictograma", idPict));
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();
                    conexion.Close();
                }
            }
        }
    }
    
}
