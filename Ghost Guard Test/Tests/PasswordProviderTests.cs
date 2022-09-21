using Ghost_Guard.Models.Application;
using Ghost_Guard.Models.Domain;
using Ghost_Guard.Models.Infrastructure;
using Random = Ghost_Guard.Models.Infrastructure.Random;

namespace Ghost_Guard_Test.Tests;

public class PasswordProviderTests
{
    [Fact]
    public void PasswordProvider_CreateThenGet()
    {
        new TestRoot().Configure();

        var provider = DI.Container.GetInstance<PasswordProvider>();
        
        provider.CreateHashKey();
        string result = provider.GetPassword(Randomize());
        Assert.NotEmpty(result);
    }
    
    [Fact]
    public void PasswordProvider_WriteThenGet()
    {
        new TestRoot().Configure();

        var provider = DI.Container.GetInstance<PasswordProvider>();
        
        provider.WriteHashKey(new HashKeyValidator(new byte[32], 512).Create());
        string result = provider.GetPassword(Randomize());
        Assert.NotEmpty(result);
    }
    
    [Fact]
    public void PasswordProvider_Other()
    {
        new TestRoot().Configure();

        var provider = DI.Container.GetInstance<PasswordProvider>();
        provider.UpgradePassword(Randomize());
        provider.DowngradePassword(Randomize());

        provider.CreateNewToken();
        provider.ApplyHashKey(Randomize());
    }
    
    private DynamicHash Randomize()
    {
        var result = new DynamicHash();
        result.Add((byte)Random.Range(0, 200));
        return result;
    }
}
