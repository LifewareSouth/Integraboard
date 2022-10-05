using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1.Pages.Pictogramas
{
    /// <summary>
    /// Lógica de interacción para CrearPictograma.xaml
    /// </summary>
    public partial class CrearPictograma : Page
    {
        bool _navigationServiceAssigned = false;
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
                    PictogramImage.Source = LoadBitmapImage(@"C:\IntegraBoard\repo\userProfile\images\cameraPhoto2.png");

                }

                // TODO: whatever state management you're going to do
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
            Microsoft.Win32.OpenFileDialog ofdImage = new OpenFileDialog();
            ofdImage.Filter = "Imagenes (*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
            bool? response = ofdImage.ShowDialog();
            
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
    }
}
