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
using WpfApp1.Pages.Player.TEmociones;
using WpfApp1.Pages.Player.TSonidos;
using WpfApp1.Pages.Tableros;

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        static List<Board> ListaAsignados = new List<Board>();
        private static Board boardSelected = new Board();
        private static readonly MenuPage instance = new MenuPage();
        public static MenuPage Instance
        {
            get
            {
                return instance;
            }
        }
        public MenuPage()
        {
            InitializeComponent();
            actualizarLista();
        }
        private void actualizarLista()
        {
            ListaAsignados = Repository.Instance.getAllAsignBoards();
            ListViewTableros.ItemsSource = ListaAsignados;
        }

        private void ListViewPictograms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewTableros.SelectedItem != null)
            {
                boardSelected = (Board)ListViewTableros.SelectedItem;
                if (boardSelected.tipo == "Sonidos")
                {
                    this.NavigationService.Navigate(new TSonidos(boardSelected));
                }
                if (boardSelected.tipo == "Emociones")
                {
                    this.NavigationService.Navigate(new TEmociones(boardSelected));
                }
            }
        }
    }
}
