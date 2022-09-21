using Ghost_Guard.Models.Domain;
using Ghost_Guard.Models.Infrastructure;
using Ghost_Guard.Models.Infrastructure.Memory;

namespace Ghost_Guard.Models.Application;

public class PasswordProvider
{
    private readonly IVersionsProvider _provider;
    private readonly IHashKeyPresenter _hashKey;
    private readonly IDataFormatter<IReadOnlyList<byte>, string> _formatter;
    private readonly IMemoryCleaner _cleaner;

    public PasswordProvider()
    {
        _provider = DI.Container.GetInstance<IVersionsProvider>();
        _hashKey = DI.Container.GetInstance<IHashKeyPresenter>();
        _formatter = DI.Container.GetInstance<
            IDataFormatter<IReadOnlyList<byte>, string>>();
        _cleaner = DI.Container.GetInstance<IMemoryCleaner>();
    }

    public string GetPassword(DynamicHash hash)
    {
        _hashKey.ReadKey();
        ApplyModifier(hash);
        byte[] password = hash.CreateLongerSequence(Config.AddedLength);
        hash.Clear();
        
        string result = _formatter.Format(password);
        _cleaner.Clear(ref password);
        return result;
    }

    public void UpgradePassword(DynamicHash password)
    {
        var hash = password.Rehash();
        password.Clear();

        _provider.UpgradeModifier(hash);
        hash.Clear();
    }

    public void DowngradePassword(DynamicHash password)
    {
        var hash = password.Rehash();
        password.Clear();

        _provider.DowngradeModifier(hash);
        hash.Clear();
    }

    public void CreateHashKey() => _hashKey.WriteKey(_hashKey.Validator.Create());

    public void WriteHashKey(byte[] hashKey) => _hashKey.WriteKey(hashKey);

    public AuthorizationToken CreateNewToken() => _hashKey.CreateToken();
        
    public void ApplyHashKey(DynamicHash password)
    {
        byte[] hashKey = _hashKey.UseKey();
        password.Add(hashKey);
        _cleaner.Clear(ref hashKey);
    }

    private void ApplyModifier(DynamicHash password)
    {
        var hash = password.Rehash();
        int modifier = _provider.GetModifier(hash);
        hash.Clear();

        password.Add((byte)modifier);
        ApplyHashKey(password);
        password.Add((byte)modifier);
    }

    public void Clear()
    {
        _provider.Clear();
        _hashKey.Clear();
    }
}
