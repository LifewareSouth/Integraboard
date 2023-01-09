using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfApp1.Pages.Tableros
{
    /// <summary>
    /// Lógica de interacción para ImportarExportarTableros.xaml
    /// </summary>
    public partial class ImportarExportarTableros : Page
    {
        List<Board> listaTablerosTotal = new List<Board>();
        string importpath = "";
        List<Board> importTempData = new List<Board>();
        public ImportarExportarTableros()
        {
            InitializeComponent();
        }
        public ImportarExportarTableros(List<Board> exportTableros)
        {
            InitializeComponent();
            Repository.Instance.deleteTempData();
            listaTablerosTotal = exportTableros;
            listViewTablerosE.ItemsSource = listaTablerosTotal;
        }

        private void importSeleccionarCarpeta_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = fbd.ShowDialog();
            string query = "";
            Repository.Instance.deleteTempData();
            if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                importpath = fbd.SelectedPath.ToString();
            }
            if (File.Exists(importpath + "\\tableros.inb4"))
            {
                query = System.IO.File.ReadAllText(importpath + "\\tableros.inb4");
                Repository.Instance.importTempData(query);
            }
            importTempData = Repository.Instance.getAllTempBoards();
            listViewTableros.ItemsSource = importTempData;
        }
    }
}
