using Ghost_Guard.Models.Application;

namespace Ghost_Guard.ViewModels;

public class SetupWindowViewModel : ApplicationWindowViewModel
{
    public SetupWindowMode Mode;
    
    public SetupWindowViewModel(ApplicationContainer application) : base(application) { }

    protected override void ApplyAction()
    {
        switch (Mode)
        {
            case SetupWindowMode.DeviceToken:
                Application.Authorize();
                break;
            case SetupWindowMode.UsbToken:
                Application.Authorize();
                break;
        }
    }
}

public enum SetupWindowMode
{
    DeviceToken,
    UsbToken
} 
