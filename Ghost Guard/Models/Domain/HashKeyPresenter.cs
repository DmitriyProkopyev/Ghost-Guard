using System.IO;
using Ghost_Guard.Models.Infrastructure;
using Ghost_Guard.Models.Infrastructure.Memory;

namespace Ghost_Guard.Models.Domain;

public class HashKeyPresenter : IHashKeyPresenter
{
    private readonly KeyPartsProvider<byte[]> _k1;
    private readonly KeyPartsProvider<AesProvider> _k3;
    private readonly IMemoryCleaner _cleaner;
    private HashIndex? _hashIndex;

    public HashKeyValidator Validator { get; }

    public HashKeyPresenter()
    {
        _cleaner = DI.Container.GetInstance<IMemoryCleaner>();
        
        _k1 = DI.Container.GetInstance<KeyPartsProvider<byte[]>>();
        _k3 = DI.Container.GetInstance<KeyPartsProvider<AesProvider>>();
        Validator = DI.Container.GetInstance<HashKeyValidator>();
    }
    
    public void ReadKey()
    {
        var bytes = _k1.HashIndexes.
                              Select(hash => hash.Bytes).
                              BytesIntersect(_k3.HashIndexes.
                                                 Select(hash => hash.Bytes));

        _hashIndex = _k1.HashIndexes.FirstOrDefault(hash => hash.Bytes.Match(bytes));

        if (_hashIndex == null)
            throw new InvalidOperationException("Files configuration is incorrect");
    }

    public void WriteKey(byte[] hashKey)
    {
        (byte[] encrypted, var decryptor) = CalculateNewKeys(hashKey);
        _hashIndex = AddKeys(encrypted, decryptor);
    }

    public byte[] UseKey()
    {
        if (_hashIndex == null)
            throw new InvalidOperationException("Presenter should first read or create key");

        return UpdateKeys();
    }

    public AuthorizationToken CreateToken() => new(UpdateKeys());

    public void Clear()
    {
        _hashIndex?.Clear();
        _k1.Clear();
        _k3.Clear();
    }

    private byte[] UpdateKeys()
    {
        byte[] k1 = _k1.TakeKey(_hashIndex);
        var k3 = _k3.TakeKey(_hashIndex);
        byte[] decrypted = Decrypt(k1, k3);

        if (!Validator.Validate(decrypted))
            throw new InvalidDataException("Decrypted hash key is wrong, it was damaged or replaced");

        (byte[] encrypted, var decryptor) = CalculateNewKeys(decrypted);
        _hashIndex = AddKeys(encrypted, decryptor);
        return decrypted;
    }

    private byte[] Decrypt(byte[] encrypted, AesProvider aes)
    {
        byte[] decrypted = aes.Decrypt(encrypted);
        //_cleaner.Clear(ref encrypted);
        //aes.Clear();
        return decrypted;
    }

    private HashIndex AddKeys(byte[] encrypted, AesProvider aes)
    {
        var index = HashIndex.CreateRandom();

        while (_k1.Contains(index) || _k3.Contains(index))
            index = HashIndex.CreateRandom();
        
        _k1.Add(encrypted, index);
        _k3.Add(aes, index);

        return index;
    }

    private (byte[], AesProvider) CalculateNewKeys(byte[] hash)
    {
        var aes = new AesProvider();
        byte[] encrypted = aes.Encrypt(hash);
        _cleaner.Clear(ref hash);
        return (encrypted, aes);
    }
}
