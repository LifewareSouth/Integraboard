using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using System.Windows.Shell;
using WpfApp1.Assets;
using WpfApp1.Model;
using WpfApp1.Pages.Dialogs;
using WpfApp1.Pages.Pictogramas;
using WpfApp1.Pages.Tableros;

namespace WpfApp1.Pages.Tableros
{
    /// <summary>
    /// Lógica de interacción para MainTablerosPage.xaml
    /// </summary>
    public partial class MainTablerosPage : Page
    {
        bool _navigationServiceAssigned = false;
        static bool actualizandoLista = false;
        static List<Board> listaTableros = new List<Board>();
        static List<Board> filteredBoard = new List<Board>();
        private static readonly MainTablerosPage instance = new MainTablerosPage();
        private static Board boardSelected = new Board();
        public static MainTablerosPage Instance
        {
            get
            {
                return instance;
            }
        }

        public MainTablerosPage()
        {
            InitializeComponent();
            tiposTablero();
            actualizarListaTableros();
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
                if (actualizandoLista== true)
                {
                    actualizarListaTableros();
                    
                    actualizandoLista = false;
                    cbCustom.SelectedIndex = 0;
                    

                }
            }
        }
        private void tiposTablero()
        {
            tipoTablero.Items.Add("Todos");
            tipoTablero.SelectedIndex = 0;
            foreach (Board.TableroTipo foo in Enum.GetValues(typeof(Board.TableroTipo)))
            {
                object aux = cbCustom.Items.Add(foo.ToString());
                tipoTablero.Items.Add(foo.ToString());
            }
        }
        private void actualizarListaTableros()
        {
            listaTableros = Repository.Instance.getAllBoards();
            
            listViewTableros.ItemsSource = listaTableros;
            
        }
        public void runActualizarLista()
        {
            //listaTableros.Clear();
            actualizandoLista = true;
        }

        private void goToCrearTableros(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CrearTableros());
        }

        private void goToMenu(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MenuPage());
        }

        private void DoubleClick_Tablero(object sender, RoutedEventArgs e)
        {
            if (listViewTableros.SelectedItem != null)
            {
                if(boardSelected.tipo == "Comunicación")
                {
                    this.NavigationService.Navigate(new CrearTableros(boardSelected));
                }
                else if (boardSelected.tipo == "Emociones")
                {
                    this.NavigationService.Navigate(new TableroEmociones(boardSelected));
                }
                else if (boardSelected.tipo == "Rutina")
                {
                    this.NavigationService.Navigate(new TableroRutina(boardSelected));
                }
                else if (boardSelected.tipo == "Sonidos")
                {
                    this.NavigationService.Navigate(new TableroSonidos(boardSelected));
                }
            }
        }

        private void listViewTableros_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewTableros.SelectedItem != null)
            {
                boardSelected = (Board)listViewTableros.SelectedItem;
                if(boardSelected.asignacion =="No Asignado")
                {
                    botonAsignar.Content = "Asignar";
                }
                else if(boardSelected.asignacion == "Asignado")
                {
                    botonAsignar.Content = "Desasignar";
                }
            }
        }

        private void botonAsignar_Click(object sender, RoutedEventArgs e)
        {
            if(listViewTableros.SelectedItem != null)
            {
                int sIndex = listViewTableros.SelectedIndex;
                if (boardSelected.asignacion == "No Asignado")
                {
                    if(boardSelected.idPictPortada != 0)
                    {
                        Repository.Instance.asignacionTablero(true, boardSelected.ID);
                    }
                    else
                    {
                        WarningTableroSinPortada wT = new WarningTableroSinPortada();
                        wT.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        var result = wT.ShowDialog();
                    }
                }
                else if (boardSelected.asignacion == "Asignado")
                {
                    Repository.Instance.asignacionTablero(false, boardSelected.ID);
                }
                listaTableros.Clear();
                actualizarListaTableros();
                listViewTableros.SelectedIndex = sIndex;
            }
            
        }

        private void cbCustom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbCustom.SelectedItem.ToString() == "Comunicación")
            {
                this.NavigationService.Navigate(new CrearTableros());
            }
            else if (cbCustom.SelectedItem.ToString() == "Emociones")
            {
                this.NavigationService.Navigate(new TableroEmociones());
            }
            else if (cbCustom.SelectedItem.ToString() == "Rutina")
            {
                this.NavigationService.Navigate(new TableroRutina());
            }
            else if (cbCustom.SelectedItem.ToString() == "Sonidos")
            {
                this.NavigationService.Navigate(new TableroSonidos());
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (listViewTableros.SelectedItem != null)
            {
                DeleteDialogWin w = new DeleteDialogWin();
                w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                var result = w.ShowDialog();
                if (result == true)
                {
                    Repository.Instance.deleteBoard(boardSelected.ID);
                    actualizarListaTableros();
                }
            }
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (listViewTableros.SelectedItem != null)
            {
                if (boardSelected.tipo == "Comunicación")
                {
                    this.NavigationService.Navigate(new CrearTableros(boardSelected));
                }
                else if (boardSelected.tipo == "Emociones")
                {
                    this.NavigationService.Navigate(new TableroEmociones(boardSelected));
                }
                else if (boardSelected.tipo == "Rutina")
                {
                    this.NavigationService.Navigate(new TableroRutina(boardSelected));
                }
                else if (boardSelected.tipo == "Sonidos")
                {
                    this.NavigationService.Navigate(new TableroSonidos(boardSelected));
                }
            }
        }

        private void buscadorTableros_TextChanged(object sender, TextChangedEventArgs e)
        {
            AplicarFiltro();
        }
        private void tipoTablero_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltro();
        }

        private void AplicarFiltro()
        {
            string textSearch = buscadorTableros.Text.ToUpper();
            List<Board> filtro = new List<Board>();
            Board asd = new Board();
            filteredBoard = listaTableros;
            if (tipoTablero.SelectedItem.ToString() != "Todos")
            {
                filtro = filteredBoard.Where(x => (x.tipo.Equals(tipoTablero.SelectedItem.ToString()))) /* filtro categorias*/
                .ToList();
                filteredBoard = filtro;
            }
            if (buscadorTableros.Text != null && buscadorTableros.Text != "")
            {
                filtro = filteredBoard.Where(x => (x.nombreTablero.ToUpper().Contains(textSearch)) ||  /*filtro nombre*/
                //(x.Texto.ToUpper().Contains(textSearch)) || /* filtro texto*/
                (x.ListaEtiquetasTableros.Where(x => (x.NombreEtiqueta.ToUpper().Contains(textSearch))).Count() > 0)) /* filtro etiquetas*/
                .ToList();
                filteredBoard = filtro;
            }
            listViewTableros.ItemsSource = filteredBoard;
        }

        private void btnExportarImportar_Click(object sender, RoutedEventArgs e)
        {
            //navigate a la page de I/E
        }
    }
}
