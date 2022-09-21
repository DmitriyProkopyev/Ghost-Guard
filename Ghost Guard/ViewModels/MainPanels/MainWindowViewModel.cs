using Ghost_Guard.Models.Application;

namespace Ghost_Guard.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ApplicationContainer _application;

        public MainWindowViewModel(ApplicationContainer application) => _application = application;
    }
}
