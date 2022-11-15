using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
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
        static bool reemplazandoPict = false;
        static bool addingPortada = false;
        static bool isEditing = false;
        private static List<etiquetaT> ListaEtiquetasTableros = new List<etiquetaT>();
        static Pictogram pictPortada = new Pictogram();
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
            ListaEtiquetasTableros = Repository.Instance.getAllEtiquetasTableros();

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
                    if(reemplazandoPict == true)
                    {
                        listaPictTablero.Remove(listaPictTablero.Where(x => x.pictograma.ID == CuadroSeleccionado.pictograma.ID).First());
                        reemplazandoPict = false;
                    }
                    AjustarTablero();
                    AgregandoPict = false;
                }
                else if (reemplazandoPict)
                {
                    reemplazandoPict = false;
                }
                else if (addingPortada)
                {
                    actualizarPictPortada();
                    addingPortada = false;
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
        public static BitmapImage LoadBitmapImage(string fileName)
        {
            try
            {
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                    return bitmapImage;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        private BindingList<pictTablero> vistas = new BindingList<pictTablero>();
        private void AjustarTablero()
        {
            int totalCuadros = rowCounter * columnsCounter;
            BindingList<pictTablero> tempVistas = new BindingList<pictTablero>();
            var baseImage = LoadBitmapImage("C:/Users/animu/Escritorio/add(3).png");
            for (int i = 0; i < rowCounter; i++)
            {
                for(int j = 0; j < columnsCounter; j++)
                {
                    pictTablero pictTab = new pictTablero();
                    if (listaPictTablero.Any(x => (x.x == j) && (x.y == i)))
                    {
                        pictTab = listaPictTablero.Where(x => (x.x == j) && (x.y == i)).First();
                    }
                    else
                    {
                        
                        Pictogram tempPictograma = new Pictogram();
                        tempPictograma.colorBorde = new SolidColorBrush(Colors.Cornsilk);
                        tempPictograma.Imagen = baseImage;
                        tempPictograma.Texto = "Asignar";
                        pictTab.pictograma = tempPictograma;

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
                List<Pictogram> pictAgregados = new List<Pictogram>();
                foreach(pictTablero pt in listaPictTablero)
                {
                    pictAgregados.Add(pt.pictograma);
                }
                this.NavigationService.Navigate(new ListadoPictogramas(pictAgregados));
            }
            else
            {
                reemplazandoPict = true;
                CuadroSeleccionado = (pictTablero)Tablero.SelectedValue;
                List<Pictogram> pictAgregados = new List<Pictogram>();
                foreach (pictTablero pt in listaPictTablero)
                {
                    if(pt.ID != CuadroSeleccionado.pictograma.ID)
                    {
                        pictAgregados.Add(pt.pictograma);
                    }
                }
                this.NavigationService.Navigate(new ListadoPictogramas(pictAgregados));
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
        private bool validateConditionsToSave()
        {
            bool valido = true;
            if(nombreTablero.Text == null)
            {
                valido = false;
            }
            if (listaPictTablero.Count ==0)
            {
                valido = false;
            }
            if(pictPortada.ID == 0)
            {
                valido=false;
            }
            return valido;
        }
        private void PictoRepresent_DoubleClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ListadoPictogramas());
        }
        private void guardarTablero_Click(object sender, RoutedEventArgs e)
        {
            if (validateConditionsToSave())
            {
                int idBoard;
                List<int> idsTags = new List<int>();
                Board newBoard = new Board();
                newBoard.nombreTablero = nombreTablero.Text;
                newBoard.tipo = comboBoxTipo.SelectedItem.ToString();
                newBoard.filas = rowCounter;
                newBoard.columnas = columnsCounter;
                newBoard.idPictPortada = pictPortada.ID;
                idBoard = Repository.Instance.crearTablero(newBoard);
                savePictBoard(idBoard);
                if (!isEditing)
                {
                    if (String.IsNullOrWhiteSpace(Tags.Text) == false)
                    {
                        idsTags = idTags(Tags.Text);
                        Repository.Instance.AsociarEtiquetasTablero(idsTags, idBoard, true);
                    }
                }



                this.NavigationService.GoBack();
            }
        }
        private List<int> idTags(string textoEtiquetas)
        {
            List<int> idsTags = new List<int>();
            List<string> etiquetasPict = new List<string>();
            string[] conjuntoEtiquetas = textoEtiquetas.Split(",");
            foreach (string tag in conjuntoEtiquetas)
            {
                etiquetasPict.Add(tag);
            }
            foreach (string tag in etiquetasPict)
            {
                //EN CASO DE NO EXISTIR LA ETIQUETA PREVIEMENTE
                if (!ListaEtiquetasTableros.Any(x => x.NombreEtiqueta == tag))
                {
                    Repository.Instance.InsertEtiquetaT(tag);
                    idsTags.Add(Repository.Instance.GetIdEtiquetaT(tag));
                }
                else
                {
                    idsTags.Add(ListaEtiquetasTableros.Where(x => x.NombreEtiqueta == tag).Select(ListIdPict => ListIdPict.ID).First());
                }
            }
            return idsTags;
        }
        public void addPictPortada(Pictogram pictogramaPortada)
        {
            pictPortada = pictogramaPortada;
            addingPortada = true;
        }

        private void QuitarPictograma_Click(object sender, RoutedEventArgs e)
        {
            if (Tablero.SelectedItem != null)
            {
                var seleccion = (pictTablero)Tablero.SelectedValue;
                if (seleccion.idPictograma != 0 )
                {
                    listaPictTablero.Remove(listaPictTablero.Where(x => x.pictograma.ID == seleccion.pictograma.ID).First());
                    AjustarTablero();
                }
            }
        }
        private void savePictBoard(int idTablero)
        {
            if (isEditing)
            {
                //Cuando se edita el tablero
            }
            else
            {
                foreach (pictTablero pt in listaPictTablero)
                {
                    if(pt.x < columnsCounter && pt.y < rowCounter)
                    {
                        Repository.Instance.EnlazarPictBoard(idTablero, pt.pictograma.ID, pt.x, pt.y);
                    }
                }
            }

        }

        public void actualizarPictPortada()
        {
            // ANITA AQUI ASIGNAR COSAS DE PICTOGRAMA DE PORTADA (pictPortada) 

        }
    }
}
