using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using WpfApp1.Assets;
using WpfApp1.Model;

namespace WpfApp1.Pages.Pictogramas
{
    /// <summary>
    /// Lógica de interacción para CrearPictograma.xaml
    /// </summary>
    public partial class CrearPictograma : Page
    {
        bool _navigationServiceAssigned = false;
        string TipoImagen = null, pathImagen;
        private static bool IsImageFromCamera = false;
        private static bool IsImageFromDB = false;
        private static ImagenModel imageDB = new ImagenModel();
        private bool isEdit = false;
        private static Pictogram pictogramEdit = new Pictogram();
        private static readonly CrearPictograma instance = new CrearPictograma();
        private static List<Etiqueta> ListaEtiquetas = new List<Etiqueta>();
        private static SoundModel sonidoSeleccionado = new SoundModel();
        public static CrearPictograma Instance
        {
            get
            {
                return instance;
            }
        }

        public CrearPictograma()
        {
            InitializeComponent();
            rellenarCategorias();
            CategoriaPict.Text = (Repository.PictogramCategoryToString(Pictogram.PictogramCategory.Verbos)).ToString();
            pictoBorde.BorderBrush = Repository.Instance.categoryColor("Verbos");
            ListaEtiquetas = Repository.Instance.getAllEtiquetas();
        }
        private void rellenarCategorias()
        {
            foreach (Pictogram.PictogramCategory foo in Enum.GetValues(typeof(Pictogram.PictogramCategory)))
            {
                object aux = CategoriaPict.Items.Add(Repository.PictogramCategoryToString(foo));
            }
        }
        public CrearPictograma(Pictogram editPict)
        {
            InitializeComponent();
            rellenarCategorias();
            ListaEtiquetas = Repository.Instance.getAllEtiquetas();
            NombrePict.Text = editPict.Nombre;
            TextPict.Text = editPict.Texto;
            CategoriaPict.Text = editPict.Categoria;
            pictoBorde.BorderBrush = Repository.Instance.categoryColor(editPict.Categoria);
            IsImageFromDB = true;
            imageDB.ID = editPict.idImagen;
            imageDB.Nombre = editPict.nombreImagen;
            imageDB.Imagen = editPict.Imagen;
            PictogramImage.Source = editPict.Imagen;
            string etiquetas = "";
            foreach(Etiqueta etiqueta in editPict.ListaEtiquetas)
            {
                if(editPict.ListaEtiquetas.First() == etiqueta)
                {
                    etiquetas = etiqueta.NombreEtiqueta;
                }
                else
                {
                    etiquetas = etiquetas+","+ etiqueta.NombreEtiqueta;
                }
            }
            EtiquetaPict.Text = etiquetas;
            isEdit = true;
            pictogramEdit = editPict;
            TipoImagen = "Database";
        }
        
        private void GoToPictogramas(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPicrogramasPage());
        }

