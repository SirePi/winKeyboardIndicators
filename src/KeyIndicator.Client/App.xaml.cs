using System.Windows;

namespace KeyIndicator.Client
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow mainWindow = new MainWindow();

            KeyboardHook kHook = new KeyboardHook();
            kHook.KeyUp += (key) =>
            {
                switch (key)
                {
                    case KeyboardHook.VKeys.CAPITAL:
                    case KeyboardHook.VKeys.NUMLOCK:
                    case KeyboardHook.VKeys.SCROLL:
                        mainWindow.ShowScreenOverlay(key);
                        break;
                }
            };

            kHook.Install();

            App.Current.DispatcherUnhandledException += (sender, args) => Shutdown();
            App.Current.Exit += (sender, args) =>
            {
                mainWindow.Close();
                kHook.Uninstall();
            };
        }
    }
}
