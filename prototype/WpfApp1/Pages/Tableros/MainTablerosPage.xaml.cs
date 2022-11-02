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
using WpfApp1.Pages.Pictogramas;

namespace WpfApp1.Pages.Tableros
{
    /// <summary>
    /// Lógica de interacción para MainTablerosPage.xaml
    /// </summary>
    public partial class MainTablerosPage : Page
    {
        public MainTablerosPage()
        {
            InitializeComponent();
        }

        private void goToCrearTableros(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CrearTableros());
        }

        private void goToMenu(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainTablerosPage());
        }
    }
}
