using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp1.Model
{
    public class Pictogram
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Texto { get; set; }
        public enum PictogramCategory { Educacion, Comunicacion, Verbos, Objetos, AVD, Comidas, Lugares, Naturaleza, Otros };

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}
