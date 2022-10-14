using Microsoft.Win32;
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
        public SelectSound()
        {
            InitializeComponent();
            List<SoundModel> listSonidos = Repository.Instance.GetAllSounds();
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
    }
}
