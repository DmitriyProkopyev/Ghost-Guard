using System.IO;
using System.Security.Cryptography;
using Ghost_Guard.Models.Infrastructure.Memory;

namespace Ghost_Guard.Models.Infrastructure;

public class MemoryGuard : IMemoryCleaner
{
    private const int OverridingAmount = 3;
    
    public void Clear<T>(ref T[] array)
    {
        if (array is null)
            return;
        
        for (int i = 0; i < array.Length; i++)
            array[i] = default;

        array = null;
    }

    public unsafe void Clear(string subject)
    {
        fixed (char* symbols = subject)
        {
            for (int i = 0; i < subject.Length; i++)
                symbols[i] = ' ';
        }
    }

    public void Clear(FileInfo file)
    {
        byte[] empty = new byte[file.Length];

        for (int i = 0; i < OverridingAmount; i++)
            File.WriteAllBytes(file.FullName, empty);

        File.Delete(file.FullName);
    }

    public void Clear(Aes aes)
    {
        aes.Clear();
        aes.Dispose();
    }
}
