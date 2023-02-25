using Ghost_Guard.Models.Application;
using Ghost_Guard.Models.Infrastructure;

namespace Ghost_Guard_Test.Mocks;

public class MockVersionsProvider : IVersionsProvider
{
    public int GetModifier(HashIndex hash) => 0;

    public void UpgradeModifier(HashIndex hash) { }

    public void DowngradeModifier(HashIndex hash) { }
    
    public void Clear() { }
}
