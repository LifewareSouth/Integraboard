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
using WpfApp1.Pages.Dialogs;

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
            listviewExportar.ItemsSource = listaTotalPict;
        }

        private void exportarSeleccionados_Click(object sender, RoutedEventArgs e)
        {
            if (listviewExportar.SelectedItems.Count > 0)
            {
                string pathtoSave = "";
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();
                if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    pathtoSave = fbd.SelectedPath.ToString() + "\\";
                }
                List<Pictogram> pictogramasExportar = new List<Pictogram>();
                foreach (Pictogram pict in listviewExportar.SelectedItems)
                {
                    pictogramasExportar.Add(pict);
                }
                SuccessDialog success = new SuccessDialog("Exportacion completa");
                success.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                success.Show();
                Repository.Instance.exportPictogramas(pictogramasExportar,pathtoSave);
            }
        }

        private void exportSelectAll_Click(object sender, RoutedEventArgs e)
        {
            if (exportSelectAllText.Text == "Seleccionar Todos")
            {
                exportSelectAllText.Text = "Deseleccionar Todos";
                listviewExportar.SelectAll();
            }
            else if (exportSelectAllText.Text == "Deseleccionar Todos")
            {
                exportSelectAllText.Text = "Seleccionar Todos";
                listviewExportar.UnselectAll();
            }
        }

        private void importSelectAll_Click(object sender, RoutedEventArgs e)
        {
            if (importSelectAllText.Text == "Seleccionar Todos")
            {
                importSelectAllText.Text = "Deseleccionar Todos";
                listviewExportar.SelectAll();
            }
            else if (importSelectAllText.Text == "Deseleccionar Todos")
            {
                importSelectAllText.Text = "Seleccionar Todos";
                listviewExportar.UnselectAll();
            }
        }
    }
}
