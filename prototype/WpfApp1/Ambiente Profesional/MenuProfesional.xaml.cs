using AForge.Math.Geometry;
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
using WpfApp1.Ambiente_Profesional.Perfil;
using WpfApp1.Pages.Perfil;
using WpfApp1.Pages.Pictogramas;
using WpfApp1.Pages.Tableros;

namespace WpfApp1.Ambiente_Profesional
{
    /// <summary>
    /// Lógica de interacción para MenuProfesional.xaml
    /// </summary>
    public partial class MenuProfesional : Page
    {
        public MenuProfesional()
        {
            InitializeComponent();
        }
        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            //Close();
        }

        private void Pictogramas_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPicrogramasPage());

        }

        private void Tableros_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainTablerosPage());
        }

        private void perfil_Click(object sender, RoutedEventArgs e)
        {
            //perfilprofesional
            this.NavigationService.Navigate(new PerfilProfesional());
        }
    }
}
