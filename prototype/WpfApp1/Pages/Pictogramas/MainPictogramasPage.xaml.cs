using System;
using System.Collections;
using System.Collections.Generic;
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

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para Pictogramas.xaml
    /// </summary>
    /// 
    class threadClass
    {
        bool activo = false;

        public bool Activo
        {
            set { activo = value; }
        }



        public threadClass() { }

        public void ThreadMethod()
        {
           
            Console.WriteLine("newThread is executing ThreadMethod.");

            try
            {
                //CAMBIAR DIALOG POR UNO NUEVO

                Cargandopictodialog cd = new Cargandopictodialog();
                cd.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                while (!activo) 
                {
                    cd.Show();
                }
                cd.Close();

            }
            catch (ThreadInterruptedException e)
            {
                Console.WriteLine("newThread cannot go to sleep - " +
                    "interrupted by main thread.");
            }
        }
    }
    public partial class MainPicrogramasPage : Page
    {
        private static BitmapImage volver = Repository.Instance.getImageFromResources(Integraboard.Properties.Resources.arrowBlanca);
        bool _navigationServiceAssigned = false;
        static bool actualizandoPictogramas = false;
        static List<Pictogram> listaPict = new List<Pictogram>();
        static List<Pictogram> filteredPict = new List<Pictogram>();
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
            this.Resources["volver"] = volver;
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
            if((listaPict.Count == 0)||(actualizandoPictogramas==true))
            {
                threadClass threadclass = new threadClass();
                Thread newThread =
                    new Thread(new ThreadStart(threadclass.ThreadMethod));
                newThread.SetApartmentState(ApartmentState.STA);
                newThread.Start();
                listaPict = Repository.Instance.getAllPict();
                ListViewPictograms.ItemsSource = listaPict;
                actualizandoPictogramas = false;
                threadclass.Activo = true;
                newThread.Join();               
            }
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
                    actualizandoPictogramas = true;
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
