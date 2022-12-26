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
using System.Speech;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using NAudio.Gui;
using System.Xml.Linq;
using Microsoft.Win32;
using System.Drawing;
using WpfApp1.Pages.Dialogs;
using System.Windows.Forms;
using SpeechLib;
using System.Runtime.InteropServices;

namespace WpfApp1.Pages.Perfil
{
    /// <summary>
    /// Lógica de interacción para PerfilAlumno.xaml
    /// </summary>
    public partial class PerfilAlumno : Page
    {
        private static BitmapImage cameraButton = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.camera);
        private static BitmapImage nombre = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.nombreConTexto);
        private static BitmapImage edad = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.edadConTexto);
        private static BitmapImage tomarFoto = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.tomarFotoConTexto);
        private static BitmapImage seleccionarFoto = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.seleccionarFotoConTexto);
        private static BitmapImage tamanoTexto = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.tamanoConTexto);
        private static BitmapImage voz = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.VozConTexto);
        private static BitmapImage menos = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.menos);
        private static BitmapImage mas = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.mas);
        private static PerfilModel datosPerfil = new PerfilModel();
        SpVoice voice = new SpVoice();
        bool imagenNueva = false;
        string tamañoSeleccionado = "";
        string pathImagen = "";
        int edadPerfil; 
        public PerfilAlumno()
        {
            datosPerfil = Repository.Instance.ObtenerPerfil();
            InitializeComponent();
            NombreAlumno.Text= datosPerfil.nombrePerfil;
            Edad.Text = datosPerfil.edad.ToString();
            edadPerfil = datosPerfil.edad;
            tamañoSeleccionado = datosPerfil.tamaño;
            //--------------Tamaño texto
            if (datosPerfil.tamaño == "Grande")
            {
                largeFont.Background = new SolidColorBrush(Colors.Cornsilk);
                largeFont.Foreground = new SolidColorBrush(Colors.Black);
            }
            else if (datosPerfil.tamaño == "Mediano")
            {
                mediumFont.Background = new SolidColorBrush(Colors.Cornsilk);
                mediumFont.Foreground = new SolidColorBrush(Colors.Black);
            }
            else if (datosPerfil.tamaño == "Grande")
            {
                smallFont.Background = new SolidColorBrush(Colors.Cornsilk);
                smallFont.Foreground = new SolidColorBrush(Colors.Black);
            }
            imagenPerfil.Source = datosPerfil.fotoPerfil;
            //--------- VOCES
            getVoices();
    
            //--------- Imagenes
            this.Resources["cam"] = cameraButton;
            this.Resources["nombre"] = nombre;
            this.Resources["edad"] = edad;
            this.Resources["tamanoLetra"] = tamanoTexto;
            this.Resources["voz"] = voz;
            this.Resources["tomarFoto"] = tomarFoto;
            this.Resources["subirFoto"] = seleccionarFoto;
            this.Resources["menosEdad"] = menos;
            this.Resources["masEdad"] = mas;
        }
        private void getVoices()
        {
            SpObjectTokenCategory otc = new SpObjectTokenCategory();
            otc.SetId("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Speech\\Voices");
            ISpeechObjectTokens tokenEnum = otc.EnumerateTokens();
            string genderVoice = "";
            int nTockenCount = tokenEnum.Count;
            string vozSeleccionada = datosPerfil.voz;
            vozSeleccionada = vozSeleccionada.Replace("AhoTTS_", "");
            vozSeleccionada = vozSeleccionada.Replace("Microsoft ", "");
            vozSeleccionada = vozSeleccionada.Replace(" Desktop", "");
            vozSeleccionada = vozSeleccionada.Replace("_es", "");
            vozSeleccionada = vozSeleccionada.Replace("_eus", "");
            int index = 0;
            int i = 0;
            foreach (ISpeechObjectToken sot in tokenEnum)
            {
                String auxName = sot.GetAttribute("name");
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
                    if (auxName == vozSeleccionada)
                    {
                        index = i;
                    }
                }
                else if (AuxGender == "Male")
                {
                    genero = "Hombre";
                    AuxGender = genero;
                    genderVoice = genero;
                    if (auxName == vozSeleccionada)
                    {
                        index = i;
                    }
                }
                else
                {
                    genero = "Niño/a";
                    AuxGender = genero;
                    genderVoice = genero;
                    if (auxName == vozSeleccionada)
                    {
                        index = i;
                    }
                }
                seleccionVoz.Items.Add(auxName + " - " + AuxGender);
                i++;
            }
            seleccionVoz.SelectedIndex = index;

        }

        private void menosEdad_Click(object sender, RoutedEventArgs e)
        {
            if (edadPerfil > 1)
            {
                edadPerfil--;
                Edad.Text = edadPerfil.ToString();
            }
        }

        private void masEdad_Click(object sender, RoutedEventArgs e)
        {
            edadPerfil++;
            Edad.Text = edadPerfil.ToString();
        }

        private void subirFotoButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofdImage = new Microsoft.Win32.OpenFileDialog();
            ofdImage.Filter = "Imagenes (*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
            bool? response = ofdImage.ShowDialog();
            if (response == true)
            {
                foreach (string name in ofdImage.SafeFileNames)
                {
                    pathImagen = ofdImage.FileNames.Where(stringToCheck => stringToCheck.Contains(name)).First();
                }

                imagenPerfil.Source = new BitmapImage( new Uri(pathImagen));
                imagenNueva = true;
            }
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

        private void GuardarPerfil_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(NombreAlumno.Text) == false)
            {
                PerfilModel updatePerfil = new PerfilModel();
                updatePerfil.nombrePerfil = NombreAlumno.Text;
                updatePerfil.edad = edadPerfil;
                updatePerfil.tamaño = tamañoSeleccionado;

                SpObjectTokenCategory otc = new SpObjectTokenCategory();
                otc.SetId("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Speech\\Voices");
                ISpeechObjectTokens tokenEnum = otc.EnumerateTokens();
                int nTockenCount = tokenEnum.Count;
                int i = 0;
                foreach (ISpeechObjectToken sot in tokenEnum)
                {
                    if (seleccionVoz.SelectedIndex == i)
                    {
                        updatePerfil.voz = sot.GetAttribute("name");
                    }
                    i++;

                }
                if (imagenNueva == false)
                {
                    Repository.Instance.updatePerfilSinFoto(updatePerfil);
                }
                else
                {
                    Repository.Instance.updatePerfilConFoto(updatePerfil, pathImagen);
                }
                SuccessDialog success = new SuccessDialog("Perfil Actualizado");
                success.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                success.Show();
                this.NavigationService.GoBack();
            }
        }
    }
}
