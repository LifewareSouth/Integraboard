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

namespace WpfApp1.Pages.Player.TEmociones
{
    /// <summary>
    /// Lógica de interacción para TEmociones.xaml
    /// </summary>
    public partial class TEmociones : Page
    {
        private BindingList<pictTablero> vistas = new BindingList<pictTablero>();
        List<pictTablero> ListaPict = new List<pictTablero>();
        int rowCounter, columnsCounter;
        public TEmociones()
        {
            InitializeComponent();
        }

        public TEmociones(Board board)
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

        private void volverMenu_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MenuPage());
        }

        private void Tablero_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Tablero.SelectedItem!= null)
            {
                pictTablero ptSeleccionado =  ((pictTablero)Tablero.SelectedItem);
                if (ptSeleccionado.ID!=0)
                {
                    Pictogram pictSeleccionado = ptSeleccionado.pictograma;
                    pictoBorde.BorderBrush = pictSeleccionado.colorBorde;
                    TextoPict.Text = pictSeleccionado.Texto;
                    imagenPict.Source = pictSeleccionado.Imagen;
                }    
            }
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
    }
}
