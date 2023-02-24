using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfApp1.Model
{
    public class ImagenModel
    {
        public int ID { get; set; }
        public string idAlfaImagen { get; set; }
        public string Nombre { get; set; }
        public BitmapImage Imagen { get; set; }
    }
}
