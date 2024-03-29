﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using WpfApp1.Pages.Dialogs;
using WpfApp1.Pages.Pictogramas;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace WpfApp1.Pages.Tableros
{
    /// <summary>
    /// Lógica de interacción para TableroRutina.xaml
    /// </summary>
    public partial class TableroRutina : Page
    {
        private static BitmapImage volver = Repository.Instance.getImageFromResources(Integraboard.Properties.Resources.arrowBlanca);
        bool _navigationServiceAssigned = false;
        static pictTablero CuadroSeleccionado = new pictTablero();
        static Collection<pictTablero> listaPictTablero = new Collection<pictTablero>();
        static bool AgregandoPict = false;
        static bool reemplazandoPict = false;
        static bool addingPortada = false;
        static bool isEditing = false;
        static bool pictogramaEditado = false;
        private static List<etiquetaT> ListaEtiquetasTableros = new List<etiquetaT>();
        static Pictogram pictPortada = new Pictogram();
        private static readonly TableroRutina instance = new TableroRutina();
        static Board boardEditable = new Board();
        private static BitmapImage imagenAddPictograma = Repository.Instance.getImageFromResources(Integraboard.Properties.Resources.add3);

        public static TableroRutina Instance
        {
            get
            {
                return instance;
            }
        }
        public TableroRutina()
        {
            InitializeComponent();
            columnsCounter = 4;
            isEditing = false;
            listaPictTablero = new Collection<pictTablero>();
            //tiposTablero();
            pictPortada = Repository.Instance.defaultPict();
            AjustarTablero();
            //comboBoxTipo.Text = "Comunicación";
            ListaEtiquetasTableros = Repository.Instance.getAllEtiquetasTableros();
            this.Resources["volver"] = volver;
        }

        public TableroRutina(Board boardEdit)
        {

            InitializeComponent();
            this.Resources["volver"] = volver;
            isEditing = true;
            listaPictTablero = new Collection<pictTablero>();
            //tiposTablero();
            ListaEtiquetasTableros = Repository.Instance.getAllEtiquetasTableros();
            boardEditable = boardEdit;
            if(boardEdit.conTiempo == "Si")
            {
                checkTiempo.IsChecked = true;
            }
            else if(boardEdit.conTiempo == "No")
            {
                checkTiempo.IsChecked = false;
            }
            //comboBoxTipo.Text = boardEdit.tipo;
            nombreTablero.Text = boardEdit.nombreTablero;
            pictPortada = boardEdit.pictPortada;
            foreach (pictTablero pt in boardEdit.pictTableros)
            {
                listaPictTablero.Add(pt);
            }
            rowCounter = boardEdit.filas;
            columnsCounter = boardEdit.columnas;
            foreach (etiquetaT et in boardEdit.ListaEtiquetasTableros)
            {
                if (et == boardEdit.ListaEtiquetasTableros.First())
                {
                    Tags.Text = et.NombreEtiqueta;
                }
                else
                {
                    Tags.Text = Tags.Text + "," + et.NombreEtiqueta;
                }
            }
            AjustarTablero();

        }
        private void page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_navigationServiceAssigned == false)
            {
                NavigationService.Navigating += NavigationService_Navigating;
                _navigationServiceAssigned = true;
            }
            ajustarTamano();
            imagenPictPortada();
        }
        void NavigationService_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                if (AgregandoPict)
                {
                    if (reemplazandoPict == true)
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
                else if (pictogramaEditado)
                {
                    pictogramaEditado = false;
                }
                AjustarTablero();
            }
        }

        private void ajustarTamano()
        {
            UniformGrid foundUniformGrid = FindVisualChild<UniformGrid>(Tablero);
            foundUniformGrid.Columns = columnsCounter;
            foundUniformGrid.Rows = rowCounter;
        }
        private void imagenPictPortada()
        {
            System.Windows.Controls.Image imagenPortada = FindVisualChild<System.Windows.Controls.Image>(PictoRepresent);
            imagenPortada.Source = pictPortada.Imagen;
        }
        /*private void tiposTablero()
        {
            foreach (Board.TableroTipo foo in Enum.GetValues(typeof(Board.TableroTipo)))
            {
                object aux = comboBoxTipo.Items.Add(foo.ToString());
            }
        }*/

        static int _rows = 1, _columns = 0;
        public int rowCounter
        {
            set
            {
                _rows = value;

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

        private BindingList<pictTablero> vistas = new BindingList<pictTablero>();
        private void AjustarTablero()
        {
            int totalCuadros = rowCounter * columnsCounter;
            BindingList<pictTablero> tempVistas = new BindingList<pictTablero>();
            for (int i = 0; i < rowCounter; i++)
            {
                for (int j = 0; j < columnsCounter; j++)
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
                        tempPictograma.Imagen = imagenAddPictograma;
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
        
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    private void Tablero_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Tablero.SelectedItem != null)
            {
                if (((pictTablero)Tablero.SelectedItem).idPictograma != 0)
                {
                    timeMinutos.IsEnabled = IsEnabled;
                    timeSegundos.IsEnabled = IsEnabled;
                    CuadroSeleccionado = (pictTablero)Tablero.SelectedItem;
                    int tiempoTotal = int.Parse(CuadroSeleccionado.tiempo);
                    int minutos = tiempoTotal / 60;
                    int segundos =tiempoTotal -(minutos * 60);
                    timeMinutos.Text = minutos.ToString();
                    timeSegundos.Text = segundos.ToString();
                    aplicarTiempo.IsEnabled = false;
                }
                else
                {
                    timeSegundos.IsEnabled = false;
                    timeMinutos.IsEnabled = false;
                }

            }
        }


        private void doubleclick_addpictogram(object sender, RoutedEventArgs e)
        {
            var seleccion = (pictTablero)Tablero.SelectedValue;
            if (seleccion.idPictograma == 0 || seleccion.idPictograma == null)
            {
                CuadroSeleccionado = (pictTablero)Tablero.SelectedValue;
                List<Pictogram> pictAgregados = new List<Pictogram>();
                foreach (pictTablero pt in listaPictTablero)
                {
                    pictAgregados.Add(pt.pictograma);
                }
                this.NavigationService.Navigate(new ListadoPictogramas(pictAgregados, "Rutina"));
            }
            else
            {
                reemplazandoPict = true;
                CuadroSeleccionado = (pictTablero)Tablero.SelectedValue;
                List<Pictogram> pictAgregados = new List<Pictogram>();
                foreach (pictTablero pt in listaPictTablero)
                {
                    if (pt.ID != CuadroSeleccionado.pictograma.ID)
                    {
                        pictAgregados.Add(pt.pictograma);
                    }
                }
                this.NavigationService.Navigate(new ListadoPictogramas(pictAgregados, "Rutina"));
            }

        }
        public void addPict(List<Pictogram> listAddPict)
        {

            foreach (Pictogram p in listAddPict)
            {

                pictTablero pictTab = new pictTablero();
                if (p == listAddPict.First())
                {
                    pictTab.idPictograma = p.ID;
                    pictTab.pictograma = p;
                    pictTab.x = CuadroSeleccionado.x;
                    pictTab.y = CuadroSeleccionado.y;
                    pictTab.tiempo = "30";
                    listaPictTablero.Add(pictTab);
                }
                else
                {
                    bool siguiente = false;
                    for (int i = 0; i < rowCounter; i++)
                    {
                        for (int j = 0; j < columnsCounter; j++)
                        {
                            if (!listaPictTablero.Any(x => (x.x == j) && (x.y == i)))
                            {
                                if (!listaPictTablero.Any(x => (x.idPictograma == pictTab.ID)))
                                {
                                    pictTab.idPictograma = p.ID;
                                    pictTab.pictograma = p;
                                    pictTab.x = j;
                                    pictTab.y = i;
                                    pictTab.tiempo = "30";
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
            if (String.IsNullOrWhiteSpace(nombreTablero.Text) == true)
            {
                valido = false;
            }
            if (listaPictTablero.Count == 0)
            {
                valido = false;
            }
            if (pictPortada.ID == 0)
            {
                valido = false;
            }
            for(int i = 0; i < columnsCounter; i++)
            {
                if(listaPictTablero.Where(x=>x.x == i).Count() == 0)
                {
                    valido = false;
                }
                else
                {
                    if((listaPictTablero.Where(x => x.x == i).First()).tiempo == "0")
                    {
                        valido = false;
                    }
                }
            }
            return valido;
        }
        private void PictoRepresent_DoubleClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ListadoPictogramas("Rutina"));
        }
        private void guardarTablero_Click(object sender, RoutedEventArgs e)
        {
            if (validateConditionsToSave())
            {
                int idBoard;
                string mensaje = "";
                List<int> idsTags = new List<int>();
                Board newBoard = new Board();
                newBoard.nombreTablero = nombreTablero.Text;
                newBoard.tipo = "Rutina";
                newBoard.filas = rowCounter;
                newBoard.columnas = columnsCounter;
                newBoard.idPictPortada = pictPortada.ID;
                if (checkTiempo.IsChecked==true)
                {
                    newBoard.conTiempo = "Si";
                }
                else
                {
                    newBoard.conTiempo = "No";
                }
                if (!isEditing)
                {
                    idBoard = Repository.Instance.crearTablero(newBoard);

                    if (String.IsNullOrWhiteSpace(Tags.Text) == false)
                    {
                        idsTags = idTags(Tags.Text);
                        Repository.Instance.AsociarEtiquetasTablero(idsTags, idBoard, true);
                    }
                    savePictBoard(idBoard);
                    mensaje = "Tablero Creado";
                }
                else//EN CASO DE EDITAR TABLERO
                {
                    newBoard.ID = boardEditable.ID;
                    Repository.Instance.updateTablero(newBoard);
                    if (String.IsNullOrWhiteSpace(Tags.Text) == false)
                    {
                        idsTags = idTags(Tags.Text);
                        Repository.Instance.AsociarEtiquetasTablero(idsTags, newBoard.ID, false);
                    }
                    savePictBoard(newBoard.ID);
                    mensaje = "Actualización correcta";
                }

                SuccessDialog success = new SuccessDialog(mensaje);
                success.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                success.Show();
                MainTablerosPage.Instance.runActualizarLista();
                this.NavigationService.Navigate(new MainTablerosPage());
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
            pictPortada = pictogramaPortada; //este es el que se debe pasar al boton de agregar representante
            addingPortada = true;
        }

        private void QuitarPictograma_Click(object sender, RoutedEventArgs e)
        {
            if (Tablero.SelectedItem != null)
            {
                var seleccion = (pictTablero)Tablero.SelectedValue;
                if (seleccion.idPictograma != 0)
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
                foreach (pictTablero pt in listaPictTablero)
                {
                    if (pt.x < columnsCounter && pt.y < rowCounter)
                    {
                        //EN EL CASO DE SER UN PICTOGRAMA NO AÑADIDO ANTERIORMENTE AL TABLERO
                        if (!boardEditable.pictTableros.Any(x => x.idPictograma == pt.idPictograma))
                        {
                            Repository.Instance.EnlazarPictBoard(idTablero, pt.pictograma.ID, pt.x, pt.y, pt.tiempo);
                        }
                        else
                        {
                            Repository.Instance.updatePictTablero(idTablero, pt.pictograma.ID, pt.x, pt.y,pt.tiempo);
                        }
                    }
                }
                //Lista para eliminar asociacion de pictogramas que ya no estan en el tablero
                List<pictTablero> pictTabEliminar = new List<pictTablero>();
                pictTabEliminar = boardEditable.pictTableros.Where(x => (!listaPictTablero.Any(y => y.idPictograma == x.idPictograma))).ToList();
                foreach (pictTablero deletePT in pictTabEliminar)
                {
                    Repository.Instance.quitarAsociacionPictTablero(idTablero, deletePT.idPictograma);
                }
            }
            else
            {
                //AL CREAR UN TABLERO SE CREAN TODAS LAS ASOCIACIONES DE LOS PICTOGRAMAS CON EL TABLERO
                foreach (pictTablero pt in listaPictTablero)
                {
                    if (pt.x < columnsCounter && pt.y < rowCounter)
                    {
                        Repository.Instance.EnlazarPictBoard(idTablero, pt.pictograma.ID, pt.x, pt.y,pt.tiempo);
                    }
                }
            }

        }

        private void EditarPictograma_Click(object sender, RoutedEventArgs e)
        {

            if (Tablero.SelectedItem != null)
            {
                var seleccion = (pictTablero)Tablero.SelectedValue;
                if (seleccion.idPictograma != 0)
                {
                    this.NavigationService.Navigate(new CrearPictos(seleccion.pictograma, true, "Comunicación"));
                    AjustarTablero();
                }
            }
        }
        public void actualizarPictogramaEditado()
        {
            listaPictTablero.Where(x => x.idPictograma == CuadroSeleccionado.idPictograma).First().pictograma = Repository.Instance.getOnePictogram(CuadroSeleccionado.idPictograma);
            pictogramaEditado = true;
        }

        public void actualizarPictPortada()
        {
            // ANITA AQUI ASIGNAR COSAS DE PICTOGRAMA DE PORTADA (pictPortada) 

        }
        private void Tablero_DragLeave(object sender, System.Windows.DragEventArgs e)
        {

        }
        private void Tablero_DragOver(object sender, System.Windows.DragEventArgs e)
        {

        }
        private void TodoItem_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed &&
                sender is FrameworkElement frameworkElement)
            {
                object todoItem = frameworkElement.DataContext;
                //DragDrop.DoDragDrop(Tablero, Tablero, System.Windows.DragDropEffects.Move);
                System.Windows.DragDropEffects dragDropResult = DragDrop.DoDragDrop(frameworkElement,
                   new System.Windows.DataObject(System.Windows.DataFormats.Serializable, todoItem),
                   System.Windows.DragDropEffects.Move);

                intercambiarPos();
            }
        }
        private void TodoItem_DragOver(object sender, System.Windows.DragEventArgs e)
        {

            if (sender is FrameworkElement element)
            {
                TargetTodoRutinaItem = element.DataContext;
                InsertedTodoRutinaItem = e.Data.GetData(System.Windows.DataFormats.Serializable);

                TodoItemInsertedRutinaCommand?.Execute(null);
            }

        }
        public static readonly DependencyProperty TodoItemInsertedRutinaCommandProperty =
            DependencyProperty.Register("TodoItemInsertedRutinaCommand", typeof(ICommand), typeof(CrearTableros),
                new PropertyMetadata(null));

        public ICommand TodoItemInsertedRutinaCommand
        {
            get { return (ICommand)GetValue(TodoItemInsertedRutinaCommandProperty); }
            set { SetValue(TodoItemInsertedRutinaCommandProperty, value); }
        }
        public static readonly DependencyProperty TargetTodoRutinaItemProperty =
            DependencyProperty.Register("TargetTodoRutinaItem", typeof(object), typeof(CrearTableros),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object TargetTodoRutinaItem
        {
            get { return (object)GetValue(TargetTodoRutinaItemProperty); }
            set { SetValue(TargetTodoRutinaItemProperty, value); }
        }
        public static readonly DependencyProperty InsertedTodoRutinaItemProperty =
            DependencyProperty.Register("InsertedTodoRutinaItem", typeof(object), typeof(CrearTableros),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object InsertedTodoRutinaItem
        {
            get { return (object)GetValue(InsertedTodoRutinaItemProperty); }
            set { SetValue(InsertedTodoRutinaItemProperty, value); }
        }

        private void checkTiempo_Checked(object sender, RoutedEventArgs e)
        {
            if(checkTiempo.IsChecked == true)
            {
                panelTiempo.Visibility = Visibility.Visible;
            }
        }
        private void checkTiempo_Unchecked(object sender, RoutedEventArgs e)
        {
            if (checkTiempo.IsChecked == false)
            {
                panelTiempo.Visibility = Visibility.Hidden;
            }
        }
        private void timeMinutos_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Tablero.SelectedItem != null)
            {
                aplicarTiempo.IsEnabled = true;
            }
        }
        private void timeSegundos_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Tablero.SelectedItem != null)
            {
                aplicarTiempo.IsEnabled = true;
            }
        }
        private void aplicarTiempo_Click(object sender, RoutedEventArgs e)
        {
            if (Tablero.SelectedItem != null)
            {
                if (((pictTablero)Tablero.SelectedItem).idPictograma != 0)
                {
                    int segundos, minutos;
                    if (timeSegundos.Text=="")
                    {
                        segundos = 0;
                    }
                    else
                    {
                        segundos = int.Parse(timeSegundos.Text);
                    }

                    if (timeMinutos.Text == "")
                    {
                        minutos = 0;
                    }
                    else
                    {
                        minutos = int.Parse(timeMinutos.Text);
                    }
                    int tiempoTotal;
                    if (minutos != 0)
                    {
                        tiempoTotal = segundos + (minutos * 60);
                    }
                    else
                    {
                        tiempoTotal = segundos;
                    }
                    listaPictTablero.Where(x => x.idPictograma == CuadroSeleccionado.idPictograma).First().tiempo = tiempoTotal.ToString();
                }
                aplicarTiempo.IsEnabled = false;
            }
        }
        private void intercambiarPos()
        {
            pictTablero target = (pictTablero)TargetTodoRutinaItem;
            pictTablero pictToMove = (pictTablero)InsertedTodoRutinaItem;
            try
            {
                if (Tablero.SelectedItems.Count > 0)
                {

                
            
                    if(((pictTablero)Tablero.SelectedItem).idPictograma!=0)
                    {
                        if (pictToMove != null)
                        {

                            if (pictToMove.idPictograma != 0)
                            {
                                if (target.idPictograma == 0)
                                {
                                    listaPictTablero.Where(x => x.idPictograma == pictToMove.idPictograma).First().x = target.x;
                                    listaPictTablero.Where(x => x.idPictograma == pictToMove.idPictograma).First().y = target.y;
                                }
                                else
                                {
                                    int tempx = pictToMove.x;
                                    int tempy = pictToMove.y;
                                    listaPictTablero.Where(x => x.idPictograma == pictToMove.idPictograma).First().x = target.x;
                                    listaPictTablero.Where(x => x.idPictograma == pictToMove.idPictograma).First().y = target.y;
                                    listaPictTablero.Where(x => x.idPictograma == target.idPictograma).First().x = tempx;
                                    listaPictTablero.Where(x => x.idPictograma == target.idPictograma).First().y = tempy;
                                }

                                AjustarTablero();
                            }
                        }
                    }
                }
            }
            catch
            {
                this.NavigationService.GoBack();
            }
        }
    }
}

