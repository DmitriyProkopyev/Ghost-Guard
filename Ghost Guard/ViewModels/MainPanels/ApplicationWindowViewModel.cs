using Ghost_Guard.Models.Application;

namespace Ghost_Guard.ViewModels;

public abstract class ApplicationWindowViewModel : ViewModelBase
{
    public readonly ApplicationContainer Application;

    private int _counter;

    public ApplicationWindowViewModel(ApplicationContainer application)
    {
        Application = application;
        Application.DataTaken += OnDataTaken;
    }
    
    private void OnDataTaken()
    {
        _counter++;

        if (_counter != 2) 
            return;
            
        Application.DataTaken -= OnDataTaken;
        ApplyAction();
    }

    protected abstract void ApplyAction();
}