using System.Text;

namespace Ghost_Guard.Models.Infrastructure.Serialization;

public class TextFormatter : IDataFormatter<IReadOnlyList<byte>, string>
{
    private readonly char[] _alphabet;

    public TextFormatter(char[] alphabet) => _alphabet = alphabet;

    public string Format(IReadOnlyList<byte> input)
    {
        var result = new StringBuilder();

        foreach (byte t in input)
        {
            char symbol = _alphabet[t % (_alphabet.Length - 1)];
            result.Append(symbol);
        }
        
        string str = result.ToString();
        result.Clear();
        return str;
    }

    public IReadOnlyList<byte> UnFormat(string input) => throw new NotSupportedException();
}
