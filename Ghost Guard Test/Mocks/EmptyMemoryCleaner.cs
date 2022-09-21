using System.Security.Cryptography;
using Ghost_Guard.Models.Infrastructure.Memory;

namespace Ghost_Guard_Test.Mocks;

public class EmptyMemoryCleaner : IMemoryCleaner
{
    public void Clear<T>(ref T[] array) { }

    public void Clear(string subject) { }

    public void Clear(FileInfo file) { }
    
    public void Clear(Aes aes) { }
}