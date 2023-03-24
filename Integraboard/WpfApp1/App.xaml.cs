using Lifeware.SoftwareCommon;
using Squirrel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Assets;
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
            if ((!File.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\database.db"))&&(!File.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\..\\database.db")))
            {

                string pathOldDb = getOldDatabase();
                if(pathOldDb != "")
                {
                    threadClassApp threadclass = new threadClassApp();
                    Thread newThread =
                    new Thread(new ThreadStart(threadclass.ThreadMethod));
                    newThread.SetApartmentState(ApartmentState.STA);
                    newThread.Start();
                    Repository.Instance.transform_old_database(pathOldDb);
                    threadclass.Activo = true;
                    newThread.Join();
                }
            }
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
            catch (Exception) 
            {
                int pid = Process.GetCurrentProcess().Id;
                string pathExe = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\IntegraBoard_Stand_Alone.exe";
                System.Diagnostics.Process.Start(pathExe);
                Process.GetProcessById(pid).Kill();
            }
        }
        private static string getOldDatabase()
        {
            string pathOldDb = "";
            string username = Environment.UserName;
            string path = "C:\\Users\\" + username + "\\AppData\\Local\\integraboard";
            string folder = "app-1.1.";
            long length = 0;
            for (int i = 66; i > 63; i--)
            {
                string TempPathOldDb = path + "\\" + folder + i;
                if (Directory.Exists(TempPathOldDb))
                {
                    if (File.Exists(TempPathOldDb + "\\test.db"))
                    {
                        if (length < new System.IO.FileInfo(TempPathOldDb + "\\test.db").Length)
                        {
                            length = new System.IO.FileInfo(TempPathOldDb + "\\test.db").Length;
                            pathOldDb = TempPathOldDb + "\\test.db";
                        }
                    }
                }
            }
            return pathOldDb;
        }
        class threadClassApp
        {
            bool activo = false;

            public bool Activo
            {
                set { activo = value; }
            }
            public threadClassApp() { }

            public void ThreadMethod()
            {
                try
                {
                    ActualizandoDatosDialog ad = new ActualizandoDatosDialog();
                    ad.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                    while (!activo)
                    {
                        ad.Show();
                    }
                    ad.Close();
                }
                catch (ThreadInterruptedException e)
                {
                    Console.WriteLine("newThread cannot go to sleep - " +
                        "interrupted by main thread.");
                }
            }
        }
    }
    
}
