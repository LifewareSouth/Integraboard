using Microsoft.Win32;
using System;
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

        private void page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_navigationServiceAssigned == false)
            {
                NavigationService.Navigating += NavigationService_Navigating;
                _navigationServiceAssigned = true;
            }
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
            }
        }
        private static bool IsImageFromCamera = false;
        private static readonly CrearPictograma instance = new CrearPictograma();
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
            foreach (Pictogram.PictogramCategory foo in Enum.GetValues(typeof(Pictogram.PictogramCategory)))
            {
                object aux = CategoriaPict.Items.Add(Repository.PictogramCategoryToString(foo));
            }
            CategoriaPict.Text = ( Repository.PictogramCategoryToString(Pictogram.PictogramCategory.Verbos)).ToString();
        }
        
        private void GoToPictogramas(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPicrogramasPage());
        }

        private void SelectSound_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofdSound = new OpenFileDialog();
            
            ofdSound.Filter = "Sonidos (*.mp3)|*.mp3";
            bool? response = ofdSound.ShowDialog();
            

            /*if(response == true)
            {
                string filepath = ofd.FileName;
                MessageBox.Show(filepath);
            }*/
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
               //codigo para guardar
            }
        }

        private bool validateConditionsToSave()
        {
            bool valido = true;
            if (String.IsNullOrWhiteSpace(NombrePict.Text) == true)
            {
                Hint1.Visibility = Visibility.Visible;
                Hint1.Text = "El pictograma debe llevar un nombre";
                valido = false;
                //buttonSave.IsEnabled = false;
            }
            else if (String.IsNullOrWhiteSpace(TextPict.Text) == true)
            {
                Hint1.Visibility = Visibility.Visible;
                Hint1.Text = "El pictograma debe tener un texto";
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
    }
}
