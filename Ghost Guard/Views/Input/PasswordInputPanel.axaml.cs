using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Ghost_Guard.Models.Application;
using Ghost_Guard.ViewModels;

namespace Ghost_Guard.Views.Input;

public partial class PasswordInputPanel : Window
{
    public PasswordInputPanel()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public void OnButtonPressed(object? sender, RoutedEventArgs e)
    {
        var viewModel = DataContext as InputPanelViewModel;
        var text = this.FindControl<TextBox>("Input");
        viewModel!.Apply(text.Text);
        Close();
    }
}
