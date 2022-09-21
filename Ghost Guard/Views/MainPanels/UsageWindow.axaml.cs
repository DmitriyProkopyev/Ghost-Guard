using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Ghost_Guard.ViewModels;

namespace Ghost_Guard.Views;

public partial class UsageWindow : Window
{
    private UsageWindowViewModel _viewModel => DataContext as UsageWindowViewModel;

    public UsageWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    public void GetPassword(object? sender, RoutedEventArgs routedEventArgs)
    {
        _viewModel.Mode = UsageWindowMode.GetPassword;
        var panel = new NameInputPanel { DataContext = new InputPanelViewModel(_viewModel.Application) };
        panel.Show();
    }
    
    public void UpgradePassword(object? sender, RoutedEventArgs routedEventArgs)
    {
        _viewModel.Mode = UsageWindowMode.UpgradePassword;
        var panel = new NameInputPanel { DataContext = new InputPanelViewModel(_viewModel.Application) };
        panel.Show();
    }
    
    public void DowngradePassword(object? sender, RoutedEventArgs routedEventArgs)
    {
        _viewModel.Mode = UsageWindowMode.DowngradePassword;
        var panel = new NameInputPanel { DataContext = new InputPanelViewModel(_viewModel.Application) };
        panel.Show();
    }
}
