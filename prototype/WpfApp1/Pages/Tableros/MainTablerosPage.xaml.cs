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
using WpfApp1.Model;
using WpfApp1.Pages.Pictogramas;
using WpfApp1.Pages.Tableros;

namespace WpfApp1.Pages.Tableros
{
    /// <summary>
    /// Lógica de interacción para MainTablerosPage.xaml
    /// </summary>
    public partial class MainTablerosPage : Page
    {
        static List<Board> listaTableros = new List<Board>();
       

        public MainTablerosPage()
        {
            InitializeComponent();
            actualizarListaTableros();
        }
        private void actualizarListaTableros()
        {
            listaTableros = Repository.Instance.getAllBoards();
            if (listaTableros.Count > 0)
            {
                listViewTableros.ItemsSource = listaTableros;
            }
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
