using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfApp1.Model
{
    public class Board
    {
        public int ID { get; set; }
        public string idAlfaTablero { get; set; }
        public string nombreTablero { get; set; }
        public string tipo {get; set; }
        public enum TableroTipo { Comunicación, Emociones, Rutina, Sonidos };
        public int filas { get; set; }
        public int columnas { get; set; }
        public int idPictPortada { get; set; }
        public Pictogram pictPortada { get; set; }
        public List<pictTablero> pictTableros { get; set; }
    }
}