        private void SelectSound_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SelectSound());
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SelectImage());
        }

        private void TakePicture_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new TomarFotografia());
        }

        public void ImagenFromCamera()
        {
            IsImageFromCamera = true;
            IsImageFromDB = false;
        }
        public void ImagenFromDB(ImagenModel imagenModel)
        {
            IsImageFromDB = true;
            IsImageFromCamera = false;
            imageDB = imagenModel;
        }
        public void SoundFromDb(SoundModel soundModel)
        {
            sonidoSeleccionado = soundModel;
        }
        public static BitmapImage LoadBitmapImage(string fileName)
        {
            try
            {
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                    return bitmapImage;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void GuardarPict_Click(object sender, RoutedEventArgs e)
        {
            if (validateConditionsToSave())
            {
                List<string> etiquetasPict = new List<string>();
                List<int> idsTags = new List<int>();
                Pictogram pict = new Pictogram();
                if(TipoImagen == "Camara")
                {
                    pict.idImagen = Repository.Instance.GuardarImagenCamara(@"C:\IntegraBoard\repo\userProfile\images\cameraPhoto2.png");
                }
                else if(TipoImagen == "Database")
                {
                    pict.idImagen = imageDB.ID;
                }
                if (sonidoSeleccionado != null)
                {
                    pict.idSonido = sonidoSeleccionado.ID;
                }
                pict.Nombre = NombrePict.Text;
                pict.Texto = TextPict.Text;
                pict.Categoria = CategoriaPict.SelectedItem.ToString();
                if (!isEdit)//PREGUNTA SI SE ESTA EDITANDO UN PICTOGRAMA O SI ES UNO NUEVO
                {
                    Repository.Instance.CrearPictograma(pict);
                    int idNewPict = Repository.Instance.getLatestPictogram();
                    if (String.IsNullOrWhiteSpace(EtiquetaPict.Text) == false)
                    {
                        idsTags = idTags(EtiquetaPict.Text);
                        Repository.Instance.AsociarEtiquetasPict(idsTags, idNewPict,true);
                    }
                }
                else
                {
                    pict.ID = pictogramEdit.ID;
                    Repository.Instance.EditPictogram(pict);
                    if (String.IsNullOrWhiteSpace(EtiquetaPict.Text) == false)
                    {
                        idsTags = idTags(EtiquetaPict.Text);
                        Repository.Instance.AsociarEtiquetasPict(idsTags,pictogramEdit.ID, false);
                    }
                }
                MainPicrogramasPage.Instance.runActualizarLista();
                this.NavigationService.GoBack();
            }
        }
        private List<int> idTags(string textoEtiquetas)
        {
            List<int> idsTags = new List<int>();
            List<string> etiquetasPict = new List<string>();
            string[] conjuntoEtiquetas = textoEtiquetas.Split(",");
            foreach (string tag in conjuntoEtiquetas)
            {
                etiquetasPict.Add(tag);
            }
            foreach (string tag in etiquetasPict)
            {
                //EN CASO DE NO EXISTIR LA ETIQUETA PREVIEMENTE
                if (!ListaEtiquetas.Any(x => x.NombreEtiqueta == tag))
                {
                    Repository.Instance.InsertEtiqueta(tag);
                    idsTags.Add(Repository.Instance.GetIdEtiqueta(tag));
                }
                else
                {
                    idsTags.Add(ListaEtiquetas.Where(x => x.NombreEtiqueta == tag).Select(ListIdPict => ListIdPict.ID).First());
                }
            }
            return idsTags;
        }
        private bool validateConditionsToSave()
        {
            bool valido = true;
            if (String.IsNullOrWhiteSpace(NombrePict.Text) == true)
            {
                Hint1.Visibility = Visibility.Visible;
                Hint1.Text = "El pictograma debe llevar un nombre";
                valido = false;
            }
            else if (String.IsNullOrWhiteSpace(TextPict.Text) == true)
            {
                Hint1.Visibility = Visibility.Visible;
                Hint1.Text = "El pictograma debe tener un texto";
                valido = false;
            }
            else if (TipoImagen==null)
            {
                Hint1.Visibility = Visibility.Visible;
                Hint1.Text = "El pictograma debe tener una imagen";
                valido = false;
            }
            return valido;
        }
        private void page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_navigationServiceAssigned == false)
            {
                NavigationService.Navigating += NavigationService_Navigating;
                _navigationServiceAssigned = true;
            }
        }

        private void CategoriaPict_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pictoBorde.BorderBrush = Repository.Instance.categoryColor(CategoriaPict.SelectedItem.ToString());
        }

        void NavigationService_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                if (IsImageFromCamera == true)
                {
                    pathImagen = @"C:\IntegraBoard\repo\userProfile\images\cameraPhoto2.png";
                    PictogramImage.Source = LoadBitmapImage(@"C:\IntegraBoard\repo\userProfile\images\cameraPhoto2.png");
                    TipoImagen = "Camara";
                }
                else if (IsImageFromDB == true)
                {
                    PictogramImage.Source = imageDB.Imagen;
                    TipoImagen = "Database";
                }
            }
        }
    }
}
