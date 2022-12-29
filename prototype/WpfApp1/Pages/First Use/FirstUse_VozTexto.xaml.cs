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
using WpfApp1.Model;
using WpfApp1.Assets;
using SpeechLib;
using WpfApp1.Pages.Dialogs;
using System.Speech.Synthesis;

namespace WpfApp1.Pages.First_Use
{
    /// <summary>
    /// Lógica de interacción para FirstUse_VozTexto.xaml
    /// </summary>
    public partial class FirstUse_VozTexto : Page
    {
        private static BitmapImage tamanoTexto = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.tamanoConTexto);
        private static BitmapImage voz = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.VozConTexto);
        private static PerfilModel datosPerfil = new PerfilModel();
        string pathImagen = "";
        string tamañoSeleccionado = "";
        List<string> nombresVoces = new List<string>();
        StackPanel LeftMenu;
        SpeechSynthesizer synth = new SpeechSynthesizer();
        private static BitmapImage sound = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.VozConTexto);
        public FirstUse_VozTexto()
        {
            InitializeComponent();
            this.Resources["tamanoLetra"] = tamanoTexto;
            this.Resources["voz"] = voz;
            this.Resources["sound"] = sound;
        }
        public FirstUse_VozTexto(string nombre, int edad, string path,StackPanel menu )
        {
            InitializeComponent();
            datosPerfil.nombrePerfil = nombre;
            datosPerfil.edad = edad;
            pathImagen = path;
            this.Resources["tamanoLetra"] = tamanoTexto;
            this.Resources["voz"] = voz;
            this.Resources["sound"] = sound;
            LeftMenu = menu;
            synth.SpeakCompleted += Reader_SpeakCompleted;
            synth.SetOutputToDefaultAudioDevice();
            synth.Rate = -1;
            getVoices();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
        private void getVoices()
        {
            SpObjectTokenCategory otc = new SpObjectTokenCategory();
            otc.SetId("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Speech\\Voices");
            ISpeechObjectTokens tokenEnum = otc.EnumerateTokens();
            string genderVoice = "";
            int nTockenCount = tokenEnum.Count;
            foreach (ISpeechObjectToken sot in tokenEnum)
            {
                String auxName = sot.GetAttribute("name");
                nombresVoces.Add(auxName);
                auxName = auxName.Replace("AhoTTS_", "");
                auxName = auxName.Replace("Microsoft ", "");
                auxName = auxName.Replace(" Desktop", "");
                auxName = auxName.Replace("_es", "");
                auxName = auxName.Replace("_eus", "");
                String AuxGender = sot.GetAttribute("gender");

                String genero;
                if (AuxGender == "Female")
                {
                    genero = "Mujer";
                    AuxGender = genero;
                    genderVoice = genero;
                }
                else if (AuxGender == "Male")
                {
                    genero = "Hombre";
                    AuxGender = genero;
                    genderVoice = genero;
                }
                else
                {
                    genero = "Niño/a";
                    AuxGender = genero;
                    genderVoice = genero;
                }
                seleccionVoz.Items.Add(auxName + " - " + AuxGender);
            }
            seleccionVoz.SelectedIndex = 0;

        }

        private void largeFont_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            largeFont.Background = new SolidColorBrush(Colors.Cornsilk);
            largeFont.Foreground = new SolidColorBrush(Colors.Black);

            mediumFont.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#1597e5");
            mediumFont.Foreground = new SolidColorBrush(Colors.White);

            smallFont.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#1597e5");
            smallFont.Foreground = new SolidColorBrush(Colors.White);
            tamañoSeleccionado = "Grande";
        }

        private void mediumFont_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            largeFont.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#1597e5");
            largeFont.Foreground = new SolidColorBrush(Colors.White);

            mediumFont.Background = new SolidColorBrush(Colors.Cornsilk);
            mediumFont.Foreground = new SolidColorBrush(Colors.Black);

            smallFont.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#1597e5");
            smallFont.Foreground = new SolidColorBrush(Colors.White);
            tamañoSeleccionado = "Mediano";
        }

        private void smallFont_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            largeFont.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#1597e5");
            largeFont.Foreground = new SolidColorBrush(Colors.White);

            mediumFont.Background = (System.Windows.Media.Brush)bc.ConvertFrom("#1597e5");
            mediumFont.Foreground = new SolidColorBrush(Colors.White);

            smallFont.Background = new SolidColorBrush(Colors.Cornsilk);
            smallFont.Foreground = new SolidColorBrush(Colors.Black);
            tamañoSeleccionado = "Pequeño";
        }
        private bool verificarCampos()
        {
            bool valido = true;
            if (tamañoSeleccionado =="")
            {
                valido = false;
            }
            return valido;
        }

        private void GuardarPerfil_Click(object sender, RoutedEventArgs e)
        {
            if (verificarCampos() == true)
            {
                LeftMenu.Visibility = Visibility.Visible;
                datosPerfil.tamaño = tamañoSeleccionado;
                //VOZ
                SpObjectTokenCategory otc = new SpObjectTokenCategory();
                otc.SetId("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Speech\\Voices");
                ISpeechObjectTokens tokenEnum = otc.EnumerateTokens();
                int nTockenCount = tokenEnum.Count;
                int index = seleccionVoz.SelectedIndex;
                int i= 0;
                foreach (ISpeechObjectToken sot in tokenEnum)
                {
                    if (i==index)
                    {
                        datosPerfil.voz= sot.GetAttribute("name");
                    }
                    i++;
                }

                //-----------------------------------------
                Repository.Instance.crearPerfilAlumno(datosPerfil, pathImagen);
                SuccessDialog success = new SuccessDialog("Perfil creado");
                success.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                success.Show();
                this.NavigationService.Navigate(new MenuPage());
            }
        }

        private void probarVoz_Click(object sender, RoutedEventArgs e)
        {
            if (textoProbarVoz.Text == "Escuchar")
            {
                textoProbarVoz.Text = "Pausar";
                synth.SelectVoice(nombresVoces[seleccionVoz.SelectedIndex]);
                synth.SpeakAsync("Prueba para conocer como se escucha esta voz");
            }
            else if (textoProbarVoz.Text == "Pausar")
            {
                textoProbarVoz.Text = "Escuchar";
                synth.SpeakAsyncCancelAll();
            }

        }

        void Reader_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            textoProbarVoz.Text = "Escuchar";
        }
    }
}
