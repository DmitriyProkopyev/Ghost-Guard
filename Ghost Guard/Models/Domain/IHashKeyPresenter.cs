namespace Ghost_Guard.Models.Domain;

public interface IHashKeyPresenter
{
    HashKeyValidator Validator { get; }

    void ReadKey();

    void WriteKey(byte[] hashKey);

    byte[] UseKey();

    AuthorizationToken CreateToken();

    void Clear();
}
