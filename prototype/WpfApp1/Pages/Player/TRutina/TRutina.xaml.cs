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

namespace WpfApp1.Pages.Player.TRutina
{
    /// <summary>
    /// Lógica de interacción para TRutina.xaml
    /// </summary>
    public partial class TRutina : Page
    {
        private static BitmapImage imagenBoton = Repository.Instance.getImageFromResources(Integraboard.Properties.Resources.playButtonBlanco);
        private static BitmapImage correctoEsquinado = Repository.Instance.getImageFromResources(Integraboard.Properties.Resources.correctoEsquinado);
        private static BitmapImage saltarEsquinado = Repository.Instance.getImageFromResources(Integraboard.Properties.Resources.saltarEsquinado);
        private static BitmapImage tiempo = Repository.Instance.getImageFromResources(Integraboard.Properties.Resources.timer);
        private static BitmapImage saltarTareaRutina = Repository.Instance.getImageFromResources(Integraboard.Properties.Resources.saltarTareaConTexto);
        private static BitmapImage addTime = Repository.Instance.getImageFromResources(Integraboard.Properties.Resources.agregarTiempoConTexto);
        private static BitmapImage terminarTareaRutina = Repository.Instance.getImageFromResources(Integraboard.Properties.Resources.terminarTareaConTexto);
        private static BitmapImage menuBtn = Repository.Instance.getImageFromResources(Integraboard.Properties.Resources.menuConTexto);

        private BindingList<pictTablero> vistas = new BindingList<pictTablero>();
        List<pictTablero> ListaPict = new List<pictTablero>();
        static int segundosPict = 0;
        int rowCounter, columnsCounter;
        int pictTablerosCount = 0; 
        bool conTiempo = false;
        DispatcherTimer timer = new DispatcherTimer();
        public TRutina()
        {
            InitializeComponent();
           
        }

        public TRutina(Board board)
        {
            InitializeComponent();
            rowCounter = board.filas;
            columnsCounter = board.columnas;
            ListaPict = board.pictTableros;
            Tablero.ItemsSource = board.pictTableros;
            this.Resources["menuBtn"] = menuBtn;
            this.Resources["check"] = imagenBoton;
            this.Resources["temp"] = tiempo;
            this.Resources["terminarTarea"] = terminarTareaRutina;
            this.Resources["saltarTarea"] = saltarTareaRutina;
            this.Resources["agregarTiempo"] = addTime;

            pictTablerosCount = board.pictTableros.Count();
            if(board.conTiempo == "Si")
            {
                conTiempo = true;
                segundosPict = int.Parse(board.pictTableros.First().tiempo);
                ajustar_tiempo();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += timer_Tick;
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
                ajustar_tiempo();
            }
            else
            {
                timer.Stop();
                if (this.NavigationService != null)
                {
                    WarningTiempo wT = new WarningTiempo();
                    wT.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    var result = wT.ShowDialog();
                    if (result==true)
                    {
                        segundosPict = 20;
                        ajustar_tiempo();
                        timer.Start();
                    }
                    else
                    {
                        ListaPict.Where(x => x.idPictograma == ((pictTablero)Tablero.SelectedItem).idPictograma).First().imagenEstado = saltarEsquinado;
                        int index = Tablero.SelectedIndex;
                        AjustarTablero();
                        Tablero.SelectedIndex = index;
                        timer.Stop();
                        if (Tablero.SelectedIndex == pictTablerosCount - 1)
                        {
                            SuccessDialog sd = new SuccessDialog("Rutina terminada");
                            sd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            sd.ShowDialog();
                        }
                        if (Tablero.SelectedIndex < pictTablerosCount - 1)
                        {
                            Tablero.SelectedIndex = Tablero.SelectedIndex + 1;
                            segundosPict = int.Parse(((pictTablero)Tablero.SelectedItem).tiempo);
                            ajustar_tiempo();
                            timer.Start();
                        }
                    }
                }
            }
            
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

        private void agregarTiempo_Click(object sender, RoutedEventArgs e)
        {
            segundosPict = segundosPict + 20;
            ajustar_tiempo();
        }

        private void saltarTarea_Click(object sender, RoutedEventArgs e)
        {
            if (conTiempo)
            {
                ListaPict.Where(x => x.idPictograma == ((pictTablero)Tablero.SelectedItem).idPictograma).First().imagenEstado = saltarEsquinado;
                int index = Tablero.SelectedIndex;
                AjustarTablero();
                Tablero.SelectedIndex= index;
                timer.Stop();
                if (Tablero.SelectedIndex == pictTablerosCount - 1)
                {
                    SuccessDialog sd = new SuccessDialog("Rutina terminada");
                    sd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    sd.ShowDialog();
                }
                if (Tablero.SelectedIndex < pictTablerosCount - 1)
                {
                    Tablero.SelectedIndex = Tablero.SelectedIndex + 1;
                    segundosPict = int.Parse(((pictTablero)Tablero.SelectedItem).tiempo);
                    ajustar_tiempo();
                    timer.Start();
                }
            }
            else
            {
                ListaPict.Where(x => x.idPictograma == ((pictTablero)Tablero.SelectedItem).idPictograma).First().imagenEstado = saltarEsquinado;
                int index = Tablero.SelectedIndex;
                AjustarTablero();
                Tablero.SelectedIndex = index;
                if (Tablero.SelectedIndex == pictTablerosCount-1)
                {
                    SuccessDialog sd = new SuccessDialog("Rutina termianda");
                    sd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    sd.ShowDialog();
                }
                if (Tablero.SelectedIndex < pictTablerosCount - 1)
                {
                    Tablero.SelectedIndex = Tablero.SelectedIndex + 1;
                }
            }
            
            
        }

        private void volverMenu_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
            }
            this.NavigationService.Navigate(new MenuPage());
        }

        private void terminarTarea_Click(object sender, RoutedEventArgs e)
        {
            if (conTiempo)
            {
                ListaPict.Where(x => x.idPictograma == ((pictTablero)Tablero.SelectedItem).idPictograma).First().imagenEstado = correctoEsquinado;
                int index = Tablero.SelectedIndex;
                AjustarTablero();
                Tablero.SelectedIndex = index;
                timer.Stop();
                if (Tablero.SelectedIndex == pictTablerosCount - 1)
                {
                    SuccessDialog sd = new SuccessDialog("Rutina terminada");
                    sd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    sd.ShowDialog();
                }
                if (Tablero.SelectedIndex < pictTablerosCount - 1)
                {
                    Tablero.SelectedIndex = Tablero.SelectedIndex + 1;
                    segundosPict = int.Parse(((pictTablero)Tablero.SelectedItem).tiempo);
                    ajustar_tiempo();
                    timer.Start();
                }
            }
            else
            {
                ListaPict.Where(x => x.idPictograma == ((pictTablero)Tablero.SelectedItem).idPictograma).First().imagenEstado = correctoEsquinado;
                int index = Tablero.SelectedIndex;
                AjustarTablero();
                Tablero.SelectedIndex = index;
                if (Tablero.SelectedIndex == pictTablerosCount - 1)
                {
                    SuccessDialog sd = new SuccessDialog("Rutina terminada");
                    sd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    sd.ShowDialog();
                }
                if (Tablero.SelectedIndex < pictTablerosCount - 1)
                {
                    Tablero.SelectedIndex = Tablero.SelectedIndex + 1;
                }
                
            }
        }
    }
}
