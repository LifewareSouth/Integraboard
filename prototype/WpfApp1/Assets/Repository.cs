﻿using AForge.Imaging.Filters;
using NAudio.Lame;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using WpfApp1.Assets.Models;
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
        public string DIRECTORY_SONIDOS = "C:\\IntegraBoard\\repo\\sonidos";
        public BitmapImage getImageFromResources(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
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
        public SolidColorBrush categoryColor(string categoria)
        {
            SolidColorBrush col = (SolidColorBrush)new BrushConverter().ConvertFrom("#000000");
            switch (categoria)
            {
                case "Educación":
                    {
                        
                        col = (SolidColorBrush)new BrushConverter().ConvertFrom("#F44336");
                        return col;
                    }
                case "Comunicación":
                    {
                        col = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFEB3B");
                        return col;
                    }
                case "Verbos":
                    {
                        col = (SolidColorBrush)new BrushConverter().ConvertFrom("#2196F3");
                        return col;
                    }
                case "Objetos":
                    {
                        col = (SolidColorBrush)new BrushConverter().ConvertFrom("#9E9E9E");
                        return col;
                    }
                case "AVD":
                    {
                        col = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF9800");
                        return col;
                    }
                case "Comidas":
                    {
                        col = (SolidColorBrush)new BrushConverter().ConvertFrom("#795548");
                        return col;
                    }
                case "Lugares":
                    {
                        col = (SolidColorBrush)new BrushConverter().ConvertFrom("#9C27B0");
                        return col;
                    }
                case "Naturaleza":
                    {
                        col = (SolidColorBrush)new BrushConverter().ConvertFrom("#4CAF50");
                        return col;
                    }
                case "Otros":
                    {
                        col = (SolidColorBrush)new BrushConverter().ConvertFrom("#000000");
                        return col;
                    }
            }
            return col;
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
                string query = "CREATE TABLE IF NOT EXISTS imagenes (idImagen INTEGER NOT NULL, idAlfaImagen TEXT, nombreImagen TEXT,blobImagen BLOB , PRIMARY key( idImagen AUTOINCREMENT));" +
                    "CREATE TABLE IF NOT EXISTS sonidos (idSonido INTEGER NOT NULL, idAlfaSonido TEXT, nombreSonido TEXT,pathSonido TEXT, PRIMARY key(idSonido AUTOINCREMENT));" +
                    "CREATE TABLE IF NOT EXISTS pictogramas (idPict INTEGER NOT NULL,idAlfaPict TEXT, nombrePict TEXT, textoPict Text, categoriaPict TEXT, idImagen int, idSonido int, PRIMARY KEY(idPict AUTOINCREMENT));" +
                    "CREATE TABLE IF NOT EXISTS etiquetas (idEtiqueta INTEGER NOT NULL, idAlfaEtiqueta TEXT, nombreEtiqueta TEXT, PRIMARY KEY(idEtiqueta AUTOINCREMENT));" +
                    "CREATE TABLE IF NOT EXISTS pictEtiqueta(IdPictEtiqueta INTEGER NOT NULL,idEtiqueta INTEGER,idPictograma INTEGER,PRIMARY KEY(IdPictEtiqueta AUTOINCREMENT));" +
                    "CREATE TABLE IF NOT EXISTS etiquetasT (idEtiqueta INTEGER NOT NULL, idAlfaEtiqueta TEXT, nombreEtiqueta TEXT, PRIMARY KEY(idEtiqueta AUTOINCREMENT));" +
                    "CREATE TABLE IF NOT EXISTS tableroEtiqueta(IdTableroEtiqueta INTEGER NOT NULL,idEtiqueta INTEGER,idTablero INTEGER,PRIMARY KEY(IdTableroEtiqueta AUTOINCREMENT));" +
                    "CREATE TABLE IF NOT EXISTS tableros(idTablero INTEGER NOT NULL,idAlfaTablero Text,nombreTablero TEXT,tipo TEXT,filas INTEGER,columnas INTEGER,pictPortada INTEGER,asignacion TEXT,conTiempo TEXT,PRIMARY KEY(idTablero AUTOINCREMENT));" +
                    "CREATE TABLE IF NOT EXISTS pictTableros(idPictTablero INTEGER NOT NULL, idTablero INTEGER, idPictograma INTEGER,x INTEGER,y INTEGER,tiempo TEXTO,PRIMARY KEY(idPictTablero AUTOINCREMENT));" +
                    "CREATE TABLE IF NOT EXISTS perfil(idPerfil INTEGER NOT NULL, idAlfaPerfil TEXT, tipoPerfil TEXT, nombrePerfil TEXT, edad INTEGER,tamaño TEXT,fotoPerfil BLOB,voz TEXT, PRIMARY KEY(idPerfil AUTOINCREMENT));";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
        }
        public bool verificarPerfil()
        {
            bool existe = true;
            string currentDirectory = Directory.GetCurrentDirectory();

            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "SELECT EXISTS(SELECT 1 FROM perfil ) as existe;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if (int.Parse(dr["existe"].ToString()) == 0)
                        {
                            existe = false;
                        }
                    }
                }
                conexion.Close();
            }
            return existe;
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
                string query = "insert into imagenes(idAlfaImagen, nombreImagen, blobImagen) values (@idAlfaImagen, @nombreImagen, @blobImagen)";

                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                SQLiteParameter parametro = new SQLiteParameter("@blobImagen", System.Data.DbType.Binary);
                cmd.Parameters.Add(new SQLiteParameter("@nombreImagen", imageName));
                cmd.Parameters.Add(new SQLiteParameter("@idAlfaImagen", alfaIdImagen));
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
        public int GuardarImagenCamara(string pathImagen)
        {
            byte[] pic = File.ReadAllBytes(pathImagen);
            int numeroImagenCamera = 0;
            string imageName = System.IO.Path.GetFileNameWithoutExtension(pathImagen);
            string alfaIdImagen = Guid.NewGuid().ToString();
            int idImagen = 0;
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "SELECT count(nombreImagen) as total from imagenes where nombreImagen  like 'cameraPhoto_%'";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);

                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int total = int.Parse(dr["total"].ToString());
                        numeroImagenCamera = total + 1;
                    }
                }
                conexion.Close();
                //AÑADE LA IMAGEN A LA BASE DE DATOS LOCAL
                conexion.Open();
                query = "insert into imagenes(idAlfaImagen, nombreImagen, blobImagen) values (@idAlfaImagen, @nombreImagen, @blobImagen)";

                cmd = new SQLiteCommand(query, conexion);
                SQLiteParameter parametro = new SQLiteParameter("@blobImagen", System.Data.DbType.Binary);
                cmd.Parameters.Add(new SQLiteParameter("@nombreImagen", "cameraPhoto_" + numeroImagenCamera));
                cmd.Parameters.Add(new SQLiteParameter("@idAlfaImagen", alfaIdImagen));
                parametro.Value = pic;
                cmd.Parameters.Add(parametro);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();

                conexion.Close();

                //BUSCA LA ID DE LA IMAGEN RECIEN AGREGADA
                query = "select idImagen from imagenes where idAlfaImagen = @idAlfaImagen";
                conexion.Open();
                cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idAlfaImagen", alfaIdImagen));
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
        public Pictogram getOnePictogram(int ID)
        {
            Pictogram pict = new Pictogram();
            int sound_id;
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                //OBTENER LA ULTIMA ID CREADA
                string query = "SELECT idPict,idAlfaPict, nombrePict,textoPict,categoriaPict, p.idImagen,nombreImagen,blobImagen, p.idSonido,nombreSonido,pathSonido " +
                    "from pictogramas p " +
                    "JOIN imagenes i on p.idImagen = i.idImagen  " +
                    "LEFT join sonidos s on p.idSonido = s.idSonido " +
                    "where idPict = @idPict;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idPict", ID));
                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int.TryParse(dr["idSonido"].ToString(), out sound_id);
                        pict.ID = int.Parse(dr["idPict"].ToString());
                        pict.idAlfaPict = dr["idAlfaPict"].ToString();
                        pict.Nombre = dr["nombrePict"].ToString();
                        pict.Texto = dr["textoPict"].ToString();
                        pict.Categoria = dr["categoriaPict"].ToString();
                        pict.idImagen = int.Parse(dr["idImagen"].ToString());
                        pict.nombreImagen = dr["nombreImagen"].ToString();
                        pict.Imagen = ImageFromBuffer((System.Byte[])dr["blobImagen"]);
                        pict.idSonido = sound_id;
                        pict.nombreSonido = dr["nombreSonido"].ToString();
                        pict.pathSonido = dr["pathSonido"].ToString();
                        pict.ListaEtiquetas = GetEtiquetasFromPict(int.Parse(dr["idPict"].ToString()));
                        pict.colorBorde = categoryColor(dr["categoriaPict"].ToString());
                        
                    }
                }
                conexion.Close();
            }
            return pict;
        }
        public void EditPictogram(Pictogram pict)
        {
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "UPDATE pictogramas set nombrePict = @nombrePict , textoPict = @textoPict, categoriaPict = @categoriaPict, idImagen = @idImagen, idSonido = @idSonido where idPict= @idPict;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@nombrePict", pict.Nombre));
                cmd.Parameters.Add(new SQLiteParameter("@textoPict", pict.Texto));
                cmd.Parameters.Add(new SQLiteParameter("@categoriaPict", pict.Categoria));
                cmd.Parameters.Add(new SQLiteParameter("@idImagen", pict.idImagen));
                cmd.Parameters.Add(new SQLiteParameter("@idSonido", pict.idSonido));
                cmd.Parameters.Add(new SQLiteParameter("@idPict", pict.ID));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
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
                            idAlfaImagen = dr["idAlfaImagen"].ToString(),
                            Nombre = dr["nombreImagen"].ToString(),
                            Imagen = ImageFromBuffer((System.Byte[])dr["blobImagen"])
                        });

                        
                    }
                }
                conexion.Close();
            }
            return listaImagenes;
        }
        public List<EtiquetaP> getAllEtiquetas()
        {
            List<EtiquetaP> listaEtiquetas = new List<EtiquetaP>();
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
                        listaEtiquetas.Add(new EtiquetaP
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
        /// <summary>
        /// Crea una etiqueta nueva de pictograma
        /// </summary>
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

        public void AsociarEtiquetasPict(List<int>ListaEtiquetas,int idPict, bool isNew)
        {

            List<int> listaEtiquetasAsociadas = new List<int>();
            List<int> listaEtiquetasEliminadas = new List<int>();
            if (!isNew)//EN CASO DE EDITAR LAS ETIQUETAS DE UN PICTOGRAMA
            {
                listaEtiquetasAsociadas = getEtiquetasAsociadas(idPict);
            }
            //ASOCIA LAS ETIQUETAS CON EL PICTOGRAMA
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                    foreach (int idEtiqueta in ListaEtiquetas)
                {
                    bool existe = false;
                    if (!isNew)
                    {
                        if (listaEtiquetasAsociadas.Contains(idEtiqueta))
                        {
                            existe = true;
                        }
                    }
                    if (!existe)
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
            if (!isNew)
                {
                    foreach(int idTag in listaEtiquetasAsociadas)
                    {
                        if (!ListaEtiquetas.Contains(idTag))
                        {
                            deleteAsociacionEtiquetasPict(idPict,idTag);
                        }
                    }
                }
                

            
        }
        public List<int> getEtiquetasAsociadas(int idPict)
        {
            List<int> listaEtiquetasAsociadas = new List<int>();
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "select idEtiqueta from pictEtiqueta where idPictograma = @idPictograma; ";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@IdPictograma", idPict));
                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        listaEtiquetasAsociadas.Add(int.Parse(dr["idEtiqueta"].ToString()));
                    }
                }
                conexion.Close();
            }
            return listaEtiquetasAsociadas;
        }
        public void deleteAsociacionEtiquetasPict(int idPict,int idTag)
        {
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "Delete from pictEtiqueta where IdEtiqueta  = @IdEtiqueta and IdPictograma = @IdPictograma ;" +
                    "VACUUM;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@IdEtiqueta", idTag));
                cmd.Parameters.Add(new SQLiteParameter("@IdPictograma", idPict));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
        }
        public void CrearSonido(string pathSonido,string nombreSonido,bool isVoice)
        {

            if (!Directory.Exists(DIRECTORY_SONIDOS)){
                Directory.CreateDirectory(DIRECTORY_SONIDOS);
            }

            string alfaSound = Guid.NewGuid().ToString();
            string pathSonidoMp3 = DIRECTORY_SONIDOS + "\\" + alfaSound + ".mp3";
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                //AÑADE LA IMAGEN A LA BASE DE DATOS LOCAL
                conexion.Open();
                string query = "insert into sonidos(idAlfaSonido,nombreSonido,pathSonido) values (@idAlfaSonido,@nombreSonido,@pathSonido)";

                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idAlfaSonido", alfaSound));
                cmd.Parameters.Add(new SQLiteParameter("@nombreSonido", nombreSonido));
                cmd.Parameters.Add(new SQLiteParameter("@pathSonido", pathSonidoMp3));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
            if (isVoice)
            {
                FileStream fs;
                byte[] rawData = ConvertToMp3(pathSonido);
                fs = new FileStream(pathSonidoMp3, FileMode.OpenOrCreate, FileAccess.Write);
                fs.Write(rawData, 0, rawData.Length);
                fs.Close();
            }
            else
            {
                File.Copy(pathSonido, pathSonidoMp3, true);
            }
        }
        public int getVoiceNumber()
        {
            int numeroVoice = 0;
            List<EtiquetaP> listaEtiquetas = new List<EtiquetaP>();
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "SELECT count(nombreSonido) as total from sonidos where nombreSonido  like 'voice%'";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);

                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int total = int.Parse(dr["total"].ToString());
                        numeroVoice = total + 1;
                    }
                }
                conexion.Close();
            }
            return numeroVoice;
        }
        public List<SoundModel> GetAllSounds()
        {
            List<SoundModel> ListaSonidos = new List<SoundModel>();
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "select idSonido, idAlfaSonido,nombreSonido,pathSonido  from sonidos order by idSonido desc;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);

                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ListaSonidos.Add(new SoundModel
                        {
                            ID = int.Parse(dr["idSonido"].ToString()),
                            idAlfaSonido = dr["idAlfaSonido"].ToString(),
                            Nombre = dr["nombreSonido"].ToString(),
                            pathSonido = dr["pathSonido"].ToString(),
                        });
                    }
                }
                conexion.Close();
            }
            return ListaSonidos;
        }
        public byte[] ConvertToMp3(string pathWav)
        {
            using (var client = new WebClient())
            {
                var file = client.DownloadData(pathWav);
                var target = new WaveFormat(44100, 1);
                using (var outPutStream = new MemoryStream())
                using (var waveStream = new WaveFileReader(new MemoryStream(file)))
                using (var conversionStream = new WaveFormatConversionStream(target, waveStream))
                using (var writer = new LameMP3FileWriter(outPutStream, conversionStream.WaveFormat, 64, null))
                {
                    conversionStream.CopyTo(writer);

                    return outPutStream.ToArray();
                }
            }
        }

        public List<Pictogram> getAllPict()
        {
            List<Pictogram> listaPict = new List<Pictogram>();
            int sound_id;
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "SELECT idPict,idAlfaPict, nombrePict,textoPict,categoriaPict, p.idImagen,nombreImagen,blobImagen, p.idSonido,nombreSonido,pathSonido " +
                "from pictogramas p " +
                "JOIN imagenes i on p.idImagen = i.idImagen  " +
                "LEFT join sonidos s on p.idSonido = s.idSonido order by idPict DESC;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);

                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int.TryParse(dr["idSonido"].ToString(), out sound_id);
                        listaPict.Add(new Pictogram
                        {
                            ID = int.Parse(dr["idPict"].ToString()),
                            idAlfaPict = dr["idAlfaPict"].ToString(),
                            Nombre = dr["nombrePict"].ToString(),
                            Texto = dr["textoPict"].ToString(),
                            Categoria = dr["categoriaPict"].ToString(),
                            idImagen = int.Parse(dr["idImagen"].ToString()),
                            nombreImagen = dr["nombreImagen"].ToString(),
                            Imagen = ImageFromBuffer((System.Byte[])dr["blobImagen"]),
                            idSonido = sound_id,
                            nombreSonido = dr["nombreSonido"].ToString(),
                            pathSonido = dr["pathSonido"].ToString(),
                            ListaEtiquetas = GetEtiquetasFromPict(int.Parse(dr["idPict"].ToString())),
                            colorBorde = categoryColor(dr["categoriaPict"].ToString())
                        });
                    }
                }
                conexion.Close();
            }
            return listaPict;

        }
        public int GetIdEtiquetaT(string NombreEtiqueta)
        {
            int IdEtiqueta = 0;
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                //BUSCA LA ID DE LA ETIQUETA
                conexion.Open();
                string query = "select idEtiqueta from etiquetasT where nombreEtiqueta = @nombreEtiqueta";
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
        public List<EtiquetaP> GetEtiquetasFromPict(int idPictograma)
        {
            List<EtiquetaP> listaEtiquetas = new List<EtiquetaP>();
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "select p.idEtiqueta,idAlfaEtiqueta,nombreEtiqueta " +
                    "from pictEtiqueta p " +
                    "join etiquetas e on e.idEtiqueta = p.idEtiqueta " +
                    "where idPictograma = @idPictograma ;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idPictograma", idPictograma));
                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        listaEtiquetas.Add(new EtiquetaP
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
        public void deletePictograma(int idPict)
        {
            //QUITA LA ASOCIACION DEL PICTOGRAMA CON LAS ETIQUETAS
            List<int> ListaEtiquetasAsociadas = getEtiquetasAsociadas(idPict);
            foreach(int idTag in ListaEtiquetasAsociadas)
            {
                deleteAsociacionEtiquetasPict(idPict, idTag);
            }
            //ELIMINA EL PICTOGRAMA DE LA BASE DE DATOS
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "Delete from pictTableros where idPictograma  = @idPict;" +
                    "update tableros  set pictPortada = 0,asignacion = 'No Asignado' where pictPortada = @idPict;" +
                    "Delete from pictogramas where idPict  = @idPict;" +
                    "VACUUM;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idPict", idPict));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
        }

        public List<pictTablero> getPictTableros(int idTablero)
        {
            List<pictTablero> listaPictTableros = new List<pictTablero>();
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "select idPictTablero, idTablero, idPictograma, x, y,tiempo from pictTableros where idTablero = @idTablero";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        listaPictTableros.Add(new pictTablero
                        {
                            ID = int.Parse(dr["idPictTablero"].ToString()),
                            idTablero = int.Parse(dr["idTablero"].ToString()),
                            idPictograma = int.Parse(dr["idPictograma"].ToString()),
                            pictograma = getOnePictogram(int.Parse(dr["idPictograma"].ToString())),
                            x = int.Parse(dr["x"].ToString()),
                            y = int.Parse(dr["y"].ToString()),
                            tiempo = dr["tiempo"].ToString()
                        });
                    }
                }
                conexion.Close();
            }

            return listaPictTableros;
        }
        public List<Board> getAllBoards()
        {
            List<Board> listaTableros = new List<Board>();
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "select idTablero, idAlfaTablero, nombreTablero, tipo,filas,columnas,pictPortada,asignacion, conTiempo from tableros;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        List<etiquetaT> listaEtiquetas = GetEtiquetasFromBoard(int.Parse(dr["idTablero"].ToString()));
                        string etiquetasJuntas ="";
                        foreach(etiquetaT etiquetaT in listaEtiquetas)
                        {
                            if(etiquetaT == listaEtiquetas.First())
                            {
                                etiquetasJuntas = etiquetaT.NombreEtiqueta;
                            }
                            else
                            {
                                etiquetasJuntas = etiquetasJuntas + ", " + etiquetaT.NombreEtiqueta;
                            }
                            
                        }
                        Pictogram pPortada = new Pictogram();
                        if (int.Parse(dr["pictPortada"].ToString()) != 0)
                        {
                            pPortada = getOnePictogram(int.Parse(dr["pictPortada"].ToString()));
                        }
                        else
                        {
                            pPortada = defaultPict();
                        }
                        listaTableros.Add(new Board
                        {
                            ID = int.Parse(dr["idTablero"].ToString()),
                            idAlfaTablero = dr["idAlfaTablero"].ToString(),
                            nombreTablero = dr["nombreTablero"].ToString(),
                            tipo = dr["tipo"].ToString(),
                            filas = int.Parse(dr["filas"].ToString()),
                            columnas = int.Parse(dr["columnas"].ToString()),
                            idPictPortada = int.Parse(dr["pictPortada"].ToString()),
                            pictPortada = pPortada,
                            pictTableros = getPictTableros(int.Parse(dr["idTablero"].ToString())),
                            ListaEtiquetasTableros = GetEtiquetasFromBoard(int.Parse(dr["idTablero"].ToString())),
                            EtiquetasJuntas=etiquetasJuntas,
                            asignacion = dr["asignacion"].ToString(),
                            conTiempo = dr["conTiempo"].ToString(),
                        });
                    }
                }
                conexion.Close();
            }

            return listaTableros;
        }
        public int crearTablero(Board board)
        {
            string alfaIdBoard = Guid.NewGuid().ToString();
            int idBoard = 0;
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                //AÑADE EL TABLERO A LA BASE DE DATOS LOCAL
                conexion.Open();
                string query = "insert into tableros( idAlfaTablero, nombreTablero, tipo,filas,columnas,pictPortada,asignacion,conTiempo) " +
                    "values (@idAlfaTablero, @nombreTablero, @tipo,@filas,@columnas,@pictPortada,@asignacion,@conTiempo)";

                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idAlfaTablero", alfaIdBoard));
                cmd.Parameters.Add(new SQLiteParameter("@nombreTablero", board.nombreTablero));
                cmd.Parameters.Add(new SQLiteParameter("@tipo", board.tipo));
                cmd.Parameters.Add(new SQLiteParameter("@filas", board.filas));
                cmd.Parameters.Add(new SQLiteParameter("@columnas", board.columnas));
                cmd.Parameters.Add(new SQLiteParameter("@pictPortada", board.idPictPortada));
                cmd.Parameters.Add(new SQLiteParameter("@asignacion", "No Asignado"));
                cmd.Parameters.Add(new SQLiteParameter("@conTiempo", board.conTiempo));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();

                conexion.Close();

                //BUSCA LA ID DEL TABLERO RECIEN AGREGADO
                query = "select idTablero from tableros where idAlfaTablero = @idAlfaTablero";
                conexion.Open();
                cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idAlfaTablero", alfaIdBoard));
                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        idBoard = int.Parse(dr["idTablero"].ToString());
                    }
                }
                conexion.Close();

            }
            return idBoard;
        }
        public void EnlazarPictBoard(int idTablero, int idPict, int x, int y,string tiempo)
        {
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                //ENLAZA LOS PICTOGRAMAS DEL TABLERO EN LA BD LOCAL
                conexion.Open();
                string query = "insert into pictTableros( idTablero, idPictograma, x,y,tiempo) " +
                    "values ( @idTablero, @idPictograma, @x,@y,@tiempo)";

                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
                cmd.Parameters.Add(new SQLiteParameter("@idPictograma", idPict));
                cmd.Parameters.Add(new SQLiteParameter("@x", x));
                cmd.Parameters.Add(new SQLiteParameter("@y", y));
                cmd.Parameters.Add(new SQLiteParameter("@tiempo", tiempo));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();

                conexion.Close();

            }
        }
        /// <summary>
        /// Obtiene todas las etiquetas de tableros
        /// </summary>
        public List<etiquetaT> getAllEtiquetasTableros()
        {
            List<etiquetaT> listaEtiquetas = new List<etiquetaT>();
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "select idEtiqueta, idAlfaEtiqueta, nombreEtiqueta from etiquetasT;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        listaEtiquetas.Add(new etiquetaT
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
        public List<etiquetaT> GetEtiquetasFromBoard(int idTablero)
        {
            List<etiquetaT> listaEtiquetas = new List<etiquetaT>();
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "select t.idEtiqueta,idAlfaEtiqueta,nombreEtiqueta " +
                    "from tableroEtiqueta t " +
                    "join etiquetasT e on e.idEtiqueta = t.idEtiqueta " +
                    "where idTablero = @idTablero ;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        listaEtiquetas.Add(new etiquetaT
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
        /// <summary>
        /// Obtiene todas las etiquetas un tablero especifico
        /// </summary>
        public List<int> getEtiquetasAsociadasTablero(int idBoard)
        {
            List<int> listaEtiquetasAsociadas = new List<int>();
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "select idEtiqueta from tableroEtiqueta where idTablero = @idTablero; ";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idTablero", idBoard));
                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        listaEtiquetasAsociadas.Add(int.Parse(dr["idEtiqueta"].ToString()));
                    }
                }
                conexion.Close();
            }
            return listaEtiquetasAsociadas;
        }
        public void AsociarEtiquetasTablero(List<int> ListaEtiquetas, int idTablero, bool isNew)
        {
            
            List<int> listaEtiquetasAsociadas = new List<int>();
            List<int> listaEtiquetasEliminadas = new List<int>();
            if (!isNew)//EN CASO DE EDITAR LAS ETIQUETAS DE UN TABLERO
            {
                listaEtiquetasAsociadas = getEtiquetasAsociadasTablero(idTablero);
            }
            //ASOCIA LAS ETIQUETAS CON EL PICTOGRAMA
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                foreach (int idEtiqueta in ListaEtiquetas)
                {
                    bool existe = false;
                    if (!isNew)
                    {
                        if (listaEtiquetasAsociadas.Contains(idEtiqueta))
                        {
                            existe = true;
                        }
                    }
                    if (!existe)
                    {
                        conexion.Open();
                        string query = "insert into tableroEtiqueta( idEtiqueta,idTablero) values (@IdEtiqueta, @idTablero)";
                        SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                        cmd.Parameters.Add(new SQLiteParameter("@IdEtiqueta", idEtiqueta));
                        cmd.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.ExecuteNonQuery();
                        conexion.Close();
                    }
                }
            }
            if (!isNew)
            {
                //ELIMINA LA ASOCIACION DE LA ETIQUETA CON EL TABLERO 
                foreach (int idTag in listaEtiquetasAsociadas)
                {
                    if (!ListaEtiquetas.Contains(idTag))
                    {
                        deleteAsociacionEtiquetasTablero(idTablero, idTag);
                    }
                }
            }
        }
        public void deleteAsociacionEtiquetasTablero(int idTablero, int idTag)
        {
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "Delete from tableroEtiqueta where IdEtiqueta  = @IdEtiqueta and idTablero = @idTablero ;" +
                    "VACUUM;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@IdEtiqueta", idTag));
                cmd.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
        }
        /// <summary>
        /// Crea una etiqueta nueva de tablero
        /// </summary>
        public void InsertEtiquetaT(string NombreEtiqueta)
        {
            int IdEtiqueta = 0;
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                //AÑADE LA ETIQUETA A LA BASE DE DATOS LOCAL
                conexion.Open();
                string query = "insert into etiquetasT(idAlfaEtiqueta, nombreEtiqueta) values (@idAlfaEtiqueta, @nombreEtiqueta)";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idAlfaEtiqueta", Guid.NewGuid().ToString()));
                cmd.Parameters.Add(new SQLiteParameter("@nombreEtiqueta", NombreEtiqueta));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
        }
        public void updateTablero(Board board)
        {
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                //idAlfaTablero, nombreTablero, tipo,filas,columnas,pictPortada,asignacion
                string query = "UPDATE tableros set nombreTablero = @nombreTablero , tipo = @tipo, filas = @filas, columnas = @columnas, pictPortada = @pictPortada,conTiempo = @conTiempo where idTablero= @idTablero;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idTablero", board.ID));
                cmd.Parameters.Add(new SQLiteParameter("@nombreTablero", board.nombreTablero));
                cmd.Parameters.Add(new SQLiteParameter("@tipo", board.tipo));
                cmd.Parameters.Add(new SQLiteParameter("@filas", board.filas));
                cmd.Parameters.Add(new SQLiteParameter("@columnas", board.columnas));
                cmd.Parameters.Add(new SQLiteParameter("@pictPortada", board.idPictPortada));
                cmd.Parameters.Add(new SQLiteParameter("@conTiempo", board.conTiempo));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
        }
        public void updatePictTablero(int idTablero, int idPictograma, int posX, int posY,string tiempo)
        {
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "UPDATE pictTableros set x = @x , y = @y , tiempo = @tiempo   where idTablero= @idTablero and idPictograma =@idPictograma ;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
                cmd.Parameters.Add(new SQLiteParameter("@idPictograma", idPictograma));
                cmd.Parameters.Add(new SQLiteParameter("@x", posX));
                cmd.Parameters.Add(new SQLiteParameter("@y", posY));
                cmd.Parameters.Add(new SQLiteParameter("@tiempo", tiempo));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
        }
        public void quitarAsociacionPictTablero(int idTablero, int idPictograma)
        {
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "Delete from pictTableros where idTablero= @idTablero and idPictograma =@idPictograma ;" +
                    "VACUUM;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
                cmd.Parameters.Add(new SQLiteParameter("@idPictograma", idPictograma));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
        }
        public void asignacionTablero(bool asignar, int idTablero)
        {
            string asignado = "";
            if (asignar)
            {
                asignado = "Asignado";
            }
            else
            {
                asignado = "No Asignado";
            }
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "UPDATE tableros set asignacion = @asignacion where idTablero= @idTablero;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
                cmd.Parameters.Add(new SQLiteParameter("@asignacion", asignado));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
        }
        public Pictogram defaultPict()
        {
            Pictogram pict = new Pictogram();
            pict.ID = 0;
            pict.Nombre = "default";
            pict.Texto = "default";
            pict.Imagen = getImageFromResources(WpfApp1.Properties.Resources.add3);
            return pict;
        }
        public void deleteBoard(int idTablero)
        {
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "Delete from pictTableros where idTablero = @idTablero;" +
                    "Delete from tableros where idTablero = @idTablero;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
        }
        public List<Board> getAllAsignBoards()
        {
            List<Board> listaTableros = new List<Board>();
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "select idTablero, idAlfaTablero, nombreTablero, tipo,filas,columnas,pictPortada,asignacion,conTiempo from tableros where asignacion = 'Asignado';";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        List<etiquetaT> listaEtiquetas = GetEtiquetasFromBoard(int.Parse(dr["idTablero"].ToString()));
                        string etiquetasJuntas = "";
                        foreach (etiquetaT etiquetaT in listaEtiquetas)
                        {
                            if (etiquetaT == listaEtiquetas.First())
                            {
                                etiquetasJuntas = etiquetaT.NombreEtiqueta;
                            }
                            else
                            {
                                etiquetasJuntas = etiquetasJuntas + ", " + etiquetaT.NombreEtiqueta;
                            }

                        }
                        Pictogram pPortada = new Pictogram();
                        if (int.Parse(dr["pictPortada"].ToString()) != 0)
                        {
                            pPortada = getOnePictogram(int.Parse(dr["pictPortada"].ToString()));
                        }
                        else
                        {
                            pPortada = defaultPict();
                        }
                        listaTableros.Add(new Board
                        {
                            ID = int.Parse(dr["idTablero"].ToString()),
                            idAlfaTablero = dr["idAlfaTablero"].ToString(),
                            nombreTablero = dr["nombreTablero"].ToString(),
                            tipo = dr["tipo"].ToString(),
                            filas = int.Parse(dr["filas"].ToString()),
                            columnas = int.Parse(dr["columnas"].ToString()),
                            idPictPortada = int.Parse(dr["pictPortada"].ToString()),
                            pictPortada = pPortada,
                            pictTableros = getPictTableros(int.Parse(dr["idTablero"].ToString())),
                            ListaEtiquetasTableros = GetEtiquetasFromBoard(int.Parse(dr["idTablero"].ToString())),
                            EtiquetasJuntas = etiquetasJuntas,
                            asignacion = dr["asignacion"].ToString(),
                            conTiempo = dr["conTiempo"].ToString()
                        });
                    }
                }
                conexion.Close();
            }

            return listaTableros;
        }
        public PerfilModel ObtenerPerfil()
        {
            PerfilModel perfil = new PerfilModel();
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "select * from perfil where idPerfil = @idPerfil; ";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idPerfil", 1));
                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        perfil.idPerfil = 1;
                        perfil.idAlfaPerfil = dr["idAlfaPerfil"].ToString();
                        perfil.tipoPerfil = dr["tipoPerfil"].ToString();
                        perfil.nombrePerfil = dr["nombrePerfil"].ToString();
                        perfil.edad = int.Parse(dr["edad"].ToString());
                        perfil.tamaño = dr["tamaño"].ToString();
                        perfil.fotoPerfil = ImageFromBuffer((System.Byte[])dr["fotoPerfil"]);
                        perfil.voz = dr["voz"].ToString();
                    }
                }
                conexion.Close();
            }

            return perfil;
        }
        public void updatePerfilSinFoto(PerfilModel perfil)
        {
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "UPDATE perfil set nombrePerfil = @nombrePerfil , edad = @edad , tamaño = @tamaño, voz=@voz   where idPerfil= @idPerfil;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idPerfil", 1));
                cmd.Parameters.Add(new SQLiteParameter("@nombrePerfil", perfil.nombrePerfil));
                cmd.Parameters.Add(new SQLiteParameter("@edad", perfil.edad));
                cmd.Parameters.Add(new SQLiteParameter("@tamaño", perfil.tamaño));
                cmd.Parameters.Add(new SQLiteParameter("@voz", perfil.voz));
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
        }
        public void updatePerfilConFoto(PerfilModel perfil,string pathImagen)
        {
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                byte[] pic = File.ReadAllBytes(pathImagen);
                
                conexion.Open();
                string query = "UPDATE perfil set nombrePerfil = @nombrePerfil , edad = @edad , tamaño = @tamaño,fotoPerfil = @fotoPerfil, voz=@voz   where idPerfil= @idPerfil;";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idPerfil", 1));
                cmd.Parameters.Add(new SQLiteParameter("@nombrePerfil", perfil.nombrePerfil));
                cmd.Parameters.Add(new SQLiteParameter("@edad", perfil.edad));
                cmd.Parameters.Add(new SQLiteParameter("@tamaño", perfil.tamaño));
                SQLiteParameter parametro = new SQLiteParameter("@fotoPerfil", System.Data.DbType.Binary);
                cmd.Parameters.Add(new SQLiteParameter("@voz", perfil.voz));
                parametro.Value = pic;
                cmd.Parameters.Add(parametro);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
        }
        public void crearPerfilAlumno(PerfilModel datosPerfil,string pathImagen)
        {
            string alfaIdPerfil = Guid.NewGuid().ToString();
            byte[] pic = File.ReadAllBytes(pathImagen);
            int idBoard = 0;
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                //AÑADE EL TABLERO A LA BASE DE DATOS LOCAL
                conexion.Open();
                string query = "insert into perfil( idPerfil, idAlfaPerfil, tipoPerfil,nombrePerfil,edad,tamaño,fotoPerfil,voz) " +
                    "values (@idPerfil, @idAlfaPerfil, @tipoPerfil,@nombrePerfil,@edad,@tamaño,@fotoPerfil,@voz)";

                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.Parameters.Add(new SQLiteParameter("@idPerfil", 1));
                cmd.Parameters.Add(new SQLiteParameter("@idAlfaPerfil", alfaIdPerfil));
                cmd.Parameters.Add(new SQLiteParameter("@tipoPerfil", "Alumno"));
                cmd.Parameters.Add(new SQLiteParameter("@nombrePerfil", datosPerfil.nombrePerfil));
                cmd.Parameters.Add(new SQLiteParameter("@edad", datosPerfil.edad));
                cmd.Parameters.Add(new SQLiteParameter("@tamaño", datosPerfil.tamaño)); 
                SQLiteParameter parametro = new SQLiteParameter("@fotoPerfil", System.Data.DbType.Binary);
                cmd.Parameters.Add(new SQLiteParameter("@voz", datosPerfil.voz));
                parametro.Value = pic;
                cmd.Parameters.Add(parametro);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                conexion.Close();

            }
        }
        public string getProfileVoice()
        {
            string vozPerfil = "";
            using (SQLiteConnection conexion = new SQLiteConnection(SqliteConnection))
            {
                conexion.Open();
                string query = "select voz from perfil where idPerfil = 1; ";
                SQLiteCommand cmd = new SQLiteCommand(query, conexion);
                cmd.CommandType = System.Data.CommandType.Text;
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        vozPerfil = dr["voz"].ToString();
                    }
                }
                conexion.Close();
            }
            return vozPerfil;
        }
        public string exportPictogramas(List<Pictogram> listaPictogramas, string pathExport)
        {
            string exportsql = "";
            List<int> idsImagenes = new List<int>();
            List<int?> idsSonidos = new List<int?>();
            string queryExportPictogram = "insert into temppictogramas(idPict,idAlfaPict, nombrePict,textoPict,categoriaPict,idImagen,idSonido) values ";
            foreach (Pictogram pictogram in listaPictogramas)
            {
                string row = "";
                if (idsImagenes.Any(x => x == pictogram.idImagen) == false)
                {
                    idsImagenes.Add(pictogram.idImagen);
                }
                
                if(listaPictogramas.First().ID != pictogram.ID)
                {
                    row = ",";
                }
                if (pictogram.idSonido != 0)
                {
                    row = row+ "(" +pictogram.ID+",'"+
                        pictogram.idAlfaPict + "','"+
                        pictogram.Nombre+"','"+
                        pictogram.Texto+"','"+
                        pictogram.Categoria+"',"+
                        pictogram.idImagen+", "+
                        pictogram.idSonido+")";

                    if (idsSonidos.Any(x => x == pictogram.idSonido) == false)
                    {
                        idsSonidos.Add(pictogram.idSonido);
                    }
                }
                else
                {
                    row = "(" + pictogram.ID + ",'" +
                        pictogram.idAlfaPict + "','" +
                        pictogram.Nombre + "','" +
                        pictogram.Texto + "','" +
                        pictogram.Categoria + "'," +
                        pictogram.idImagen + ", " +
                        "NULL)";
                }
                queryExportPictogram = queryExportPictogram + row;


            }
            queryExportPictogram = queryExportPictogram + ";";
            exportsql = queryExportPictogram;
            return exportsql;
        }
    }
}
