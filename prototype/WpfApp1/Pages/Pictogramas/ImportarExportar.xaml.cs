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
        private static BitmapImage volver = Repository.Instance.getImageFromResources(WpfApp1.Properties.Resources.arrowBlanca);
        List<Pictogram> listaTotalPict = new List<Pictogram>();
        string importpath="";
        List<Pictogram> importTempData = new List<Pictogram>();
        BackgroundWorker worker;
        CargandoDialog cargando;
        public ImportarExportar()
        {
            InitializeComponent();
            
        }
        public ImportarExportar(List<Pictogram>exportPict)
        {
            InitializeComponent();
            Repository.Instance.deleteTempData();
            listaTotalPict = exportPict;
            listviewExportar.ItemsSource = listaTotalPict;
            this.Resources["volver"] = volver;
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
                    pathtoSave = fbd.SelectedPath.ToString();
               
                    List<Pictogram> pictogramasExportar = new List<Pictogram>();
                    foreach (Pictogram pict in listviewExportar.SelectedItems)
                    {
                        pictogramasExportar.Add(pict);
                    }
                    if (!Directory.Exists(pathtoSave + "\\pictogramasGuardados"))
                    {
                        Directory.CreateDirectory(pathtoSave + "\\pictogramasGuardados");
                    }
                    if (!Directory.Exists(pathtoSave + "\\pictogramasGuardados\\sonidos"))
                    {
                        Directory.CreateDirectory(pathtoSave + "\\pictogramasGuardados\\sonidos");
                    }
                    Repository.Instance.exportPictogramas(pictogramasExportar,pathtoSave,"pictogramas");
                    SuccessDialog success = new SuccessDialog("Exportacion completa");
                    success.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    success.Show();
                }
                this.NavigationService.GoBack();
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
                listviewImportar.SelectAll();
            }
            else if (importSelectAllText.Text == "Deseleccionar Todos")
            {
                importSelectAllText.Text = "Seleccionar Todos";
                listviewImportar.UnselectAll();
            }
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
            if (File.Exists(importpath + "\\pictogramas.inb4"))
            {
                query = System.IO.File.ReadAllText(importpath + "\\pictogramas.inb4");
                Repository.Instance.importTempData(query);
            }
            importTempData = Repository.Instance.getAllTempPict();
            listviewImportar.ItemsSource = importTempData;
        }

        private void importarSeleccionados_Click(object sender, RoutedEventArgs e)
        {
            if (listviewImportar.SelectedItems.Count > 0)
            {
                int total_elementos = listviewImportar.SelectedItems.Count;
                List<Pictogram> pictSeleccionados = new List<Pictogram>();
                foreach (Pictogram pict in listviewImportar.SelectedItems)
                {
                    pictSeleccionados.Add(pict);
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
                        Repository.Instance.importPictograms(pictSeleccionados[i], importpath);
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
                MainPicrogramasPage.Instance.runActualizarLista();
                this.NavigationService.GoBack();
            }
        }
                           
private void volverMenu_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPicrogramasPage());
        }

        private void cancelarImportar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPicrogramasPage());
        }

        private void cancelarExportar_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPicrogramasPage());
        }

        private void GoToPictogramas(object sender, RoutedEventArgs e)
        {

        }
    }
}
