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

namespace WpfApp1.Pages.Dialogs
{
    /// <summary>
    /// Lógica de interacción para WarningTableroSinPortada.xaml
    /// </summary>
    public partial class WarningTableroSinPortada : Window
    {
        public WarningTableroSinPortada()
        {
            InitializeComponent();
        }
        public WarningTableroSinPortada(string mensaje)
        {
            InitializeComponent();
            mensajeAdvertencia.Text= mensaje;
        }
        private void closebtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
