using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfApp1.Model
{
    public class PerfilModel
    {
        public int idPerfil { get; set; }
        public string idAlfaPerfil { get; set; }
        public string tipoPerfil { get; set; }
        public string nombrePerfil { get; set; }
        public int edad { get; set; }
        public string tamaño { get; set; }
        public BitmapImage fotoPerfil { get; set; }
        public string voz { get; set; }
    }
}
