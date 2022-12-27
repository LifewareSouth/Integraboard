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
using WpfApp1.Model;
using WpfApp1.Assets;

namespace WpfApp1.Pages.First_Use
{
    /// <summary>
    /// Lógica de interacción para FirstUse_VozTexto.xaml
    /// </summary>
    public partial class FirstUse_VozTexto : Page
    {
        private static BitmapImage tamanoTexto = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.tamanoConTexto);
        private static BitmapImage voz = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.VozConTexto);
        private static PerfilModel datosPerfil = new PerfilModel();
        string pathImagen = "";
        private static BitmapImage sound = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.VozConTexto);
        public FirstUse_VozTexto()
        {
            InitializeComponent();
            this.Resources["tamanoLetra"] = tamanoTexto;
            this.Resources["voz"] = voz;
            this.Resources["sound"] = sound;
        }
        public FirstUse_VozTexto(string nombre, int edad, string path )
        {
            InitializeComponent();
            datosPerfil.nombrePerfil = nombre;
            datosPerfil.edad = edad;
            pathImagen = path;
            this.Resources["tamanoLetra"] = tamanoTexto;
            this.Resources["voz"] = voz;

        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
