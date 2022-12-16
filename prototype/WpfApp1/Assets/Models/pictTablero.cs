using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WpfApp1.UserControls;

namespace WpfApp1.Model
{
    public class pictTablero
    {
        AddPictogram _addpict = new AddPictogram();
        public int ID { get; set; }
        public int idTablero { get; set; }
        public int idPictograma { get; set; }
        public int x { get; set; }
        public int y { get; set; } 
        public Pictogram pictograma { get; set; }
        public string tiempo { get; set; } //
        public BitmapImage imagenEstado { get; set; }
    }
}
