using Ghost_Guard.Models.Application;
using Ghost_Guard.Models.Infrastructure;
using Ghost_Guard.Models.Infrastructure.Serialization;

namespace Ghost_Guard_Test.Tests;

public class SerializationTests
{
    private static readonly string _keyText = "S0wgV0Fae+p/vbKkVyJDLjhiEwMyDN4BvllT6jb/AAY=";
    private static readonly byte[] _key =
    {
        75, 76, 32, 87, 65, 90, 123, 234,
        127, 189, 178, 164, 87, 34, 67, 46,
        56, 98, 19, 3, 50, 12, 222, 1,
        190, 89, 83, 234, 54, 255, 0, 6
    };
    
    [Fact]
    public void SimpleFormattersTest()
    {
        new TestRoot().Configure();
        byte[] textBytes = { 21, 6, 32, 78 };
        
        var binary = new BinaryDataFormatter();
        var text = new TextFormatter(Config.Alphabet);

        byte[] result = binary.UnFormat(_keyText);
        
        Assert.Equal(_keyText, binary.Format(_key));
        Assert.True(_key.Match(result));
        Assert.Equal("VGg=", text.Format(textBytes));
    }

    [Fact]
    public void AesFormatterTest()
    {
        new TestRoot().Configure();
        var aes = new IndexedAesDataFormatter();
        byte[] iv = new byte[16];
        var hash = new HashIndex(_key);
        
        string expected = _keyText + " " + _keyText + " AAAAAAAAAAAAAAAAAAAAAA==";
        var pair = new KeyValuePair<HashIndex, AesProvider>(hash, new AesProvider(_key, iv));
        
        Assert.Equal(expected, aes.Format(pair));
        Assert.Equal(pair, aes.UnFormat(expected));
    }

    [Fact]
    public void BinaryFormatterTest()
    {
        new TestRoot().Configure();
        var binary = new IndexedBinaryFormatter();
        var hash = new HashIndex(_key);
        
        string expected = _keyText + " " + _keyText;
        var pair = new KeyValuePair<HashIndex, byte[]>(hash, _key);
        var unformatted = binary.UnFormat(expected);
        
        Assert.Equal(expected, binary.Format(pair));
        Assert.Equal(pair.Key, unformatted.Key);
        Assert.True(pair.Value.Match(unformatted.Value));
    }

    [Fact]
    public void VersionsFormatterTest()
    {
        new TestRoot().Configure();
        var versions = new VersionsFormatter();
        var hash = new HashIndex(_key);

        string expected = _keyText + " 0";
        var pair = new KeyValuePair<HashIndex, int>(hash, 0);

        Assert.Equal(expected, versions.Format(pair));
        Assert.Equal(pair, versions.UnFormat(expected));
    }
    
    [Fact]
    public void FileAdapterTest()
    {
        new TestRoot().Configure();

        string directory = AppContext.BaseDirectory + "/test.dat";
        var file = new FileInfo(directory);
        var adapter = new FileAdapter<byte[]>(file);
        
        adapter.Write(_key);
        var keys = adapter.Read();
        adapter.Clear();

        Assert.True(_key.Match(keys.First()));
    }
}
