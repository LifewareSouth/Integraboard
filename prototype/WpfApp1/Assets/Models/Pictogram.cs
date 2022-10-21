using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace WpfApp1.Model
{
    public class Pictogram
    {
        public int ID { get; set; }
        public string idAlfaPict { get; set; }
        public string Nombre { get; set; }
        public string Texto { get; set; }
        public enum PictogramCategory { Educacion, Comunicacion, Verbos, Objetos, AVD, Comidas, Lugares, Naturaleza, Otros };
        public string Categoria { get; set; }
        public int idImagen { get; set; }
        public string nombreImagen { get; set; }
        public BitmapImage Imagen { get; set; }
        public int? idSonido { get; set; }
        public string nombreSonido { get; set; }
        public string pathSonido { get; set; }
        public List<Etiqueta> ListaEtiquetas { get; set; }
        public SolidColorBrush colorBorde { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}
