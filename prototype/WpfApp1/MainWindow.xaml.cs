using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using WpfApp1.Assets;
using WpfApp1.Pages.First_Use;
using WpfApp1.Pages.Perfil;
using WpfApp1.Pages.Tableros;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            btnshow.Click += Btnshow_Click;
            btnclose.Click += Btnclose_Click;
            Repository.Instance.CreacionDeTablas();
            bool perfilExiste = Repository.Instance.verificarPerfil();
            if (perfilExiste)
            {
                Main.NavigationService.Navigate(new MenuPage());
            }
            else
            {
                LeftMenu.Visibility = Visibility.Collapsed;
                Main.NavigationService.Navigate(new FirstUse_NombreEdadFoto());
            }
            
        }
        private void Btnclose_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["CloseMenu"] as Storyboard;
            sb.Begin(LeftMenu);
        }
        private void Btnshow_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = Resources["OpenMenu"] as Storyboard;
            sb.Begin(LeftMenu);
        }


        private void btnshow_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnclose_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Pictogramas_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new MainPicrogramasPage();

        }

        private void Tableros_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new MainTablerosPage();
        }

        private void perfil_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new PerfilAlumno();
        }
    }
}
