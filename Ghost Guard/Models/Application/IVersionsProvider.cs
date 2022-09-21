using Ghost_Guard.Models.Infrastructure;

namespace Ghost_Guard.Models.Application;

public interface IVersionsProvider
{
    int GetModifier(HashIndex hash);

    void UpgradeModifier(HashIndex hash);

    void DowngradeModifier(HashIndex hash);

    void Clear();
}