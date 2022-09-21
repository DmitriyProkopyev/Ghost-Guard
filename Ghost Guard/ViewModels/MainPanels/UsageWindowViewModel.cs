using Ghost_Guard.Models.Application;

namespace Ghost_Guard.ViewModels;

public class UsageWindowViewModel : ApplicationWindowViewModel
{
    public UsageWindowMode Mode;
    
    public UsageWindowViewModel(ApplicationContainer application) : base(application) { }

    protected override void ApplyAction()
    {
        switch (Mode)
        {
            case UsageWindowMode.GetPassword:
                Application.GetPassword();
                break;
            case UsageWindowMode.UpgradePassword:
                Application.UpgradePassword();
                break;
            case UsageWindowMode.DowngradePassword:
                Application.DowngradePassword();
                break;
        }
    }
}

public enum UsageWindowMode
{
    GetPassword, 
    UpgradePassword,
    DowngradePassword
} 
