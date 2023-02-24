using Lifeware.SoftwareCommon;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LifewareSoftwareLauncher
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        App() : base()
        {
            Squirrel.SquirrelAwareApp.HandleEvents(OnInitialInstall, OnAppUpdate, OnAppObsolete, OnAppUninstall, OnFirstRun);
        }

        void OnAppUpdate(Version v)
        {
            DoNothingAndClose();
        }

        void OnInitialInstall(Version v)
        {
            using (var u = new Squirrel.UpdateManager(GlobalConfig.UpdateUrl))
            {
                Squirrel.EasyModeMixin.CreateShortcutForThisExe(u);
            }
        }

        void OnAppObsolete(Version v)
        {
            DoNothingAndClose();
        }

        void OnFirstRun()
        {
            DoNothingAndClose();
        }

        void OnAppUninstall(Version v)
        {
            using (var u = new Squirrel.UpdateManager(GlobalConfig.UpdateUrl))
            {
                Squirrel.EasyModeMixin.RemoveShortcutForThisExe(u);
            }
        }
        void DoNothingAndClose()
        {
            this.Shutdown();
        }
    }
}
