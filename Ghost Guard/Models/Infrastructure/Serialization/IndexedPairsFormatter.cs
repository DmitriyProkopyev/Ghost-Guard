using Ghost_Guard.Models.Infrastructure.Memory;

namespace Ghost_Guard.Models.Infrastructure.Serialization;

public abstract class IndexedPairsFormatter<T> : IDataFormatter<KeyValuePair<HashIndex, T>, string>
{
    protected readonly IMemoryCleaner Cleaner;
    protected const char Divider = ' ';
    protected readonly BinaryDataFormatter Binary = new();

    protected IndexedPairsFormatter() => Cleaner = DI.Container.GetInstance<IMemoryCleaner>();
    
    public string Format(KeyValuePair<HashIndex, T> input)
    {
        string hash = input.Key.ToString();
        string value = Format(input.Value);
        string result = string.Concat(hash, Divider, value);
        Cleaner.Clear(hash, value);
        return result;
    }

    public KeyValuePair<HashIndex, T> UnFormat(string input)
    {
        int dividerIndex = input.IndexOf(Divider);
        string hashValue = input[..dividerIndex];
        string actualValue = input[(dividerIndex + 1)..];

        byte[] hashBytes = Binary.UnFormat(hashValue);
        T actual = FormatBack(actualValue);
        
        Cleaner.Clear(input, hashValue, actualValue);
        var hash = new HashIndex(hashBytes);
        Cleaner.Clear(ref hashBytes);

        return new KeyValuePair<HashIndex, T>(hash, actual);
    }

    protected abstract string Format(T value);
    
    protected abstract T FormatBack(string value);
}
