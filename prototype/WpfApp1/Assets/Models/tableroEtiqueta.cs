using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Assets.Models
{
    /// <summary>
    /// Asociacion entre tableros y etiqueta de tablero
    /// </summary>
    internal class tableroEtiqueta
    {
        public int IdPictEtiqueta { get; set; }
        public int IdEtiqueta { get; set; }
        public int IdPictograma { get; set; }
    }
}
