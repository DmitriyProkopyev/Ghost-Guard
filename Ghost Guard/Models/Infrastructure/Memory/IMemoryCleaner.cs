using System.IO;
using System.Security.Cryptography;

namespace Ghost_Guard.Models.Infrastructure.Memory;

public interface IMemoryCleaner
{ 
    void Clear<T>(ref T[] array); 
    
    void Clear(string subject);

    void Clear(FileInfo file);

    void Clear(Aes aes);
    
    public void Clear(params string[] subjects)
    {
        foreach (string str in subjects)
            Clear(str);
    }
}
