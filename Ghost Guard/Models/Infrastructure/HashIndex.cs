using Ghost_Guard.Models.Infrastructure.Memory;

namespace Ghost_Guard.Models.Infrastructure;

public class HashIndex
{
    private readonly IMemoryCleaner _cleaner;
    private byte[] _hash;

    public IReadOnlyList<byte> Bytes => _hash;

    public HashIndex(IEnumerable<byte> hash)
    {
        _cleaner = DI.Container.GetInstance<IMemoryCleaner>();
        _hash = hash.ToArray();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not HashIndex hash) 
            return false;
        
        for (int i = 0; i < _hash.Length; i++)
            if (Bytes[i] != hash.Bytes[i])
                return false;

        return true;
    }

    public override int GetHashCode() => _hash == null ? _hash.GetHashCode() : 0;

    public override string ToString() => Convert.ToBase64String(_hash);

    public static HashIndex CreateRandom()
    {
        byte[] basic = Random.Randomize(Random.Range(16, 32));
        
        var hash = new DynamicHash();
        hash.Add(basic);
        byte[] bytes = hash.Bytes.ToArray();
        hash.Clear();

        for (int i = 0; i < Random.Range(0, basic.RandomElement()); i++)
            bytes[i % bytes.Length] += (byte)Random.Range(0, bytes.RandomElement());

        var result = new HashIndex(bytes);
        DI.Container.GetInstance<IMemoryCleaner>().Clear(ref bytes);
        return result;
    }

    public void Clear() => _cleaner.Clear(ref _hash);
}
