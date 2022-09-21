global using System;
global using System.Collections.Generic;
global using System.Linq;
global using Random = Ghost_Guard.Models.Infrastructure.Random;
global using SimpleInjector;

using System.IO;
using Ghost_Guard.Models.Application;
using Ghost_Guard.Models.Domain;
using Ghost_Guard.Models.Infrastructure;
using Ghost_Guard.Models.Infrastructure.Memory;
using Ghost_Guard.Models.Infrastructure.OsAdapters;

using Ghost_Guard.Models.Infrastructure.Serialization;

namespace Ghost_Guard;

public class Root
{
    public ApplicationContainer Configure(string fileName)
    {
        string directory = AppContext.BaseDirectory;
        Console.ForegroundColor = Config.FontColor;

        var k1 = new FileInfo(directory + Config.K1);
        var k3 = new FileInfo(fileName);
        Config.ConnectDevice(k3.DirectoryName ?? throw new FileNotFoundException());

        var versionsFile = new FileInfo(directory + Config.VersionsFileName);
        
        SetupContainer(k1, k3, versionsFile);
        
        return new ApplicationContainer();
    }

    private void SetupContainer(FileInfo k1, FileInfo k3, FileInfo versions)
    {
        var container = DI.Container;

        container.Register(() => OsAdapter.Create(Environment.OSVersion.Platform));
        container.Register<IMemoryCleaner, MemoryGuard>();
        container.Register(() => Config.Alphabet);

        container.Register<IDataFormatter<byte[], string>, BinaryDataFormatter>();
        container.Register<IDataFormatter<IReadOnlyList<byte>, string>, TextFormatter>();
        container.Register<IDataFormatter<KeyValuePair<HashIndex, AesProvider>, string>, IndexedAesDataFormatter>();
        container.Register<IDataFormatter<KeyValuePair<HashIndex, byte[]>, string>, IndexedBinaryFormatter>();
        container.Register<IDataFormatter<KeyValuePair<HashIndex, int>, string>, VersionsFormatter>();

        container.Register<IFileAdapter<KeyValuePair<HashIndex, 
            byte[]>>>(() => new FileAdapter<KeyValuePair<HashIndex, byte[]>>(k1));
        container.Register<IFileAdapter<KeyValuePair<HashIndex, 
            AesProvider>>>(() => new FileAdapter<KeyValuePair<HashIndex, AesProvider>>(k3));
        container.Register<IFileAdapter<KeyValuePair<HashIndex, 
            int>>>(() => new FileAdapter<KeyValuePair<HashIndex, int>>(versions));
        
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

        container.Register<IVersionsProvider, VersionsProvider>();
        container.Register(() => new KeyPartsProvider<byte[]>(ClearBytes));
        container.Register(() => new KeyPartsProvider<AesProvider>(ClearAes));

        container.Register(() => new HashKeyValidator(Config.ConstHashKeyPart, Config.HashKeySize));
        container.Register<IHashKeyPresenter, HashKeyPresenter>();
        container.Register<PasswordProvider>();
        
        container.Verify();
    }
}
