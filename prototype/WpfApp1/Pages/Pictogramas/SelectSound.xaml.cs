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
        }

        private void RecordSound_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddSounds_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofdSound = new OpenFileDialog();

                        ofdSound.Filter = "Sonidos (*.mp3)|*.mp3";
                        bool? response = ofdSound.ShowDialog();
        }
    }
}
