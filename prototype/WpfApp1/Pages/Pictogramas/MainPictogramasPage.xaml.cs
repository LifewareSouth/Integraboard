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
using WpfApp1.Pages.Dialogs;
using WpfApp1.Pages.Pictogramas;

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para Pictogramas.xaml
    /// </summary>
    public partial class MainPicrogramasPage : Page
    {
        public MainPicrogramasPage()
        {
            InitializeComponent();
        }

        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            Pictos.Content = new Tableros();
        }*/

        private void VolverMenu_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MenuPage());
        }

        private void CrearPictos(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CrearPictograma());
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            //this.NavigationService.Navigate(new DeleteDialog());
            DeleteDialogWin w = new DeleteDialogWin();
            w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            w.Show();
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            PictoPreview w = new PictoPreview();
            w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            w.Show();
        }
    }
}
