using Ghost_Guard_Test.Mocks;
using Ghost_Guard.Models.Application;
using Ghost_Guard.Models.Domain;
using Ghost_Guard.Models.Infrastructure;
using Ghost_Guard.Models.Infrastructure.Memory;
using Ghost_Guard.Models.Infrastructure.OsAdapters;
using Ghost_Guard.Models.Infrastructure.Serialization;

namespace Ghost_Guard_Test;

public class TestRoot
{
    public void Configure()
    {
        var container = DI.Container;

        container.Register(() => OsAdapter.Create(Environment.OSVersion.Platform));
        container.Register<IMemoryCleaner, EmptyMemoryCleaner>();
        container.Register(() => Config.Alphabet);
        container.Register<IVersionsProvider, MockVersionsProvider>();
        
        container.Register<IDataFormatter<byte[], string>, BinaryDataFormatter>();
        container.Register<IDataFormatter<IReadOnlyList<byte>, string>, TextFormatter>();
        container.Register<IndexedPairsFormatter<AesProvider>, IndexedAesDataFormatter>();
        container.Register<IndexedPairsFormatter<byte[]>, IndexedBinaryFormatter>();
        container.Register<IndexedPairsFormatter<int>, VersionsFormatter>();
        
        var k1File = new FakeFileAdapter<KeyValuePair<HashIndex, byte[]>>();
        var k3File = new FakeFileAdapter<KeyValuePair<HashIndex, AesProvider>>();
        var versionsFile = new FakeFileAdapter<KeyValuePair<HashIndex, int>>();
        
        container.Register<IFileAdapter<KeyValuePair<HashIndex, byte[]>>>(() => k1File);
        container.Register<IFileAdapter<KeyValuePair<HashIndex, AesProvider>>>(() => k3File);
        container.Register<IFileAdapter<KeyValuePair<HashIndex, int>>>(() => versionsFile);

        byte[]? ClearBytes(byte[] array)
        {
            new MemoryGuard().Clear(ref array);
            return null;
        }

        AesProvider? ClearAes(AesProvider provider)
        {
            provider.Clear();
            return null;
        }

        container.Register(() => new KeyPartsProvider<byte[]>(ClearBytes));
        container.Register(() => new KeyPartsProvider<AesProvider>(ClearAes));
        
        container.Register(() => new HashKeyValidator(Config.ConstHashKeyPart, Config.HashKeySize));
        container.Register<IHashKeyPresenter, MockHashKey>();
        container.Register<PasswordProvider>();

        container.Verify();
    }
}
