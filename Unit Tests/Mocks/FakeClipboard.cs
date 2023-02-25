using Avalonia.Input;
using Avalonia.Input.Platform;

namespace Ghost_Guard_Test.Mocks;

public class FakeClipboard : IClipboard
{
    private string _line = string.Empty;
    private object _data;
    
    public Task<string> GetTextAsync() => new (() => _line);

    public Task SetTextAsync(string text)
    {
        _line = text;
        return Task.CompletedTask;
    }

    public Task ClearAsync()
    {
        _line = string.Empty;
        return Task.CompletedTask;
    }

    public Task SetDataObjectAsync(IDataObject data)
    {
        _data = data;
        return Task.CompletedTask;
    }

    public Task<string[]> GetFormatsAsync() => new (Array.Empty<string>);

    public Task<object> GetDataAsync(string format) => new (() => _data);
}
