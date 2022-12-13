using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WpfApp1.Pages.Player.TRutina
{
    /// <summary>
    /// Lógica de interacción para TRutina.xaml
    /// </summary>
    public partial class TRutina : Page
    {
        private static BitmapImage imagenBoton = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.playButtonBlanco);
        List<pictTablero> ListaPict = new List<pictTablero>();
        public TRutina()
        {
            InitializeComponent();
            //this.Resources["check"] = imagenBoton;
        }

        public TRutina(Board board)
        {
            InitializeComponent();
            ListaPict = board.pictTableros;
            //bind aqui con Tablero.source = ListaPict
            this.Resources["check"] = imagenBoton;
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            playButton.Visibility = Visibility.Collapsed;
        }
    }
}
