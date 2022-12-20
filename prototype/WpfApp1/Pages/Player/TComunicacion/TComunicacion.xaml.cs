using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Assets;
using WpfApp1.Model;

namespace WpfApp1.Pages.Player.TComunicacion
{
    /// <summary>
    /// Lógica de interacción para TComunicacion.xaml
    /// </summary>
    public partial class TComunicacion : Page
    {
        private BindingList<pictTablero> vistas = new BindingList<pictTablero>();
        private BindingList<pictTablero> vistasListado = new BindingList<pictTablero>();
        List<pictTablero> ListaPict = new List<pictTablero>();
        List<pictTablero> ListaPictListado = new List<pictTablero>();
        private static BitmapImage incorrectoEsquinado = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.incorrectoEsquinado);
        int rowCounter, columnsCounter,columnsListado;
        public TComunicacion()
        {
            InitializeComponent();
        }
        public TComunicacion(Board board)
        {
            InitializeComponent();
            rowCounter = board.filas;
            columnsCounter = board.columnas;
            Tablero.ItemsSource = board.pictTableros;
            ListaPict = board.pictTableros;
            AjustarTablero();
        }
        private void page_Loaded(object sender, RoutedEventArgs e)
        {
            ajustarTamano();
        }
        private void ajustarTamano()
        {
            UniformGrid foundUniformGrid = FindVisualChild<UniformGrid>(Tablero);
            foundUniformGrid.Columns = columnsCounter;
            foundUniformGrid.Rows = rowCounter;
        }
        private T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                    return childOfChild;
            }
            return null;
        }

        private void Tablero_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            
        }
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Tablero.SelectedItem != null)
            {
                if (ListaPictListado.Count() < 7)
                {
                    if (((pictTablero)Tablero.SelectedItem).idPictograma != 0)
                    {
                        pictTablero pictTablero = (pictTablero)Tablero.SelectedItem;
                        pictTablero.imagenEstado = incorrectoEsquinado;
                        ListaPictListado.Add(pictTablero);
                        columnsListado++;
                        AjustarListado();

                        ajustarTamanoListado();
                    }
                }
            }
        }
        private void listado_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Listado.SelectedItem != null)
            {
                if (((pictTablero)Listado.SelectedItem).idPictograma != 0)
                {
                    ListaPictListado.Remove((pictTablero)Listado.SelectedItem);
                    columnsListado--;
                    AjustarListado();

                    ajustarTamanoListado();
                }
            }
        }
        private void ajustarTamanoListado()
        {
            UniformGrid foundUniformGrid = FindVisualChild<UniformGrid>(Listado);
            foundUniformGrid.Columns = columnsListado; 
        }

        private void eliminar_Click(object sender, RoutedEventArgs e)
        {
            ListaPictListado.Clear();
            columnsListado = 0;
            AjustarListado();
            ajustarTamanoListado();
        }

        private void AjustarTablero()
        {
            int totalCuadros = rowCounter * columnsCounter;
            BindingList<pictTablero> tempVistas = new BindingList<pictTablero>();
            for (int i = 0; i < rowCounter; i++)
            {
                for (int j = 0; j < columnsCounter; j++)
                {
                    pictTablero pictTab = new pictTablero();
                    if (ListaPict.Any(x => (x.x == j) && (x.y == i)))
                    {
                        pictTab = ListaPict.Where(x => (x.x == j) && (x.y == i)).First();
                    }
                    else
                    {

                        Pictogram tempPictograma = new Pictogram();
                        tempPictograma.colorBorde = new SolidColorBrush(Colors.White);
                        tempPictograma.Texto = "";
                        pictTab.pictograma = tempPictograma;

                    }
                    pictTab.x = j;
                    pictTab.y = i;
                    tempVistas.Add(pictTab);

                }
            }
            vistas = tempVistas;
            Tablero.ItemsSource = vistas;
        }
        private void AjustarListado()
        {
            Listado.ItemsSource=null;
            BindingList<pictTablero> tempVistas = new BindingList<pictTablero>();
            for (int j = 0; j < columnsListado; j++)
            {
                pictTablero pictTab = new pictTablero();
                /*if (ListaPict.Any(x => (x.x == j)))
                {
                    pictTab = ListaPictListado.Where(x => (x.x == j)).First();
                }*/
                pictTab = ListaPictListado[j];
                tempVistas.Add(pictTab);
            }
            
            vistasListado = tempVistas;
            Listado.ItemsSource = ListaPictListado;
        }
    }
}
