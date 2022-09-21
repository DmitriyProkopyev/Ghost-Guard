using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Ghost_Guard.Views;

namespace Ghost_Guard
{
    public partial class App : Application
    {
        private static string _path = string.Empty;

        [STAThread]
        public static void Main(string[] args)
        {
            //_path = args.Length == 1 ? args[0] : string.Empty;
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        
        public override void Initialize() => AvaloniaXamlLoader.Load(this);
        
        public override void OnFrameworkInitializationCompleted()
        {   
            var window = new MainWindow();
            window.Show();
            base.OnFrameworkInitializationCompleted();
        }

        private static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                         .UsePlatformDetect()
                         .LogToTrace()
                         .UseReactiveUI();
    }
}
