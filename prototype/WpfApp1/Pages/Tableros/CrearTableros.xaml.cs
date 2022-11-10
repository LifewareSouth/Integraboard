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
        bool _navigationServiceAssigned = false;
        static pictTablero CuadroSeleccionado = new pictTablero();
        static Collection<pictTablero> listaPictTablero = new Collection<pictTablero>();
        static bool AgregandoPict = false;
        private static readonly CrearTableros instance = new CrearTableros();
        public static CrearTableros Instance
        {
            get
            {
                return instance;
            }
        }
        public CrearTableros()
        {
            
            InitializeComponent();
            listaPictTablero = new Collection<pictTablero>();
            tiposTablero();
            AjustarTablero();
            comboBoxTipo.Text = "Comunicación";
        }
        private void page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_navigationServiceAssigned == false)
            {
                NavigationService.Navigating += NavigationService_Navigating;
                _navigationServiceAssigned = true;
            }
        }
        void NavigationService_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                if (AgregandoPict)
                {
                    AjustarTablero();
                    AgregandoPict = false;
                }
            }
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
        private BindingList<pictTablero> vistas = new BindingList<pictTablero>();
        private void AjustarTablero()
        {
            int totalCuadros = rowCounter * columnsCounter;
            BindingList<pictTablero> tempVistas = new BindingList<pictTablero>();
            for (int i = 0; i < rowCounter; i++)
            {
                for(int j = 0; j < columnsCounter; j++)
                {
                    pictTablero pictTab = new pictTablero();
                    if (listaPictTablero.Any(x => (x.x == i) && (x.y == j)))
                    {
                        pictTab = listaPictTablero.Where(x => (x.x == i) && (x.y == j)).First();
                    }
                    pictTab.x = j;
                    pictTab.y = i;
                    tempVistas.Add(pictTab);

                }
            }
            vistas = tempVistas;
            Tablero.ItemsSource = vistas;
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

        private void Tablero_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var asd = Tablero.SelectedValue;
        }


        private void doubleclick_addpictogram(object sender, RoutedEventArgs e)
        {
            var seleccion = (pictTablero)Tablero.SelectedValue;
            if (seleccion.idPictograma == 0 || seleccion.idPictograma ==null)
            {
                CuadroSeleccionado = (pictTablero)Tablero.SelectedValue;
                this.NavigationService.Navigate(new ListadoPictogramas());
            }
            
        } 
        public void addPict(List<Pictogram> listAddPict)
        {

            foreach(Pictogram p in listAddPict)
            {
                
                pictTablero pictTab = new pictTablero();
                if (p == listAddPict.First())
                {
                    pictTab.idPictograma = p.ID;
                    pictTab.pictograma = p;
                    pictTab.x = CuadroSeleccionado.x;
                    pictTab.y = CuadroSeleccionado.y;
                    listaPictTablero.Add(pictTab);
                }
                else
                {
                    bool siguiente = false;
                    for (int i = 0; i < rowCounter; i++)
                    {
                        for (int j = 0; j < columnsCounter; j++)
                        {
                            if (!listaPictTablero.Any(x => (x.x == j)&& (x.y == i) ))
                            {
                                if (!listaPictTablero.Any(x => (x.idPictograma == pictTab.ID)))
                                {
                                    pictTab.idPictograma = p.ID;
                                    pictTab.pictograma = p;
                                    pictTab.x = j;
                                    pictTab.y = i;
                                    listaPictTablero.Add(pictTab);
                                    siguiente = true;
                                }
                                
                            }
                            if (siguiente)
                                break;

                        }
                        if (siguiente)
                            break;
                    }
                    
                }
                
            }
            AgregandoPict = true;
        }


    }
}
