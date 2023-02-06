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

namespace WpfApp1.Pages.Dialogs.VistaPrevia
{
    /// <summary>
    /// Lógica de interacción para TEmociones.xaml
    /// </summary>
    public partial class previaEmociones : Window
    {
        private BindingList<pictTablero> vistas = new BindingList<pictTablero>();
        List<pictTablero> ListaPict = new List<pictTablero>();
        int rowCounter, columnsCounter;
        private static BitmapImage menuBtn = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.menuConTexto);
        private static BitmapImage closeButton = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.close_cruz_invertida);
        public previaEmociones()
        {
            InitializeComponent();
        }

        public previaEmociones(Board board)
        {
            InitializeComponent();
            rowCounter = board.filas;
            columnsCounter = board.columnas;
            Tablero.ItemsSource = board.pictTableros;
            ListaPict = board.pictTableros;
            AjustarTablero();
            this.Resources["menuBtn"] = menuBtn;
            this.Resources["closeButton"] = closeButton;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
