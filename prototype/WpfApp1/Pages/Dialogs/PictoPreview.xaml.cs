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
using WpfApp1.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace WpfApp1.Pages.Dialogs
{
    /// <summary>
    /// Lógica de interacción para PictoPreview.xaml
    /// </summary>
    public partial class PictoPreview : Window
    {
        private static BitmapImage closeButton = Repository.Instance.getImageFromResources(Integraboard.Properties.Resources.close_cruz_invertida);
        public PictoPreview()
        {
            InitializeComponent();
            
        }

        public PictoPreview(Pictogram pict)
        { 
            InitializeComponent();
            pictoPreview.Source = pict.Imagen;
            bordePict.BorderBrush = pict.colorBorde;
            pictoTexto.Text = pict.Texto;
            this.Resources["closeButton"] = closeButton;
        }
        private void closePreview_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
