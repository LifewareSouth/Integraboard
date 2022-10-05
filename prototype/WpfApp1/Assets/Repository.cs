using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using WpfApp1.Pages.Pictogramas;

namespace WpfApp1.Assets
{
    public class Repository
    {
        private static readonly Repository instance = new Repository();
        public static Repository Instance
        {
            get
            {
                return instance;
            }
        }

        public static string PictogramCategoryToString(Pictogram.PictogramCategory pCategory)
        {
            switch (pCategory)
            {
                case (Pictogram.PictogramCategory.Educacion):
                    {
                        return "Educación";
                    }
                case (Pictogram.PictogramCategory.Comunicacion):
                    {
                        return "Comunicación";
                    }
                case (Pictogram.PictogramCategory.Verbos):
                    {
                        return "Verbos";
                    }
                case (Pictogram.PictogramCategory.Objetos):
                    {
                        return "Objetos";
                    }
                case (Pictogram.PictogramCategory.AVD):
                    {
                        return "AVD";
                    }
                case (Pictogram.PictogramCategory.Comidas):
                    {
                        return "Comidas";
                    }
                case (Pictogram.PictogramCategory.Lugares):
                    {
                        return "Lugares";
                    }
                case (Pictogram.PictogramCategory.Naturaleza):
                    {
                        return "Naturaleza";
                    }
                case (Pictogram.PictogramCategory.Otros):
                    {
                        return "Otros";
                    }

            }
            return "";
        }
    }
}
