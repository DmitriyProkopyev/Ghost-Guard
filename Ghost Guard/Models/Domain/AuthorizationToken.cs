using System.IO;
using Ghost_Guard.Models.Infrastructure;
using Ghost_Guard.Models.Infrastructure.Memory;
using Ghost_Guard.Models.Infrastructure.Serialization;

namespace Ghost_Guard.Models.Domain;

public class AuthorizationToken
{
    private readonly IMemoryCleaner _cleaner;
    private byte[] _key;

    public byte[] Key => _key;

    public AuthorizationToken(FileInfo file)
    {
        _cleaner = DI.Container.GetInstance<IMemoryCleaner>();
        var adapter = new FileAdapter<byte[]>(file);
        var keys = adapter.Read().ToArray();

        if (keys.Length != 1)
            throw new ArgumentException("Provided file is incorrect");

        _key = keys.First();
        adapter.Clear();
    }

    public AuthorizationToken(byte[] key)
    {
        _key = key;
        _cleaner = DI.Container.GetInstance<IMemoryCleaner>();
    }

    public void Clear() => _cleaner.Clear(ref _key);
}