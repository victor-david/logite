using Restless.Logite.Core;
using Restless.Logite.Database.Core;
using Restless.Logite.View;
using Restless.Logite.ViewModel;
using System.Windows;

namespace Restless.Logite
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            RegistryManager.Initialize();
            DatabaseController.Instance.Init(RegistryManager.DatabaseDirectory);
            Config.Instance.SetDefaultLocalLogDirectory(RegistryManager.DatabaseDirectory);

            MainWindow main = WindowFactory.Main.Create();

            main.Width = Config.Instance.MainWindowWidth;
            main.Height = Config.Instance.MainWindowHeight;
            main.WindowState = Config.Instance.MainWindowState;

            main.DataContext = MainWindowViewModel.Instance;
            main.Closing += (s, e) => MainWindowViewModel.Instance.SignalClosing();

            main.Show();
        }

        /// <summary>
        /// Called when the application is exiting to shut down everything and save any pending database updates.
        /// </summary>
        /// <param name="e">The exit event args</param>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            DatabaseController.Instance.Shutdown(saveTables: true);
        }
    }
}
