﻿using Lifeware.SoftwareCommon;
using Squirrel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Pages.Dialogs;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static double profileSize
        {
            get => (double)Current.Resources["profileSize"];
            set => Current.Resources["profileSize"] = value;
        }
        public App()
        {
            Update();
            RestoreDB();
        }


        static async Task Update()
        {

            ReleaseEntry release = null;
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

                        update u = new update();
                        u.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        var result = u.ShowDialog();

                        if (result == true)
                        {

                            release = await mgr.UpdateApp();

                            int pid = Process.GetCurrentProcess().Id;
                            BackupDatabase();
                            string mensaje = "Descarga completa.\n ¿Desea reiniciar IntegraBoard ahora?";
                            update up = new update(mensaje);
                            //Restart(pid);
                        }
                    }


                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private static void Restart(int pid)
        {
            //System.Diagnostics.Process.Start("C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\integraboard\\IntegraBoard_Stand_Alone.exe");
            Process.GetProcessById(pid).Kill();
        }
        private static void BackupDatabase()
        {
            string dbFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "database.db");
            string destination = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\..\\database.db";
            File.Copy(dbFile, destination, true);
        }

        private static void RestoreDB()
        {
            //Restore database after application update            
            string dest = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\database.db";
            string sourceFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\..\\database.db";

            // Check if we have settings that we need to restore
            if (!File.Exists(sourceFile))
                // Nothing we need to do
                return;

            // Create directory as needed
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(dest));
            }
            catch (Exception) { }

            // Copy our backup file in place 
            try
            {
                File.Copy(sourceFile, dest, true);
            }
            catch (Exception) { }

            // Delete backup file
            try
            {
                File.Delete(sourceFile);
            }
            catch (Exception) { }
        }
    }
    
}
