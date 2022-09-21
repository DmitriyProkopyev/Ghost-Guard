using Ghost_Guard.Models.Application;

namespace Ghost_Guard.ViewModels;

public class CreationWindowViewModel : ApplicationWindowViewModel
{
    public CreationWindowMode Mode;
        
    public CreationWindowViewModel(ApplicationContainer application) : base(application) { }

    protected override void ApplyAction()
    {
        switch (Mode)
        {
            case CreationWindowMode.HashKey:
                Application.Create();
                break;
            case CreationWindowMode.DeviceToken:
                Application.RegisterDevice();
                break;
            case CreationWindowMode.UsbToken:
                Application.RegisterUsb();
                break;
        }
    }
}

public enum CreationWindowMode
{
    HashKey,
    DeviceToken,
    UsbToken
} 
