using Squirrel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifeware.SoftwareCommon
{
    public class UpdateTools
    {
        /// <summary>
        /// Indica que se ha detectado una nueva versión del software
        /// </summary>
        static event EventHandler<UpdateEndEventArgs> UpdateEnd;

        /// <summary>
        /// Realiza búsqueda de actualizaciones para el software de manera asincrónica. Si existen actualizaciones lanza el evento <see cref="UpdateEnd"/> 
        /// </summary>
        /// <remarks>
        /// Esta funcionalidad debe probarse en un Release (Setup generado mediante Squirrel) debido a que las funciones de Squirrel no funcionan en ambiente de desarrollo.
        /// </remarks>
        public static async void UpdateApp()
        {
#if !DEBUG
            try
            {
                using (var updateManager = new UpdateManager(GlobalConfig.UpdateUrl))
                {
                    NuGet.SemanticVersion currentVersion = updateManager.CurrentlyInstalledVersion();
                    ReleaseEntry newInstalledVersion;


                    newInstalledVersion = await updateManager.UpdateApp();
                    if (newInstalledVersion != null && currentVersion != null && newInstalledVersion.Version != currentVersion)
                    {

                        UpdateEnd(null, new UpdateEndEventArgs(newInstalledVersion.Version));
                    }
                }
            }
            catch(Exception e) {
                Log.Write($"Update Error: {e.Message}");
            }
#endif
        }

    }
}
