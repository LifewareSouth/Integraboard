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
using WpfApp1.Model;

namespace WpfApp1.Pages.Pictogramas
{
    /// <summary>
    /// Lógica de interacción para ImportarExportar.xaml
    /// </summary>
    public partial class ImportarExportar : Page
    {
        List<Pictogram> listaTotalPict = new List<Pictogram>();
        public ImportarExportar()
        {
            InitializeComponent();
        }
        public ImportarExportar(List<Pictogram>exportPict)
        {
            InitializeComponent();
            listaTotalPict = exportPict;
            listviewImportar.ItemsSource = listaTotalPict;
        }


    }
}
