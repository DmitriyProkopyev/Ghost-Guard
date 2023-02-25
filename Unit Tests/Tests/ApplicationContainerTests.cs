using Ghost_Guard_Test.Mocks;
using Ghost_Guard.Models.Application;

namespace Ghost_Guard_Test.Tests;

public class ApplicationContainerTests
{
    [Fact]
    public void Legit()
    {
        var app = new TestRoot().Configure(new FakeClipboard());
        app.Create();

        app.TakeData(new byte[] { 0, 14, 45, 90, 2 });
        app.TakeData(new byte[] { 18, 13, 54, 30, 247 });
        app.GetPassword();
    }
    
    [Fact]
    public void Gradation()
    {
        new TestRoot().Configure();
        var app = new ApplicationContainer();
        app.Create();
        
        app.TakeData(new byte[] { 0, 14, 45, 90, 2 });
        app.TakeData(new byte[] { 18, 13, 54, 30, 247 });
        
        app.UpgradePassword();
        app.DowngradePassword();
    }
}
