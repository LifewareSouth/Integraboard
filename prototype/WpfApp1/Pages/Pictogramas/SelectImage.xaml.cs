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
    /// Lógica de interacción para SelectImage.xaml
    /// </summary>
    public partial class SelectImage : Page
    {
        List<ImagenModel> ListaImagenes = new List<ImagenModel>();
        public SelectImage()
        {
            InitializeComponent();
            ListaImagenes = Repository.Instance.getAllImages();
        }

        private void AddImages_Click(object sender, RoutedEventArgs e)
        {
            //Aquí va lo que estaba en la otra page
            string pathImagen = null;
            Microsoft.Win32.OpenFileDialog ofdImage = new OpenFileDialog();
            ofdImage.Filter = "Imagenes (*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
            bool? response = ofdImage.ShowDialog();
            foreach (string name in ofdImage.SafeFileNames)
            {
                pathImagen = ofdImage.FileNames.Where(stringToCheck => stringToCheck.Contains(name)).First();
            }
            Repository.Instance.GuardarImagen(pathImagen);
            //PictogramImage.Source = LoadBitmapImage(pathImagen);
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
        

    }
}
