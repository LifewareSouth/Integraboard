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
using WpfApp1.Pages.Tableros;

namespace WpfApp1.UserControls
{
    /// <summary>
    /// Lógica de interacción para AddPictogram.xaml
    /// </summary>
    public partial class AddPictogram : UserControl
    {
        public AddPictogram()
        {
            InitializeComponent();
        }

        private void AddPictogramClick(object sender, RoutedEventArgs e)
        {
            //this.NavigationService.Navigate(new MenuPage());
            //addPictogram.Content = ListadoPictogramas();
            /*Uri uri = new Uri("./Pages/Tableros/ListadoTrableros.xaml", UriKind.Relative);
            this.NavigationService.Navigate(uri);
            */
        }
    }
}
