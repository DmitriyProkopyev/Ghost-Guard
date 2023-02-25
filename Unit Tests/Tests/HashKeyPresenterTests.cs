using Ghost_Guard.Models.Domain;
using Ghost_Guard.Models.Infrastructure;

namespace Ghost_Guard_Test.Tests;

public class HashKeyPresenterTests
{
    [Fact]
    public void HashKeyPresenter_Empty()
    {
        new TestRoot().Configure();

        var presenter = new HashKeyPresenter();
        Assert.Throws<InvalidOperationException>(() => presenter.UseKey());
        Assert.Throws<InvalidOperationException>(() => presenter.ReadKey());
    }
    
    [Fact]
    public void KeyManagement()
    {
        new TestRoot().Configure();
        
        var k1 = DI.Container.GetInstance<KeyPartsProvider<byte[]>>();
        var k3 = DI.Container.GetInstance<KeyPartsProvider<AesProvider>>();
        var validator = DI.Container.GetInstance<HashKeyValidator>();
        byte[] key = validator.Create();
        
        var hash = Setup();

        byte[] encrypted = k1.TakeKey(hash);
        var aes = k3.TakeKey(hash);
        
        byte[] actual = aes.Decrypt(encrypted);
        Assert.True(key.Match(actual));

        HashIndex Setup()
        {
            var hash = HashIndex.CreateRandom();
            var aes = new AesProvider();
            var enc = aes.Encrypt(key);
            
            k1.Add(enc, hash);
            k3.Add(aes, hash);
            return hash;
        }
    }
    
    [Fact]
    public void HashKeyPresenter_Read()
    {
        new TestRoot().Configure();

        var presenter = new HashKeyPresenter();
        var k1 = DI.Container.GetInstance<KeyPartsProvider<byte[]>>();
        var k3 = DI.Container.GetInstance<KeyPartsProvider<AesProvider>>();
        
        var hash = HashIndex.CreateRandom();
        var aes = new AesProvider();
        byte[] data = presenter.Validator.Create();
        byte[] key = aes.Encrypt(data);

        k1.Add(key, hash);
        k3.Add(aes, hash);

        presenter.ReadKey();
        byte[] actual = presenter.UseKey(); 
        Assert.True(data.Match(actual));
    }

    [Fact]
    public void HashKeyPresenter_Write()
    {
        new TestRoot().Configure();

        var presenter = new HashKeyPresenter();
        byte[] key = presenter.Validator.Create();
        presenter.WriteKey(key);
        byte[] actual = presenter.UseKey();
        Assert.True(key.Match(actual));
    }
}
