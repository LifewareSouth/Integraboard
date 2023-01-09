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
using System.Windows.Shapes;
using WpfApp1.Assets;

namespace WpfApp1.Pages.Dialogs
{
    /// <summary>
    /// Lógica de interacción para WarningTiempo.xaml
    /// </summary>
    public partial class WarningTiempo : Window
    {
        private static BitmapImage cronometro = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.stopwatch);
        private static BitmapImage closeButton = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.close_cruz_invertida);
        public WarningTiempo()
        {
            InitializeComponent();
            this.Resources["cronometro"] = cronometro;
            this.Resources["closeButton"] = closeButton;
        }
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }

        private void closebtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}
