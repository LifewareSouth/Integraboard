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
using WpfApp1.Pages.Dialogs;
using WpfApp1.Pages.Pictogramas;

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para Pictogramas.xaml
    /// </summary>
    public partial class MainPicrogramasPage : Page
    {
        bool _navigationServiceAssigned = false;
        static bool actualizandoPictogramas = false;
        static List<Pictogram> listaPict = new List<Pictogram>();
        private static readonly MainPicrogramasPage instance = new MainPicrogramasPage();
        public static MainPicrogramasPage Instance
        {
            get
            {
                return instance;
            }
        }
        public MainPicrogramasPage()
        {
            InitializeComponent();
            ActualizarLista();
        }
        private void ActualizarLista()
        {
            Style rowStyle = new Style(typeof(DataGridRow));
            rowStyle.Setters.Add(new EventSetter(DataGridRow.MouseDoubleClickEvent,
                                     new MouseButtonEventHandler(Row_DoubleClick)));
            
            listaPict = Repository.Instance.getAllPict();
            if (listaPict.Count > 0)
            {
                ListViewPictograms.ItemsSource = listaPict;
            }
        }
        private void ListViewPictograms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }
        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Pictogram pictEdit = ((Pictogram)ListViewPictograms.SelectedItem);
            this.NavigationService.Navigate(new CrearPictograma(pictEdit));
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

        public void runActualizarLista()
        {
            actualizandoPictogramas = true;
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
                if (actualizandoPictogramas == true)
                {
                    ActualizarLista();
                    actualizandoPictogramas = false;
                }
            }
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewPictograms.SelectedValue != null)
            {
                Pictogram pictEdit = ((Pictogram)ListViewPictograms.SelectedItem);
                this.NavigationService.Navigate(new CrearPictograma(pictEdit));
            }
        }
    }
}
