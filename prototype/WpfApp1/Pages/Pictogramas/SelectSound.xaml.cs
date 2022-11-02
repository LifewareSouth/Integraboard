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

namespace WpfApp1.Pages.Pictogramas
{
    /// <summary>
    /// Lógica de interacción para SelectSound.xaml
    /// </summary>
    public partial class SelectSound : Page
    {
        Mp3FileReader reader;
        IWavePlayer waveOut;
        SoundModel sonidoReproducible = new SoundModel();
        public SelectSound()
        {
            InitializeComponent();
            List<SoundModel> listSonidos = Repository.Instance.GetAllSounds();
            if (listSonidos.Count > 0)
            {
                ListViewSounds.ItemsSource = listSonidos;
            }
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

                //CODIGO PARA ACTUALIZAR LISTADO DE LA PAGINA ACTUAL
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
                this.NavigationService.GoBack();
            }
        }
    }
}
