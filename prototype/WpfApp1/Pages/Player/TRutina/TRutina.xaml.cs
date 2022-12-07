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

namespace WpfApp1.Pages.Player.TRutina
{
    /// <summary>
    /// Lógica de interacción para TRutina.xaml
    /// </summary>
    public partial class TRutina : Page
    {
        private static BitmapImage imagenBoton = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.checkMark);
        public TRutina()
        {
            InitializeComponent();
            this.Resources["check"] = imagenBoton;
        }
    }
}
