using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Ghost_Guard.Models.Application;
using Ghost_Guard.ViewModels;

namespace Ghost_Guard.Views;

public partial class MainWindow : Window
{
    private readonly ApplicationContainer _application;

    public MainWindow()
    {
        InitializeComponent();
        
        string path = "k3.dat";
        var root = new Root();
        _application = root.Configure(path);

        DataContext = new MainWindowViewModel(_application);
        
        _application.ExecutionCompleted += Exit;
        Closing += OnClosing;
    }

    public void OpenCreationWindow(object? sender, RoutedEventArgs routedEventArgs)
    {
        var window = new CreationWindow { DataContext = new CreationWindowViewModel(_application) };
        window.Show();
    }

    public void OpenSetupWindow(object? sender, RoutedEventArgs routedEventArgs)
    {
        var window = new SetupWindow { DataContext = new SetupWindowViewModel(_application) };
        window.Show();
    }

    public void OpenUsageWindow(object? sender, RoutedEventArgs routedEventArgs)
    {
        var window = new UsageWindow { DataContext = new UsageWindowViewModel(_application) };
        window.Show();
    }

    private void Exit()
    {
        _application.ExecutionCompleted -= Exit;
        Close();
    }

    private void OnClosing(object? sender, CancelEventArgs e)
    {
        _application.ExecutionCompleted -= Exit;
        Closing -= OnClosing;
        _application.ExitImmediately();
    }
}
