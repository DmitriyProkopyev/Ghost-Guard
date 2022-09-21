using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Ghost_Guard.Models.Application;
using Ghost_Guard.ViewModels;
using Ghost_Guard.Views.Input;

namespace Ghost_Guard.Views;

public partial class NameInputPanel : Window
{
    private ApplicationContainer Application => (DataContext as InputPanelViewModel)!.Application;
    
    public NameInputPanel()
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

        new PasswordInputPanel { DataContext = DataContext }.Show();
        Close();
    }
}
