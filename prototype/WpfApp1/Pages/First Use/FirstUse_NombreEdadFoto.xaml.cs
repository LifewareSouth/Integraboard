using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
using WpfApp1.Assets;
using WpfApp1.Pages.Dialogs;
using WpfApp1.Pages.Perfil;

namespace WpfApp1.Pages.First_Use
{
    /// <summary>
    /// Lógica de interacción para FirstUse_NombreEdadFoto.xaml
    /// </summary>
    public partial class FirstUse_NombreEdadFoto : Page
    {
        private static BitmapImage menos = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.menos);
        private static BitmapImage mas = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.mas);
        private static BitmapImage cameraButton = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.camera);
        private static BitmapImage nombre = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.nombreConTexto);
        private static BitmapImage edad = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.edadConTexto);
        private static BitmapImage tomarFoto = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.tomarFotoConTexto);
        private static BitmapImage seleccionarFoto = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.seleccionarFotoConTexto);
        static int edadPerfil = 0;
        static string pathImagen = "";
        static StackPanel LeftMenu;
        static bool imagefromCamera = false;
        bool _navigationServiceAssigned = false;
        private static readonly FirstUse_NombreEdadFoto instance = new FirstUse_NombreEdadFoto();
        public static FirstUse_NombreEdadFoto Instance
        {
            get
            {
                return instance;
            }
        }
        public FirstUse_NombreEdadFoto()
        {
            InitializeComponent();
            edadPerfil = int.Parse(Edad.Text);
            this.Resources["cam"] = cameraButton;
            this.Resources["nombre"] = nombre;
            this.Resources["edad"] = edad;
            this.Resources["tomarFoto"] = tomarFoto;
            this.Resources["subirFoto"] = seleccionarFoto;
            this.Resources["menosEdad"] = menos;
            this.Resources["masEdad"] = mas;
            
        }
        public FirstUse_NombreEdadFoto(StackPanel Menu)
        {
            InitializeComponent();
            edadPerfil = int.Parse(Edad.Text);
            this.Resources["cam"] = cameraButton;
            this.Resources["nombre"] = nombre;
            this.Resources["edad"] = edad;
            this.Resources["tomarFoto"] = tomarFoto;
            this.Resources["subirFoto"] = seleccionarFoto;
            this.Resources["menosEdad"] = menos;
            this.Resources["masEdad"] = mas;
            LeftMenu = Menu;


        }
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
                if (imagefromCamera == true)
                {
                    imagenPerfil.Source = new BitmapImage(new Uri(pathImagen));
                    imagefromCamera = false;
                }
            }
        }

        private void menosEdad_Click(object sender, RoutedEventArgs e)
        {
            if (edadPerfil > 1)
            {
                edadPerfil--;
                Edad.Text = edadPerfil.ToString();
            }
        }

        private void masEdad_Click(object sender, RoutedEventArgs e)
        {
            edadPerfil++;
            Edad.Text = edadPerfil.ToString();
        }

        private bool verificarCampos()
        {
            bool valido = true;
            if (String.IsNullOrWhiteSpace(NombreAlumno.Text) == true)
            {
                valido = false;
            }
            else if (pathImagen=="")
            {
                valido = false;
            }
            return valido;
        }

        private void subirFotoButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofdImage = new Microsoft.Win32.OpenFileDialog();
            ofdImage.Filter = "Imagenes (*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
            bool? response = ofdImage.ShowDialog();
            if (response == true)
            {
                foreach (string name in ofdImage.SafeFileNames)
                {
                    pathImagen = ofdImage.FileNames.Where(stringToCheck => stringToCheck.Contains(name)).First();
                }

                imagenPerfil.Source = new BitmapImage(new Uri(pathImagen));
            }
        }

        private void GuardarPict_Click(object sender, RoutedEventArgs e)
        {
            if (verificarCampos() == true)
            {
                this.NavigationService.Navigate(new FirstUse_VozTexto(NombreAlumno.Text, edadPerfil, pathImagen, LeftMenu));
            }
        }

        private void tomarFotoButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new TomarFoto("First Use"));
        }
        public void ImagenFromCamera(string path)
        {
            pathImagen = path;
            imagefromCamera = true;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            WarningTableroSinPortada wt = new WarningTableroSinPortada("Al no crear perfil el programa se cerrará");
            wt.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var result = wt.ShowDialog();
            if (result == true)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }
    }
    
}
