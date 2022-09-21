namespace Ghost_Guard.Models.Infrastructure.Serialization;

public class IndexedAesDataFormatter : IndexedPairsFormatter<AesProvider>
{
    protected override string Format(AesProvider input)
    {
        string key = Binary.Format(input.Key);
        string iv = Binary.Format(input.IV);

        string result = string.Concat(key, Divider, iv);
        Cleaner.Clear(key, iv);

        return result;
    }

    protected override AesProvider FormatBack(string input)
    {
        int index = input.IndexOf(Divider);
        string keyValue = input[..index];
        string ivValue = input[(index + 1)..];

        byte[] key = Binary.UnFormat(keyValue);
        byte[] iv = Binary.UnFormat(ivValue);
        
        Cleaner.Clear(input, keyValue, ivValue);

        return new AesProvider(key, iv);
    }
}
