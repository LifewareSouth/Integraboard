using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
using WpfApp1.UserControls;

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
            AjustarTablero();
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
                UniformGrid foundUniformGrid = FindVisualChild<UniformGrid>(Tablero);
                foundUniformGrid.Rows = rowCounter;
                AjustarTablero();
            }
        }

        private void MoreCols_Click(object sender, RoutedEventArgs e)
        {
            if (_columns < 7)
            {
                columnsCounter++;
                UniformGrid foundUniformGrid = FindVisualChild<UniformGrid>(Tablero);
                foundUniformGrid.Columns = columnsCounter;
                AjustarTablero();
            }
        }

        private void LessCols_Click(object sender, RoutedEventArgs e)
        {
            if (_columns > 1)
            {
                columnsCounter--;
                UniformGrid foundUniformGrid = FindVisualChild<UniformGrid>(Tablero);
                foundUniformGrid.Columns = columnsCounter;
                AjustarTablero();
            }
        }

        private void MoreRows_Click(object sender, RoutedEventArgs e)
        {
            if (_rows < 7)
            {
                rowCounter++;
                UniformGrid foundUniformGrid = FindVisualChild<UniformGrid>(Tablero);
                foundUniformGrid.Rows = rowCounter;
                AjustarTablero();
            }
        }
        private BindingList<AddPictogram> views =
    new BindingList<AddPictogram>();
        public AddPictogram addpict { get; set; }
        private void AjustarTablero()
        {
            
            
            //ItemsPanelTemplate itemsPanelTemplate = new ItemsPanelTemplate();
            //Template

            addpict = new AddPictogram();
            Grid grid = FindVisualChild<Grid>(Tablero);
            int totalCuadros = rowCounter * columnsCounter;
            views.Clear();
            BindingList<AddPictogram> tempList = new BindingList<AddPictogram>();
            for (int i = 0; i < totalCuadros; i++)
            {
                tempList.Add(new AddPictogram());
            }
            for (int i = 0; i < rowCounter; i++)

            {
                //testTablero2.RowDefinitions.Add(new RowDefinition());
               

            }

            for (int j = 0; j < columnsCounter; j++)
            {
                //grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            views = tempList;
            Tablero.ItemsSource = views;
        }
        private T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                    return childOfChild;
            }
            return null;
        }

        private void doubleclick_addpictogram(object sender, RoutedEventArgs e)
        {

            this.NavigationService.Navigate(new ListadoPictogramas());
        }

        private void testEvent(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new ListadoPictogramas());
        }

        private void t1(object sender, RoutedEventArgs e)
        {

            
        }
    }
}
