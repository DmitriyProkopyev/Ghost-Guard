namespace Ghost_Guard.Models.Infrastructure;

public static class BytesExtensions
{
    public static bool Match(this byte[] array1, byte[] array2)
    {
        if (array1.Length != array2.Length)
            return false;

        return !array1.Where((value, i) => value != array2[i]).Any();
    }
    
    public static bool Match(this IEnumerable<byte> array1, IEnumerable<byte> array2)
    {
        bool Choose(byte first, byte second) => first == second;
        
        return EnumerateBoth(array1, array2, Choose);
    }

    public static IEnumerable<byte> BytesIntersect(this IEnumerable<IEnumerable<byte>> array1, 
        IEnumerable<IEnumerable<byte>> array2)
    {
        foreach (var bytes1 in array1)
            if (array2.Any(bytes2 => bytes1.Match(bytes2)))
                return bytes1;

        throw new InvalidOperationException();
    }

    private static bool EnumerateBoth<T>(IEnumerable<T> enumerable1, IEnumerable<T> enumerable2, Func<T, T, bool> func)
    {
        using var first = enumerable1.GetEnumerator();
        using var second = enumerable2.GetEnumerator();

        while (first.MoveNext() & second.MoveNext())
            if (func(first.Current, second.Current) == false)
                return false;

        return true;
    }
}
