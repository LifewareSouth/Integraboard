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
using WpfApp1.Pages.Dialogs;

namespace WpfApp1.Pages.Pictogramas
{
    /// <summary>
    /// Lógica de interacción para SelectImage.xaml
    /// </summary>
    public partial class SelectImage : Page
    {
        private static BitmapImage volver = Repository.Instance.getImageFromResources(Integraboard.Properties.Resources.arrowBlanca);
        List<ImagenModel> ListaImagenes = new List<ImagenModel>();
        List<ImagenModel> filteredImages = new List<ImagenModel>();
        public SelectImage()
        {
            InitializeComponent();
            ActualizarLista();
            this.Resources["volver"] = volver;
        }
        ImagenModel ImagenSeleccionada = new ImagenModel();
        private void AddImages_Click(object sender, RoutedEventArgs e)
        {
            //Aquí va lo que estaba en la otra page
            string pathImagen = null;
            Microsoft.Win32.OpenFileDialog ofdImage = new OpenFileDialog();
            ofdImage.Filter = "Imagenes (*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
            bool? response = ofdImage.ShowDialog();
            if (response == true)
            {
                foreach (string name in ofdImage.SafeFileNames)
                {
                    pathImagen = ofdImage.FileNames.Where(stringToCheck => stringToCheck.Contains(name)).First();
                }
                Repository.Instance.GuardarImagen(pathImagen);
                ActualizarLista();
            }
        }

        private void ActualizarLista()
        {
            ListaImagenes = Repository.Instance.getAllImages();
            if (ListaImagenes.Count > 0)
            {
                ListViewImages.ItemsSource = ListaImagenes;
            }
        }
        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void ListViewImages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewImages.SelectedValue != null)
            {
                btn_aceptar.IsEnabled = true;
                ImagenSeleccionada = ((ImagenModel)ListViewImages.SelectedItem);
            }
            else
            {
                btn_aceptar.IsEnabled = false;
            }
        }

        private void btn_aceptar_Click(object sender, RoutedEventArgs e)
        {
            string Mensaje = "Listo!";
            if (ListViewImages.SelectedValue != null)
            {
                SuccessDialog success = new SuccessDialog(Mensaje);
                success.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                success.Show();
                CrearPictos.Instance.ImagenFromDB(ImagenSeleccionada);
                this.NavigationService.GoBack();
            }
        }

        private void busqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<ImagenModel> filtro = new List<ImagenModel>();
            filteredImages = ListaImagenes;
            if (busqueda.Text != null && busqueda.Text != "")
            {
                foreach (ImagenModel imagenes in ListaImagenes)
                {
                    if (imagenes.Nombre.Contains(busqueda.Text))
                    {
                        filtro.Add(imagenes);
                    }
                }
                filteredImages = filtro;
            }
            ListViewImages.ItemsSource = filteredImages;
        }

        private void volverPictos_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
