using Ghost_Guard.Models.Infrastructure;
using Random = Ghost_Guard.Models.Infrastructure.Random;

namespace Ghost_Guard_Test.Tests;

public class ExtensionsTests
{
    private static readonly byte[] _bytes =
    {
        75, 76, 32, 87, 65, 90, 123, 234,
        127, 189, 178, 164, 87, 34, 67, 46,
        56, 98, 19, 3, 50, 12, 222, 1,
        190, 89, 83, 234, 54, 255, 0, 6
    };
    
    private static readonly byte[] _bytes2 =
    {
        73, 76, 32, 87, 65, 90, 123, 214,
        127, 189, 28, 164, 47, 67, 59, 46,
        56, 98, 19, 3, 50, 12, 222, 1,
        0, 89, 83, 14, 56, 255, 0, 6
    };
    
    [Fact]
    public void BytesMatchingTest()
    {
        Assert.True(_bytes.Match(_bytes));
        Assert.False(_bytes.Match(_bytes2));

        var first = _bytes as IEnumerable<byte>;
        var second = _bytes as IEnumerable<byte>;
        var third = _bytes2 as IEnumerable<byte>;
        
        Assert.True(first.Match(second));
        Assert.True(second.Match(first));
        Assert.False(second.Match(third));
    }
    
    [Fact]
    public void IntersectionTest()
    {
        byte[][] first = new byte[5][];
        byte[][] second = new byte[5][];

        for (int i = 0; i < first.Length; i++)
            first[i] = Random.Randomize(16);
        for (int i = 0; i < second.Length; i++)
            second[i] = Random.Randomize(16);

        Assert.Throws<InvalidOperationException>(() => first.BytesIntersect(second));
        
        first[3] = _bytes;
        second[1] = _bytes;
        
        Assert.Equal(first.BytesIntersect(second), _bytes);
    }
}
