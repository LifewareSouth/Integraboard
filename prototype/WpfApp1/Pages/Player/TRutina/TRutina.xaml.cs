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
using System.Windows.Threading;
using WpfApp1.Assets;
using WpfApp1.Model;

namespace WpfApp1.Pages.Player.TRutina
{
    /// <summary>
    /// Lógica de interacción para TRutina.xaml
    /// </summary>
    public partial class TRutina : Page
    {
        private static BitmapImage imagenBoton = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.playButtonBlanco);
        private BindingList<pictTablero> vistas = new BindingList<pictTablero>();
        List<pictTablero> ListaPict = new List<pictTablero>();
        static int segundosPict = 0;
        int rowCounter, columnsCounter;
        int pictTablerosCount = 0;
        DispatcherTimer timer = new DispatcherTimer();
        public TRutina()
        {
            InitializeComponent();
            //this.Resources["check"] = imagenBoton;
        }

        public TRutina(Board board)
        {
            InitializeComponent();
            rowCounter = board.filas;
            columnsCounter = board.columnas;
            ListaPict = board.pictTableros;
            Tablero.ItemsSource = board.pictTableros;
            this.Resources["check"] = imagenBoton;
            pictTablerosCount = board.pictTableros.Count();
            segundosPict = int.Parse(board.pictTableros.First().tiempo);
            lblTime.Content = segundosPict;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            
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

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            playButton.Visibility = Visibility.Collapsed;
            Tablero.SelectedIndex = 0;
            botonesInferiores.Visibility = Visibility.Visible;
            timer.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            if (segundosPict > 0)
            {
                segundosPict--;
                lblTime.Content = segundosPict;
            }
            else
            {
                if(Tablero.SelectedIndex < pictTablerosCount-1)
                {
                    Tablero.SelectedIndex = Tablero.SelectedIndex + 1;
                    segundosPict = int.Parse(((pictTablero)Tablero.SelectedItem).tiempo);
                    lblTime.Content = segundosPict;
                }
                else
                {
                    timer.Stop();
                }
                
            }
            
        }
    }
}
