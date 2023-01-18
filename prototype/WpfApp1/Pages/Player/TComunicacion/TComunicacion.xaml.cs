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
using System.Speech.Synthesis;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using WpfApp1.UserControls;
using NAudio.Wave;
using System.Media;

namespace WpfApp1.Pages.Player.TComunicacion
{
    /// <summary>
    /// Lógica de interacción para TComunicacion.xaml
    /// </summary>
    public partial class TComunicacion : Page
    {
        SpeechSynthesizer synth = new SpeechSynthesizer();
        Mp3FileReader reader;
        IWavePlayer waveOut;
        SoundModel sonidoReproducible = new SoundModel();
        private BindingList<pictTablero> vistas = new BindingList<pictTablero>();
        private BindingList<pictTablero> vistasListado = new BindingList<pictTablero>();
        List<pictTablero> ListaPict = new List<pictTablero>();
        List<pictTablero> ListaPictListado = new List<pictTablero>();
        bool speaking = false;
        bool escucharDirectamente = false;
        private static BitmapImage incorrectoEsquinado = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.incorrectoEsquinado);
        int rowCounter, columnsCounter,columnsListado;
        public TComunicacion()
        {
            InitializeComponent();
        }
        public TComunicacion(Board board)
        {
            InitializeComponent();
            synth.SpeakCompleted += Reader_SpeakCompleted;
            synth.SetOutputToDefaultAudioDevice();
            synth.Rate = -1;
            rowCounter = board.filas;
            columnsCounter = board.columnas;
            Tablero.ItemsSource = board.pictTableros;
            ListaPict = board.pictTableros;
            synth.SelectVoice(Repository.Instance.getProfileVoice());
            AjustarTablero();
        }
        void Reader_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            speaking = false;
            //escuchar.Content = "Escuchar";            
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
                if (!escucharDirectamente)
                {
                    if (ListaPictListado.Count() < 7)
                    {
                        if (((pictTablero)Tablero.SelectedItem).idPictograma != 0)
                        {
                            pictTablero pictTablero = new pictTablero();
                            pictTablero tempPT = (pictTablero)Tablero.SelectedItem;
                            pictTablero.pictograma = tempPT.pictograma;
                            pictTablero.idPictograma = tempPT.idPictograma;
                            pictTablero.imagenEstado = incorrectoEsquinado;
                            ListaPictListado.Add(pictTablero);
                            columnsListado++;
                            AjustarListado();

                            ajustarTamanoListado();
                        }
                    }
                }
                //-------------------------------------------------------------------
                else
                {
                    pictTablero pictTablero = (pictTablero)Tablero.SelectedItem;
                    if (((pictTablero)Tablero.SelectedItem).idPictograma != 0)
                    {
                        synth.SpeakAsyncCancelAll();
                        speaking = true;
                        synth.SpeakAsync(pictTablero.pictograma.Texto);

                    }
                }
                //-----------------------------------------------------------------
                
            }
        }
        private void listado_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Listado.SelectedItem != null)
            {
                if (((pictTablero)Listado.SelectedItem).idPictograma != 0)
                {
                    int idpt = ((pictTablero)Listado.SelectedItem).ID;
                    ListaPictListado.Remove(ListaPictListado.Where(x=>x.ID==idpt).First());
                    columnsListado--;
                    ajustarTamanoListado();
                    AjustarListado();

                    
                    
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

        private void escuchar_Click(object sender, RoutedEventArgs e)
        {
            
            if (escucharDirectamente == false)
            {
                if (speaking==false)
                {
                    if (ListaPictListado.Count() > 0)
                    {
                        //escuchar.Content = "Parar";
                        string oracion = "";
                        foreach (pictTablero pt in ListaPictListado)
                        {

                            oracion = oracion + " " + pt.pictograma.Texto;
                        }
                        synth.SpeakAsyncCancelAll();
                        synth.SetOutputToDefaultAudioDevice();
                        synth.Rate = -1;
                        speaking = true;
                        synth.SpeakAsync(oracion);
                    }
                }
                else if (speaking == true)
                {
                    //escuchar.Content = "Escuchar";
                    synth.SpeakAsyncCancelAll();

                }
            }
            
        }
        public void sonido_termina(object sender, WaveOutEvent e)
        {

        }
        public void PlaySound(string audioPath)
        {

            reader = new Mp3FileReader(audioPath);
            waveOut = new WaveOutEvent();

            waveOut.Init(reader);
            waveOut.Play();
        }

        public void StopSound()
        {
            waveOut.Stop();
            this.waveOut.Dispose();
        }

        private void volverMenu_Click(object sender, RoutedEventArgs e)
        {
            if (speaking == true)
            {
                synth.SpeakAsyncCancelAll();
            }
            this.NavigationService.Navigate(new MenuPage());
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            escucharDirectamente = true;
            panelSuperior.Visibility = Visibility.Collapsed;
            viewboxTablero.Margin = new Thickness(0, -107, 0, 0);
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            escucharDirectamente = false;
            panelSuperior.Visibility = Visibility.Visible;
            viewboxTablero.Margin = new Thickness(0, 10, 0, 0);
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
                pictTab.ID = j;
                tempVistas.Add(pictTab);
            }
            vistasListado = tempVistas;
            Listado.ItemsSource = vistasListado;
            List<pictTablero> tempListaPictListado = new List<pictTablero>();
            foreach (pictTablero pt in vistasListado)
            {
                tempListaPictListado.Add(pt);
            }
            ListaPictListado = tempListaPictListado;
        }
    }
}
