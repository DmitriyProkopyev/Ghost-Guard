namespace Ghost_Guard.Models.Infrastructure.Serialization;

public class VersionsFormatter : IndexedPairsFormatter<int>
{
    protected override string Format(int value) => value.ToString();

    protected override int FormatBack(string value) => int.Parse(value);
}
