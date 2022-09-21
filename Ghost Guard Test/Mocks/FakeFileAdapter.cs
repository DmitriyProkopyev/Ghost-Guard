using System.Collections.Immutable;
using Ghost_Guard.Models.Infrastructure.Serialization;

namespace Ghost_Guard_Test.Mocks;

public class FakeFileAdapter<T> : IFileAdapter<T>
{
    private IEnumerable<T> _values = ImmutableArray<T>.Empty;

    public void Write(IEnumerable<T> values) => _values = values;

    public void Write(T value) => _values = _values.Append(value);

    public IEnumerable<T> Read() => _values;

    public void Clear() { }
}
