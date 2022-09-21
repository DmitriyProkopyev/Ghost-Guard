using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Ghost_Guard.Models.Application;
using Ghost_Guard.ViewModels;
using Ghost_Guard.Views.Input;

namespace Ghost_Guard.Views;

public partial class SetupWindow : Window
{
    private SetupWindowViewModel _viewModel => DataContext as SetupWindowViewModel;
    
    public SetupWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    public void ApplyDeviceToken(object? sender, RoutedEventArgs routedEventArgs)
    {
        _viewModel.Mode = SetupWindowMode.DeviceToken;
        _viewModel.Application.Authorize();
    }
    
    public void ApplyUsbToken(object? sender, RoutedEventArgs routedEventArgs)
    {
        _viewModel.Mode = SetupWindowMode.UsbToken;
        _viewModel.Application.Authorize();
    }
}
