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
using WpfApp1.Pages.Dialogs;

namespace WpfApp1.Pages.Dialogs.VistaPrevia
{
    /// <summary>
    /// Lógica de interacción para TRutina.xaml
    /// </summary>
    public partial class previaRutina : Window
    {
        private static BitmapImage imagenBoton = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.playButtonBlanco);
        private static BitmapImage correctoEsquinado = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.correctoEsquinado);
        private static BitmapImage saltarEsquinado = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.saltarEsquinado);
        private static BitmapImage tiempo = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.timer);
        
        private BindingList<pictTablero> vistas = new BindingList<pictTablero>();
        List<pictTablero> ListaPict = new List<pictTablero>();
        static int segundosPict = 0;
        int rowCounter, columnsCounter;
        int pictTablerosCount = 0; 
        bool conTiempo = false;
        DispatcherTimer timer = new DispatcherTimer();
        public previaRutina()
        {
            InitializeComponent();
           
        }

        public previaRutina(Board board)
        {
            InitializeComponent();
            rowCounter = board.filas;
            columnsCounter = board.columnas;
            ListaPict = board.pictTableros;
            Tablero.ItemsSource = board.pictTableros;
            this.Resources["check"] = imagenBoton;
            this.Resources["temp"] = tiempo;

            pictTablerosCount = board.pictTableros.Count();
            if(board.conTiempo == "Si")
            {
                conTiempo = true;
                segundosPict = int.Parse(board.pictTableros.First().tiempo);
                ajustar_tiempo();

            }
            else if(board.conTiempo == "No")
            {
                conTiempo = false;
                playButton.Visibility = Visibility.Collapsed;
                SeccionTiempo.Visibility = Visibility.Collapsed;
                agregarTiempo.Visibility = Visibility.Collapsed;
            }
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


        
        void ajustar_tiempo()
        {
            if (segundosPict < 60)
            {
                if (segundosPict < 10)
                {
                    lblTime.Text = "00:0" + segundosPict;
                }
                else
                {
                    lblTime.Text = "00:" + segundosPict;
                }
            }
            else
            {
                int minutos = segundosPict / 60;
                int segundos = segundosPict - (minutos * 60);
                string textoSegundos = "";
                string textoMinutos = "";
                if (minutos < 10)
                {
                    textoMinutos = "0" + minutos;
                }
                else
                {
                    textoMinutos = minutos.ToString();
                }
                if (segundos < 10)
                {
                    textoSegundos = "0" + segundos;
                }
                else
                {
                    textoSegundos = segundos.ToString();
                }
                lblTime.Text = textoMinutos + ":" + textoSegundos;
            }
        }

        
    }
}
