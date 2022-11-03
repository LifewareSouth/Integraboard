using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
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
using WpfApp1.Pages.Pictogramas;

namespace WpfApp1.Pages.Tableros
{
    /// <summary>
    /// Lógica de interacción para CrearTableros.xaml
    /// </summary>
    public partial class CrearTableros : Page
    {
        public CrearTableros()
        {
            InitializeComponent();
            tiposTablero();
            comboBoxTipo.Text = "Comunicación";
        }
        private void tiposTablero()
        {
            foreach (Board.TableroTipo foo in Enum.GetValues(typeof(Board.TableroTipo)))
            {
                object aux = comboBoxTipo.Items.Add(foo.ToString());
            }
        }

        int _rows = 4, _columns = 4;
        public int rowCounter
        {
            set
            {
                _rows = value;
                rows.Content= rowCounter.ToString();

            }
            get
            {
                return _rows;
            }
        }
        public int columnsCounter
        {
            set
            {
                _columns = value;
                Cols.Content = columnsCounter.ToString();

            }
            get
            {
                return _columns;
            }
        }

        private void goToCrearPictos(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CrearPictos());
        }

        private void goToTableros(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainTablerosPage());
        }

        private void LessRows_Click(object sender, RoutedEventArgs e)
        {
            if (_rows > 1)
            {
                rowCounter--;
            }
        }

        private void MoreCols_Click(object sender, RoutedEventArgs e)
        {
            if (_columns < 7)
            {
                columnsCounter++;
            }
        }

        private void LessCols_Click(object sender, RoutedEventArgs e)
        {
            if (_columns > 1)
            {
                columnsCounter--;
            }
        }

        private void MoreRows_Click(object sender, RoutedEventArgs e)
        {
            if (_rows < 7)
            {
                rowCounter++;  
            }
        }
    }
}
