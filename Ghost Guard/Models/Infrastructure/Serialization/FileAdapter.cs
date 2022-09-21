using System.IO;
using Ghost_Guard.Models.Infrastructure.Memory;

namespace Ghost_Guard.Models.Infrastructure.Serialization;

public class FileAdapter<T> : IFileAdapter<T>
{
    private readonly FileInfo _file;
    private readonly IDataFormatter<T, string> _formatter;
    private readonly IMemoryCleaner _cleaner;

    public FileAdapter(FileInfo file)
    {
        _cleaner = DI.Container.GetInstance<IMemoryCleaner>();
        _file = file;
        _formatter = DI.Container.GetInstance<IDataFormatter<T, string>>();
    }
    
    public void Write(IEnumerable<T> values)
    {
        string[] result = new string[values.Count()];
        int index = 0;
        
        foreach (var value in values)
            result[index++] = Serialize(value);

        File.WriteAllLines(_file.FullName, result);
    }

    public void Write(T value) => File.WriteAllLines(_file.FullName, new[] { Serialize(value) });

    public IEnumerable<T> Read() => File.ReadAllLines(_file.FullName).Select(Deserialize);

    public void Clear() => _cleaner.Clear(_file);

    private string Serialize(T value) => _formatter.Format(value);

    private T Deserialize(string value) => _formatter.UnFormat(value);
}
