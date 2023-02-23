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
    public partial class DeleteDialogWin : Window
    {
        private static BitmapImage closeButton = Repository.Instance.getImageFromResources(Integraboard.Properties.Resources.close_cruz_invertida);
        public DeleteDialogWin()
        {
            InitializeComponent();
            this.Resources["closeButton"] = closeButton;
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
