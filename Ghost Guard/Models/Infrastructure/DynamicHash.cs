using System.Security.Cryptography;
using Ghost_Guard.Models.Infrastructure.Memory;

namespace Ghost_Guard.Models.Infrastructure;

public class DynamicHash
{
    private readonly SHA256 _sha;
    private readonly IMemoryCleaner _cleaner;
    private byte[] _bytes;

    public const int Length = 32;

    public IEnumerable<byte> Bytes => _bytes;

    public DynamicHash()
    {
        _cleaner = DI.Container.GetInstance<IMemoryCleaner>();
        _sha = SHA256.Create();
        _bytes = new byte[Length];
    }

    public void Add(byte symbol)
    {
        int index = Math.Abs(_bytes.Sum(x => x) - symbol);
        _bytes[index % (Length - 1)] += symbol;

        byte[] result = _sha.ComputeHash(_bytes);
        _cleaner.Clear(ref _bytes);
        _bytes = result;
    }

    public void Add(IEnumerable<byte> symbols)
    {
        foreach (byte symbol in symbols)
            Add(symbol);
    }
    
    public HashIndex Rehash()
    {
        var hash = new DynamicHash();

        foreach (byte symbol in _bytes)
            hash.Add(symbol);

        var result = new HashIndex(hash.Bytes);
        hash.Clear();
        return result;
    }

    public void Clear()
    {
        _cleaner.Clear(ref _bytes);
        _sha.Dispose();
    }

    public byte[] CreateLongerSequence(int addedLength)
    {
        if (addedLength is > Length or < 1)
            throw new ArgumentOutOfRangeException(nameof(addedLength));

        var secondPart = Rehash();
        byte[] firstBytes = _bytes;
        var secondBytes = secondPart.Bytes;

        int length = firstBytes.Length + addedLength + secondBytes.Sum(x => x) % addedLength;
        byte[] result = new byte[length];

        for (int i = 0; i < length; i++)
        {
            if (i < firstBytes.Length)
                result[i] = firstBytes[i];
            else
                result[i] = secondBytes[i - secondBytes.Count];
        }

        return result;
    }
}
