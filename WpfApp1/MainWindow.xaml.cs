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
using WpfApp1.Model;
namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            listNames();
        }

        private void ButtonAddName_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtName.Text) && !lstNames.Items.Contains(txtName.Text))
            {
                DatabaseInteraction.Instance.GuardarNombre(txtName.Text);// Guarda el nombre en la base de datos
                lstNames.Items.Clear();
                txtName.Clear();
                listNames();
            }
        }

        private void listNames()
        {
            List<Nombre> names = DatabaseInteraction.Instance.GetNombres();//Obtiene los nombres que estan en la base de datos
            foreach (Nombre nombre in names)
                lstNames.Items.Add(nombre.Name);

        }
    }
}
