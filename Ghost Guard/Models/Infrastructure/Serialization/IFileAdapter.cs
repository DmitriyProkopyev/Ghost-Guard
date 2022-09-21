namespace Ghost_Guard.Models.Infrastructure.Serialization;

public interface IFileAdapter<T>
{
    void Write(IEnumerable<T> values);

    void Write(T value);

    IEnumerable<T> Read();

    void Clear();
}