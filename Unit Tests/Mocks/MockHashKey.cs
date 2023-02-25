using Ghost_Guard.Models.Domain;
using Ghost_Guard.Models.Infrastructure;

namespace Ghost_Guard_Test.Mocks;

public class MockHashKey : IHashKeyPresenter
{
    public HashKeyValidator Validator 
        => DI.Container.GetInstance<HashKeyValidator>();
    
    public void ReadKey() { }

    public void WriteKey(byte[] hashKey) { }

    public byte[] UseKey() => Array.Empty<byte>();

    public AuthorizationToken CreateToken() 
        => new AuthorizationToken(Array.Empty<byte>());

    public void Clear() { }
}