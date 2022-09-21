using Ghost_Guard.Models.Application;
using Ghost_Guard.Models.Domain;
using Ghost_Guard.Models.Infrastructure;
using Random = Ghost_Guard.Models.Infrastructure.Random;

namespace Ghost_Guard_Test.Tests;

public class KeySystemsTests
{
    [Fact]
    public void HashKeyValidatorTest()
    {
        new TestRoot().Configure();

        var validator = new HashKeyValidator(Config.ConstHashKeyPart, Config.HashKeySize);
        Assert.True(validator.Validate(validator.Create()));
        Assert.False(validator.Validate(new byte[10]));

        byte[] wrong = new byte[512];
        wrong[0] = 255;

        Assert.False(validator.Validate(wrong));
    }
    
    [Fact]
    public void KeyPartTest()
    {
        new TestRoot().Configure();
        int value1 = Random.Range(0, 256);
        int value2 = value1 + 10;
        
        var keyPart = new KeyPartsProvider<int>(_ => 0);
        var hash1 = HashIndex.CreateRandom();
        var hash2 = HashIndex.CreateRandom();
        
        keyPart.Add(value1, hash1);
        keyPart.Add(value2, hash2);
        
        Assert.False(hash1.Bytes.Match(hash2.Bytes));
        Assert.True(keyPart.Contains(hash1));
        Assert.Equal(value1, keyPart.TakeKey(hash1));
        Assert.Equal(value2, keyPart.TakeKey(hash2));
        Assert.False(keyPart.Contains(hash1));

        Assert.Throws<ArgumentException>(() => keyPart.TakeKey(hash1));
        Assert.Throws<ArgumentNullException>(() => keyPart.Add(0, null));
    }
    
    [Fact]
    public void VersionsProviderTest()
    {
        new TestRoot().Configure();
        
        var provider = new VersionsProvider();
        var hash1 = HashIndex.CreateRandom();
        var hash2 = HashIndex.CreateRandom();
        var hash3 = HashIndex.CreateRandom();
        
        Assert.Equal(0, provider.GetModifier(hash1));
        provider.UpgradeModifier(hash1);
        Assert.Equal(1, provider.GetModifier(hash1));
        
        provider.DowngradeModifier(hash2);
        Assert.Equal(0, provider.GetModifier(hash2));
        
        for (int i = 0; i < 10; i++)
            provider.UpgradeModifier(hash3);
        
        provider.DowngradeModifier(hash3);
        
        Assert.Equal(9, provider.GetModifier(hash3));
    }
}
