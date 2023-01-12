using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Assets;
using WpfApp1.Model;
using WpfApp1.Pages.Dialogs;
using WpfApp1.Pages.Pictogramas;
using MessageBox = System.Windows.Forms.MessageBox;

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
        static List<Pictogram> filteredPict = new List<Pictogram>();
        BackgroundWorker _worker;
        cargandolalistadepicto cargandopicto = new cargandolalistadepicto();
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
            CategoriaPict.Items.Add("Todas");
            foreach (Pictogram.PictogramCategory foo in Enum.GetValues(typeof(Pictogram.PictogramCategory)))
            {
                object aux = CategoriaPict.Items.Add(Repository.PictogramCategoryToString(foo));
            }
            CategoriaPict.Text = "Todas";
            ActualizarLista();
        }
        private void ActualizarLista()
        {
            Style rowStyle = new Style(typeof(DataGridRow));
            rowStyle.Setters.Add(new EventSetter(DataGridRow.MouseDoubleClickEvent,
                                     new MouseButtonEventHandler(Row_DoubleClick)));
 
            if ((listaPict.Count == 0) || (actualizandoPictogramas == true))
            {
                _worker = new BackgroundWorker();
                _worker.WorkerReportsProgress = true;
                _worker.WorkerSupportsCancellation = true;
                _worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                _worker.RunWorkerAsync();
                cargandopicto.Show();
            
                ListViewPictograms.ItemsSource = listaPict;
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            
            
            listaPict = Repository.Instance.getAllPict();
            //Thread.Sleep(5000);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cargandopicto.Close();
        }



        private void ListViewPictograms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }
        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Pictogram pictEdit = ((Pictogram)ListViewPictograms.SelectedItem);
            this.NavigationService.Navigate(new CrearPictos(pictEdit,false,null));
        }

        private void VolverMenu_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MenuPage());
        }

        private void CrearPictos(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CrearPictos());
        }

        private async void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewPictograms.SelectedValue != null)
            {
                DeleteDialogWin w = new DeleteDialogWin();
                w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                var result = w.ShowDialog();
                if(result == true)
                {
                    Pictogram pictEliminar = ((Pictogram)ListViewPictograms.SelectedItem);
                    Repository.Instance.deletePictograma(pictEliminar.ID);
                    ActualizarLista();
                }
            }
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewPictograms.SelectedValue != null)
            {
                Pictogram pictSeleccionado = ((Pictogram)ListViewPictograms.SelectedItem);
                PictoPreview w = new PictoPreview(pictSeleccionado);
                w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                w.Show();
            }
                
        }

        public void runActualizarLista()
        {
            //listaPict.Clear();
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
                    this.DataContext = null;
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
                this.NavigationService.Navigate(new CrearPictos(pictEdit,false,null));
            }
        }

        private void buscadorPict_TextChanged(object sender, TextChangedEventArgs e)
        {
            AplicarFiltro();
        }

        private void CategoriaPict_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltro();
        }
        private void AplicarFiltro()
        {
            string textSearch = buscadorPict.Text.ToUpper();
            List<Pictogram> filtro = new List<Pictogram>();
            Pictogram asd = new Pictogram();
            filteredPict = listaPict;
            if (CategoriaPict.SelectedItem.ToString() != "Todas")
            {
                filtro = filteredPict.Where(x => (x.Categoria.Equals(CategoriaPict.SelectedItem.ToString()))) /* filtro categorias*/
                .ToList();
                filteredPict = filtro;
            }
            if (buscadorPict.Text != null && buscadorPict.Text != "")
            {
                filtro = filteredPict.Where(x => (x.Nombre.ToUpper().Contains(textSearch)) ||  /*filtro nombre*/
                (x.Texto.ToUpper().Contains(textSearch)) || /* filtro texto*/
                (x.ListaEtiquetas.Where(x => (x.NombreEtiqueta.ToUpper().Contains(textSearch))).Count() > 0)) /* filtro etiquetas*/
                .ToList();
                filteredPict = filtro;
            }
            ListViewPictograms.ItemsSource = filteredPict;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ImportarExportar(listaPict));
        }

        public List<Pictogram> getPictograms()
        {
            return listaPict;
        }
    }
}
