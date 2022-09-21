using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Ghost_Guard.ViewModels;

namespace Ghost_Guard.Views;

public partial class CreationWindow : Window
{
    private CreationWindowViewModel _viewModel => DataContext as CreationWindowViewModel;

    public CreationWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    public void CreateHashKey(object? sender, RoutedEventArgs routedEventArgs) 
        => _viewModel.Application.Create();

    public void CreateDeviceToken(object? sender, RoutedEventArgs routedEventArgs)
    {
        _viewModel.Mode = CreationWindowMode.DeviceToken;
        _viewModel.Application.RegisterDevice();
    }
    
    public void CreateUsbToken(object? sender, RoutedEventArgs routedEventArgs)
    {
        _viewModel.Mode = CreationWindowMode.UsbToken;
        _viewModel.Application.RegisterUsb();
    }
}
