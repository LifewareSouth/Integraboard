using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifeware.SoftwareCommon
{
    /// <summary>
    /// Representa las configuraciones que se deben hacer para cada software. Agrgar acá parametros propios de la aplicacion en cuestión.
    /// </summary>
    public class GlobalConfig
    {
        //TODO: Establecer nombre de aplicación aca
        private const string APPNAME = "Integraboard_version_4";
        //TODO: Establecer versión de aplicación. Recordar actualizar en cada Release
        public const string APPVERSION = "0.0.0.1";
        //TODO: Establecer GUID de la aplicación.
        private const string APPGUID = "0ec6ed26-b1b8-4906-acdf-dd05a3901f88";
        //TODO: Establecer URL de descargas y actualizaciones de la aplicación
        private const string UPDATE_URL = "http://updates.lifeware.cl/integraboardStandAlone/debug/";
        
        //no tocar desde este punto

        public static string AppName { get { return APPNAME; } }
        public static string AppVersion { get { return APPVERSION; } }
        public static string AppGuid { get { return APPGUID; } }
        public static string ExeName { get { return $"{APPNAME}.exe"; } }
        public static string UpdateUrl { get { return UPDATE_URL; } }
    }
}
