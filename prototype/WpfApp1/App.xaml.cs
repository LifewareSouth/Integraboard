using Lifeware.SoftwareCommon;
using Squirrel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;



namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Update();
        }


        static async Task Update()
        {

            ReleaseEntry release = null;
            IntegraBoard_Stand_Alone.Properties.Settings.Default.Actualizando = false;
            IntegraBoard_Stand_Alone.Properties.Settings.Default.Save();
            try
            {
                using (var mgr = new UpdateManager(GlobalConfig.UpdateUrl))
                {
                    SquirrelAwareApp.HandleEvents(
                       onInitialInstall: v => mgr.CreateShortcutForThisExe(),
                       onAppUpdate: v => mgr.CreateShortcutForThisExe(),
                       onAppUninstall: v => mgr.CreateShortcutForThisExe()
                   );
                    var updateinfo = await mgr.CheckForUpdate();
                    if (updateinfo.ReleasesToApply.Any())
                    {
                        string mensaje = "Nueva versión disponible. ¿Quiere descargar ahora?";
                        DialogResult dr = System.Windows.Forms.MessageBox.Show(mensaje, "Actualización disponible", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (dr == DialogResult.Yes)
                        {
                            IntegraBoard_Stand_Alone.Properties.Settings.Default.Actualizando = true;
                            IntegraBoard_Stand_Alone.Properties.Settings.Default.Save();


                            release = await mgr.UpdateApp();

                            int pid = Process.GetCurrentProcess().Id;
                            BackupDatabase();
                            ;
                            mensaje = "Descarga completa. ¿Desea reiniciar IntegraBoard ahora?";
                            dr = System.Windows.Forms.MessageBox.Show(mensaje, "Descarga completa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dr == DialogResult.Yes)
                            {
                                Restart(pid);
                            }
                        }
                    }


                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                IntegraBoard_Stand_Alone.Properties.Settings.Default.Actualizando = false;
                IntegraBoard_Stand_Alone.Properties.Settings.Default.Save();
            }


        }

        private static void Restart(int pid)
        {
            //System.Diagnostics.Process.Start("C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\integraboard\\IntegraBoard_Stand_Alone.exe");
            Process.GetProcessById(pid).Kill();
        }

    }
}
