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

namespace WpfApp1.Pages.Tableros
{
    /// <summary>
    /// Lógica de interacción para ListadoPictogramas.xaml
    /// </summary>
    public partial class ListadoPictogramas : Page
    {
        bool _navigationServiceAssigned = false;
        static bool actualizandoPictogramas = false;
        static List<Pictogram> listaPict = new List<Pictogram>();
        static bool isAddingPictogram = true;
        static List<Pictogram> filteredPict = new List<Pictogram>();
        private static readonly MainPicrogramasPage instance = new MainPicrogramasPage();
        public static MainPicrogramasPage Instance
        {
            get
            {
                return instance;
            }
        }
        public ListadoPictogramas()
        {
            InitializeComponent();
            categorias();
            ActualizarLista();
            isAddingPictogram=false;
            ListViewPictograms.SelectionMode = SelectionMode.Single;
        }

        public ListadoPictogramas(List<Pictogram> PictAgregados)
        {
            InitializeComponent();
            ActualizarLista();
            isAddingPictogram = true;
            if(PictAgregados.Count > 0)
            {//QUITAR DEL LISTADO LOS PICTOGRAMAS YA AGREGADOS
                List<Pictogram> tempList = new List<Pictogram>();
                foreach (Pictogram p in PictAgregados)
                {
                    listaPict.Remove(listaPict.Where(x => x.ID == p.ID).First());
                }
            }
            categorias();

        }
        private void categorias()
        {
            CategoriaPict.Items.Add("Todas");
            foreach (Pictogram.PictogramCategory foo in Enum.GetValues(typeof(Pictogram.PictogramCategory)))
            {
                object aux = CategoriaPict.Items.Add(Repository.PictogramCategoryToString(foo));
            }
            CategoriaPict.Text = "Todas";
        }
        private void ActualizarLista()
        {


            listaPict = Repository.Instance.getAllPict();
            if (listaPict.Count > 0)
            {
                ListViewPictograms.ItemsSource = listaPict;
            }
        }
        private void ListViewPictograms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void btn_aceptar_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewPictograms.SelectedItem != null)
            {
                if (!isAddingPictogram)//CUANDO SE AÑADE PICTOGRAMA DE PORTADA
                {
                    Pictogram p = new Pictogram();
                    p = (Pictogram)ListViewPictograms.SelectedValue;
                    CrearTableros.Instance.addPictPortada(p);
                }
                else//CUANDO SE SUMAN PICTOGRAMAS AL TABLERO
                {
                    List<Pictogram> listaPict = new List<Pictogram>();
                    foreach (Pictogram item in (ListViewPictograms.SelectedItems))
                    {
                        listaPict.Add(item);
                    }
                    if (isAddingPictogram)
                    {
                        CrearTableros.Instance.addPict(listaPict);
                    }
                }
                
                
                this.NavigationService.GoBack();
            }

        }
    }
}
