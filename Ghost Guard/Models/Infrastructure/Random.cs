using System.Security.Cryptography;

namespace Ghost_Guard.Models.Infrastructure;

public static class Random
{
    private static readonly RandomNumberGenerator _randomGenerator;
    private static readonly System.Random _random;

    static Random()
    {
        _randomGenerator = RandomNumberGenerator.Create();
        byte[] bytes = Randomize(100);
        int seed = bytes.Sum(num => num) * bytes[0];
        _random = new System.Random(seed);
    }
    
    public static byte[] Randomize(int length)
    {
        byte[] bytes = new byte[length];
        _randomGenerator.GetBytes(bytes);
        return bytes;
    }

    public static int Range(int min, int max) => _random.Next(min, max);

    public static void Clear() => _randomGenerator.Dispose();
}

public static class ArrayExtension
{
    public static T RandomElement<T>(this T[] array) => array[Random.Range(0, array.Length)];
}
