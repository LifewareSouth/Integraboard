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
    /// Lógica de interacción para CargandoDialog.xaml
    /// </summary>
    public partial class CargandoDialog : Window
    {
        public CargandoDialog()
        {
            InitializeComponent();
        }
        public string ProgressText
        {
            set
            {
                this.lblProgress.Content = value;
            }
        }

    }
}
