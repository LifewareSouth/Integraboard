﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    /// <summary>
    /// Asociacion entre pictograma y etiqueta de pictograma
    /// </summary>
    public class PictEtiqueta
    {
        public int IdPictEtiqueta { get; set; }
        public int IdEtiqueta { get; set; }
        public int IdPictograma { get; set; }
    }
}
