using AForge.Math.Geometry;
using Microsoft.Win32;
using NAudio.Wave;
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
using WpfApp1.Pages.Dialogs;
using WpfApp1.Pages.Tableros;

namespace WpfApp1.Pages.Pictogramas
{
    /// <summary>
    /// Lógica de interacción para SelectSound.xaml
    /// </summary>
    public partial class SelectSound : Page
    {
        private static BitmapImage volver = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.arrowBlanca);
        Mp3FileReader reader;
        IWavePlayer waveOut;
        SoundModel sonidoReproducible = new SoundModel();
        bool _navigationServiceAssigned = false;
        private static readonly SelectSound instance = new SelectSound();
        static bool actualizandoListaSonidos = false;
        private static BitmapImage imagenSonido= Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.sound);
        public static SelectSound Instance
        {
            get
            {
                return instance;
            }
        }
        public SelectSound()
        {
            InitializeComponent();
            actualizarListaSonidos();
            this.Resources["volver"] = volver;
        }
        private void actualizarListaSonidos()
        {
            List<SoundModel> listSonidos = Repository.Instance.GetAllSounds();
            ListViewSounds.ItemsSource = listSonidos;
            this.Resources["sound"] = imagenSonido;
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
                if (actualizandoListaSonidos)
                {
                    actualizarListaSonidos();
                    actualizandoListaSonidos = false;
                }
            }
        }
        public void runactualizarListaSonidos()
        {
            actualizandoListaSonidos = true;
        }
        private void AddSounds_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofdSound = new OpenFileDialog();
            ofdSound.Filter = "Sonidos (*.mp3)|*.mp3";
            bool? response = ofdSound.ShowDialog();
            if(response == true)
            {
                string pathSonidoNuevo = ofdSound.FileName;
                string nombreSonidoNuevo = System.IO.Path.GetFileNameWithoutExtension(pathSonidoNuevo);
                Repository.Instance.CrearSonido(pathSonidoNuevo, nombreSonidoNuevo, false);

                actualizarListaSonidos();
            }
        }

        private void RecordSound_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new VoiceRecorder());
        }

        private void playbtn_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewSounds.SelectedValue != null)
            {
                pausebtn.IsEnabled = true;
                playbtn.IsEnabled = false;
                sonidoReproducible = ((SoundModel)ListViewSounds.SelectedItem);
                PlaySound(sonidoReproducible.pathSonido);
            }
        }

        private void pausebtn_Click(object sender, RoutedEventArgs e)
        {
            StopSound();
            pausebtn.IsEnabled = false;
            playbtn.IsEnabled = true;

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

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void btn_aceptar_Click(object sender, RoutedEventArgs e)
        {
            String Mensaje = "Listo!";
            if (ListViewSounds.SelectedValue != null)
            {
                //EN CASO DE ESTAR REPRODUCIENDO SONIDO, LO PAUSA
                if (waveOut != null)
                {
                    if (waveOut.PlaybackState == PlaybackState.Playing)
                    {
                        StopSound();
                    }
                }                
                CrearPictos.Instance.SoundFromDb(((SoundModel)ListViewSounds.SelectedItem));
                SuccessDialog success = new SuccessDialog(Mensaje);
                success.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                success.Show();
                this.NavigationService.GoBack();
            }
        }

        private void GoToPictogramas(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
