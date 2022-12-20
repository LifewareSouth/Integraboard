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
            if (Tablero.SelectedItem!=null)
            {
                if (((pictTablero)Tablero.SelectedItem).idPictograma != 0)
                {
                    añadirAlListado((pictTablero)Tablero.SelectedItem);
                    columnsListado++;
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
        private void añadirAlListado(pictTablero pictSeleccionado)
        {
            pictSeleccionado.y = 0;
            pictSeleccionado.x = ListaPictListado.Count();
            ListaPictListado.Add(pictSeleccionado);
        }
        private void AjustarListado()
        {
            BindingList<pictTablero> tempVistas = new BindingList<pictTablero>();
            for (int j = 0; j < columnsListado; j++)
            {
                pictTablero pictTab = new pictTablero();
                if (ListaPict.Any(x => (x.x == j)))
                {
                    pictTab = ListaPictListado.Where(x => (x.x == j)).First();
                }
                pictTab.x = j;
                pictTab.y = 0;
                tempVistas.Add(pictTab);
            }
            
            vistasListado = tempVistas;
            Listado.ItemsSource = vistasListado;
        }
    }
}
