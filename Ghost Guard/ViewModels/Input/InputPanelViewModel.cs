using Ghost_Guard.Models.Application;
using Ghost_Guard.Models.Infrastructure;
using Ghost_Guard.Models.Infrastructure.Memory;

namespace Ghost_Guard.ViewModels;

public class InputPanelViewModel : ViewModelBase
{
    public readonly ApplicationContainer Application;

    public InputPanelViewModel(ApplicationContainer application) => Application = application;

    public void Apply(string text)
    {
        var cleaner = DI.Container.GetInstance<IMemoryCleaner>();
        
        Application.TakeData(text.Select(s => (byte)s));
        cleaner.Clear(text);
    }
}
