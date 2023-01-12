using System.Windows;
using System.Windows.Forms;

namespace WpfApp1.Pages.Dialogs
{
    /// <summary>
    /// Lógica de interacción para update.xaml
    /// </summary>
    public partial class update : Window
    {
        public update()
        {
            InitializeComponent();
        }

        public update(string msj)
        {
            InitializeComponent();
            Mensaje.Text = msj;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
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
