using Ghost_Guard.Models.Infrastructure.Memory;

namespace Ghost_Guard.Models.Infrastructure.Serialization;


public class BinaryDataFormatter : IDataFormatter<byte[], string>
{
    private readonly IMemoryCleaner _cleaner;
    
    public BinaryDataFormatter() => _cleaner = DI.Container.GetInstance<IMemoryCleaner>();
    
    public string Format(byte[] input)
    {
        string result = Convert.ToBase64String(input);
        return result;
    }

    public byte[] UnFormat(string input)
    {
        byte[] result = Convert.FromBase64String(input);
        _cleaner.Clear(input);
        return result;
    }
}
