namespace Ghost_Guard.Models.Infrastructure.Serialization;

public class IndexedBinaryFormatter : IndexedPairsFormatter<byte[]>
{
    protected override string Format(byte[] input) => Binary.Format(input);

    protected override byte[] FormatBack(string input) => Binary.UnFormat(input);
}
