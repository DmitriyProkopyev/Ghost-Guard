using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Ghost_Guard.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    public void Template(object? sender, RoutedEventArgs routedEventArgs)
    {
        
    }
}
