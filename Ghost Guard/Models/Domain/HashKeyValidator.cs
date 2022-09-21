namespace Ghost_Guard.Models.Domain;

public class HashKeyValidator
{
    private readonly byte[] _constantPart;
    private readonly int _size;

    public HashKeyValidator(byte[] constant, int size)
    {
        _constantPart = constant;

        if (constant.Length >= size)
            throw new ArgumentOutOfRangeException("Key size should exceed the size of the constant part");

        _size = size;
    }

    public byte[] Create()
    {
        byte[] key = Random.Randomize(_size);

        for (int i = 0; i < _constantPart.Length; i++)
            key[i] = _constantPart[i];

        return key;
    }

    public bool Validate(byte[] key)
    {
        if (key.Length != _size)
            return false;

        return !_constantPart.Where((t, i) => key[i] != t).Any();
    }
}
