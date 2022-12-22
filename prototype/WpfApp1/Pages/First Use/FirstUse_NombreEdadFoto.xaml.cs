using System;
using System.Collections.Generic;
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
using WpfApp1.Assets;

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
        public FirstUse_NombreEdadFoto()
        {
            InitializeComponent();
            this.Resources["cam"] = cameraButton;
            this.Resources["nombre"] = nombre;
            this.Resources["edad"] = edad;
            this.Resources["tomarFoto"] = tomarFoto;
            this.Resources["subirFoto"] = seleccionarFoto;
            this.Resources["menosEdad"] = menos;
            this.Resources["masEdad"] = mas;
        }
    }
    
}
