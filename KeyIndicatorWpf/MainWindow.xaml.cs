using Hardcodet.Wpf.TaskbarNotification;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SnowyPeak.KeyIndicatorWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly SolidColorBrush colorizedFillBrush = new SolidColorBrush(Colors.Red);
        private readonly BackgroundWorker fadeInOutWorker = new BackgroundWorker { 
            WorkerReportsProgress = true
        };

        public Brush ColorizedFillBrush => colorizedFillBrush;
        public bool ToggledOn { get; private set; }
        public KeyboardHook.VKeys ToggledKey { get; private set; }

        private int ticks;
        private bool showAllIndicators;
        private bool isApplicationInStartup;
        private readonly TaskbarIcon taskIcon;

        public MainWindow()
        {
            InitializeComponent();

            isApplicationInStartup = WindowsStartupHelper.IsApplicationInStartup();
            bool.TryParse(ConfigurationHelper.ReadVariable(Config.SHOW_ALL_INDICATORS), out showAllIndicators);

            // Taskbar menu
            MenuItem showIndicators = new MenuItem { Header = "Show Indicators", IsCheckable = true, IsChecked = showAllIndicators };
            showIndicators.Checked += (sender, args) => OnShowIndicatorsCheckUncheck(true);
            showIndicators.Unchecked += (sender, args) => OnShowIndicatorsCheckUncheck(false);

            MenuItem launchOnStartup = new MenuItem { Header = "Launch on Windows startup", IsCheckable = true, IsChecked = isApplicationInStartup };
            launchOnStartup.Checked += (sender, args) => OnLaunchOnStartupCheckUncheck(true);
            launchOnStartup.Unchecked += (sender, args) => OnLaunchOnStartupCheckUncheck(false);

            MenuItem quit = new MenuItem { Header = "Quit" };
            quit.Click += (sender, args) => App.Current.Shutdown();

            taskIcon = new TaskbarIcon { ToolTipText = "Key Indicator" };

            taskIcon.ContextMenu = new ContextMenu();
            taskIcon.ContextMenu.Items.Add(showIndicators);
            taskIcon.ContextMenu.Items.Add(launchOnStartup);
            taskIcon.ContextMenu.Items.Add(new Separator());
            taskIcon.ContextMenu.Items.Add(quit);

            UpdateIndicators();

            fadeInOutWorker.ProgressChanged += (sender, args) =>
            {
                switch (args.ProgressPercentage)
                {
                    case 0: Show(); break;
                    case 1: Hide(); break;
                }
            };

            fadeInOutWorker.DoWork += (sender, args) =>
            {
                fadeInOutWorker.ReportProgress(0);

                while (ticks < 10)
                {
                    ticks++;
                    System.Threading.Thread.Sleep(50);
                }

                fadeInOutWorker.ReportProgress(1);
            };
        }

        public void ShowScreenOverlay(KeyboardHook.VKeys key)
        {
            Color fillColor = SystemParameters.WindowGlassColor;
            fillColor.A = 128;

            colorizedFillBrush.Color = fillColor;

            switch(key)
            {
                case KeyboardHook.VKeys.CAPITAL:
                    ToggledOn = Keyboard.IsKeyToggled(Key.CapsLock);
                    break;

                case KeyboardHook.VKeys.NUMLOCK:
                    ToggledOn = Keyboard.IsKeyToggled(Key.NumLock);
                    break;

                case KeyboardHook.VKeys.SCROLL:
                    ToggledOn = Keyboard.IsKeyToggled(Key.Scroll);
                    break;
            }
            
            ToggledKey = key;

            UpdateIndicators();

            OnPropertyChanged(nameof(ColorizedFillBrush));
            OnPropertyChanged(nameof(ToggledOn));
            OnPropertyChanged(nameof(ToggledKey));
            ticks = 0;

            if (!fadeInOutWorker.IsBusy)
                fadeInOutWorker.RunWorkerAsync();
        }

        private void OnShowIndicatorsCheckUncheck(bool value)
        {
            if (showAllIndicators != value)
            {
                showAllIndicators = value;
                ConfigurationHelper.SetVariable(Config.SHOW_ALL_INDICATORS, value.ToString());
                UpdateIndicators();
            }
        }

        private void OnLaunchOnStartupCheckUncheck(bool value)
        {
            if(isApplicationInStartup != value)
            {
                isApplicationInStartup = value;
                if (isApplicationInStartup)
                    WindowsStartupHelper.AddApplicationToStartup();
                else
                    WindowsStartupHelper.RemoveApplicationFromStartup();
            }
        }

        private void UpdateIndicators()
        {
            if (showAllIndicators)
            {
                bool capsOn = Keyboard.IsKeyToggled(Key.CapsLock);
                bool numOn = Keyboard.IsKeyToggled(Key.NumLock);
                bool scrollOn = Keyboard.IsKeyToggled(Key.Scroll);

                if (capsOn && numOn && scrollOn)
                    taskIcon.IconSource = Icons.Instance.CapsNumScroll;
                else if (capsOn && numOn)
                    taskIcon.IconSource = Icons.Instance.CapsNum;
                else if (capsOn && scrollOn)
                    taskIcon.IconSource = Icons.Instance.CapsScroll;
                else if (numOn && scrollOn)
                    taskIcon.IconSource = Icons.Instance.NumScroll;
                else if (capsOn)
                    taskIcon.IconSource = Icons.Instance.Caps;
                else if (numOn)
                    taskIcon.IconSource = Icons.Instance.Num;
                else if (scrollOn)
                    taskIcon.IconSource = Icons.Instance.Scroll;
                else
                    taskIcon.IconSource = Icons.Instance.Off;
            }
            else
                taskIcon.IconSource = Icons.Instance.NoIndicators;
        }

        private void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
