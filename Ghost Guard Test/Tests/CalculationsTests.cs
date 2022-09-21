using Ghost_Guard.Models.Application;
using Ghost_Guard.Models.Domain;
using Ghost_Guard.Models.Infrastructure;
using Random = Ghost_Guard.Models.Infrastructure.Random;

namespace Ghost_Guard_Test.Tests;

public class CalculationsTests
{
    private static readonly byte[] _key =
    {
        75, 76, 32, 87, 65, 90, 123, 234,
        127, 189, 178, 164, 87, 34, 67, 46,
        56, 98, 19, 3, 50, 12, 222, 1,
        190, 89, 83, 234, 54, 255, 0, 6
    };

    [Fact]
    public void AesTest()
    {
        new TestRoot().Configure();

        var aes = new AesProvider();
        byte[] data = new HashKeyValidator(Config.ConstHashKeyPart, Config.HashKeySize).Create();
        byte[] encrypted = aes.Encrypt(data);
        byte[] decrypted = aes.Decrypt(encrypted);
        
        Assert.True(data.Match(decrypted));

        aes = new AesProvider(_key, new byte[16]);
        encrypted = aes.Encrypt(data);
        decrypted = aes.Decrypt(encrypted);
        
        Assert.True(data.Match(decrypted));

        Assert.Throws<ArgumentException>(() => new AesProvider(new byte[31], new byte[16]));
        Assert.Throws<ArgumentException>(() => new AesProvider(new byte[32], new byte[15]));
    }
    
    [Fact]
    public void MemoryTest()
    {
        new TestRoot().Configure();

        byte[] originalBytes = new byte[_key.Length];
        Array.Copy(_key, originalBytes, _key.Length);
        
        byte[] copy = originalBytes;
        byte[] empty = new byte[originalBytes.Length];
        
        var memoryGuard = new MemoryGuard();
        memoryGuard.Clear(ref originalBytes);
        Assert.Null(originalBytes);
        Assert.True(copy.Match(empty));

        string text = "Sensitive data";
        memoryGuard.Clear(text);
        
        Assert.Equal("              ", text);

        var file = new FileInfo(AppContext.BaseDirectory + "/test.dat");
        File.WriteAllBytes(file.FullName, _key);
        memoryGuard.Clear(file);

        Assert.Throws<FileNotFoundException>(() => File.ReadAllBytes(file.FullName));
    }
    
    [Fact]
    public void DynamicHashTest()
    {
        new TestRoot().Configure();
        
        var hash = new DynamicHash();

        for (int i = 0; i < 5; i++)
        {
            int random = Random.Range(0, 256);
            hash.Add((byte)random);
            Assert.False(new byte[DynamicHash.Length].Match(hash.Bytes));
        
            byte[] added = hash.CreateLongerSequence(8);
            Assert.True(added.Length is <= 48 and >= 40);
        }

        Assert.Throws<ArgumentOutOfRangeException>(() 
            => hash.CreateLongerSequence(-1));
    }
    
    [Fact]
    public void HashIndexTest()
    {
        new TestRoot().Configure();
        Assert.True(HashIndex.CreateRandom().Bytes.Count == DynamicHash.Length);
    }
}
