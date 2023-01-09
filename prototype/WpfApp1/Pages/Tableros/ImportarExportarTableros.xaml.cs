using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WpfApp1.Pages.Dialogs;

namespace WpfApp1.Pages.Tableros
{
    /// <summary>
    /// Lógica de interacción para ImportarExportarTableros.xaml
    /// </summary>
    public partial class ImportarExportarTableros : Page
    {
        private static BitmapImage volver = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.arrowBlanca);
        List<Board> listaTablerosTotal = new List<Board>();
        string importpath = "";
        List<Board> importTempData = new List<Board>();
        BackgroundWorker worker;
        CargandoDialog cargando;
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
            this.Resources["volver"] = volver;
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
        private void exportSelectAll_Click(object sender, RoutedEventArgs e)
        {
            if (exportSelectAllText.Text == "Seleccionar Todos")
            {
                exportSelectAllText.Text = "Deseleccionar Todos";
                listViewTablerosE.SelectAll();
            }
            else if (exportSelectAllText.Text == "Deseleccionar Todos")
            {
                exportSelectAllText.Text = "Seleccionar Todos";
                listViewTablerosE.UnselectAll();
            }
        }

        private void importSelectAll_Click(object sender, RoutedEventArgs e)
        {
            if (importSelectAllText.Text == "Seleccionar Todos")
            {
                importSelectAllText.Text = "Deseleccionar Todos";
                listViewTableros.SelectAll();
            }
            else if (importSelectAllText.Text == "Deseleccionar Todos")
            {
                importSelectAllText.Text = "Seleccionar Todos";
                listViewTableros.UnselectAll();
            }
        }

        private void exportarSeleccionados_Click(object sender, RoutedEventArgs e)
        {
            if (listViewTablerosE.SelectedItems.Count > 0)
            {
                string pathtoSave = "";
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();
                if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    pathtoSave = fbd.SelectedPath.ToString();

                    List<Board> tablerosExportar = new List<Board>();
                    foreach (Board board in listViewTablerosE.SelectedItems)
                    {
                        tablerosExportar.Add(board);
                    }
                    if (!Directory.Exists(pathtoSave + "\\tablerosGuardados"))
                    {
                        Directory.CreateDirectory(pathtoSave + "\\tablerosGuardados");
                    }
                    if (!Directory.Exists(pathtoSave + "\\tablerosGuardados\\sonidos"))
                    {
                        Directory.CreateDirectory(pathtoSave + "\\tablerosGuardados\\sonidos");
                    }
                    Repository.Instance.exportTableros(tablerosExportar, pathtoSave);
                    SuccessDialog success = new SuccessDialog("Exportacion completa");
                    success.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    success.Show();
                }
            }
        }

        private void importarSeleccionados_Click(object sender, RoutedEventArgs e)
        {
            if (listViewTableros.SelectedItems.Count > 0)
            {
                int total_elementos = listViewTableros.SelectedItems.Count;
                List<Board> boardSeleccionados = new List<Board>();
                foreach (Board board in listViewTableros.SelectedItems)
                {
                    boardSeleccionados.Add(board);
                }
                cargando = new CargandoDialog();
                cargando.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                System.Windows.Threading.Dispatcher dispatcher = cargando.Dispatcher;

                worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;

                worker.DoWork += delegate (object s, DoWorkEventArgs args)
                {
                    for (int i = 0; i < total_elementos; i++)
                    {

                        if (worker.CancellationPending)
                        {
                            args.Cancel = true;
                            return;
                        }
                        Repository.Instance.importTableros(boardSeleccionados[i], importpath);
                        //System.Threading.Thread.Sleep(1);
                        worker.ReportProgress(Convert.ToInt32(((decimal)i / (decimal)total_elementos) * 100));
                    }

                };
                worker.ProgressChanged += delegate (object s, ProgressChangedEventArgs args)
                {
                    cargando.ProgressText = args.ProgressPercentage.ToString() + "%";
                };
                worker.RunWorkerCompleted += delegate (object s, RunWorkerCompletedEventArgs args)
                {
                    cargando.Close();
                };
                worker.RunWorkerAsync();
                cargando.ShowDialog();
                SuccessDialog success = new SuccessDialog("Importación completa");
                success.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                success.Show();
                this.NavigationService.Navigate(new MainTablerosPage());
            }
        }
        
    }
}
