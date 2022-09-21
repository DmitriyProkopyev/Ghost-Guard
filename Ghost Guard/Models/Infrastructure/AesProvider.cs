using System.Security.Cryptography;
using Ghost_Guard.Models.Infrastructure.Memory;

namespace Ghost_Guard.Models.Infrastructure;

public class AesProvider
{
    private readonly Aes _aes;
    private readonly IMemoryCleaner _cleaner;

    public byte[] Key => _aes.Key;
    public byte[] IV => _aes.IV;

    public AesProvider(byte[] key, byte[] iv)
    {
        if (key.Length != 32)
            throw new ArgumentException("Key size should be 32 bytes");
        if (iv.Length != 16)
            throw new ArgumentException("IV size should be 16 bytes");
        
        _aes = Aes.Create();
        _aes.KeySize = 256;
        _aes.Mode = CipherMode.CBC;
        _aes.Key = key;
        _aes.IV = iv;

        _cleaner = DI.Container.GetInstance<IMemoryCleaner>();
    }

    public AesProvider()
    {
        byte[] key = Random.Randomize(32);
        byte[] iv = Random.Randomize(16);

        _aes = Aes.Create();
        _aes.KeySize = 256;
        _aes.Mode = CipherMode.CBC;
        _aes.Key = key;
        _aes.IV = iv;
        
        _cleaner = DI.Container.GetInstance<IMemoryCleaner>();
    }

    public byte[] Encrypt(byte[] data) => _aes.EncryptCbc(data, _aes.IV, PaddingMode.None);

    public byte[] Decrypt(byte[] data) => _aes.DecryptCbc(data, _aes.IV, PaddingMode.None);

    public void Clear() => _cleaner.Clear(_aes);

    public override bool Equals(object? obj)
    {
        if (obj is null or not AesProvider)
            return false;

        var aes = obj as AesProvider;
        return Key.Match(aes.Key) && IV.Match(aes.IV);
    }
}
