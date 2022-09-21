using System.IO;
using System.Threading.Tasks;
using Avalonia.Input.Platform;
using Ghost_Guard.Models.Domain;
using Ghost_Guard.Models.Infrastructure;
using Ghost_Guard.Models.Infrastructure.Memory;
using Ghost_Guard.Models.Infrastructure.OsAdapters;
using Ghost_Guard.Models.Infrastructure.Serialization;

namespace Ghost_Guard.Models.Application;

public class ApplicationContainer
{
    private readonly IMemoryCleaner _cleaner;
    private readonly PasswordProvider _provider;
    private readonly DynamicHash _hash;
    private readonly IClipboard _clipboard;

    private string _result;

    public event Action DataTaken; 
    public event Action ExecutionCompleted;

    public ApplicationContainer()
    {
        _cleaner = DI.Container.GetInstance<IMemoryCleaner>();
        _provider = DI.Container.GetInstance<PasswordProvider>();

        DI.Container.GetInstance<OsAdapter>().Setup(ExitImmediately);
        _clipboard = Avalonia.Application.Current!.Clipboard!;
        
        _hash = new DynamicHash();
        _result = string.Empty;
    }

    public void GetPassword()
    {
        _result = _provider.GetPassword(_hash);
        _clipboard.SetTextAsync(_result);
        Exit(5);
    }

    public void UpgradePassword() => _provider.UpgradePassword(_hash);

    public void DowngradePassword() => _provider.DowngradePassword(_hash);

    public void Create() => _provider.CreateHashKey();

    public void Authorize()
    {
        var file = new FileInfo(Config.DeviceAuthorizationTokenPath);
        file = file.Exists ? file : new FileInfo(Config.UsbAuthorizationTokenPath);

        if (!file.Exists)
            throw new InvalidOperationException("Authorization token doesn't exist");

        var token = new AuthorizationToken(file);
        _provider.WriteHashKey(token.Key);
    }

    public void RegisterDevice()
    {
        var token = _provider.CreateNewToken();
        var adapter = new FileAdapter<byte[]>(new FileInfo(Config.UsbAuthorizationTokenPath));
        adapter.Write(token.Key);
        token.Clear();
    }

    public void RegisterUsb()
    {
        var token = _provider.CreateNewToken();
        var adapter = new FileAdapter<byte[]>(new FileInfo(Config.DeviceAuthorizationTokenPath));
        adapter.Write(token.Key);
        token.Clear();
    }

    public void TakeData(IEnumerable<byte> data)
    {
        foreach (byte symbol in data)
            _hash.Add(symbol);
        _provider.ApplyHashKey(_hash);
        DataTaken?.Invoke();
    }

    public async void Exit(int delay)
    {
        _hash.Clear();
        _provider.Clear();
        _cleaner.Clear(_result);
        Random.Clear();
        GC.Collect();

        await Task.Delay(delay * 1000);
        await _clipboard.ClearAsync();
        ExecutionCompleted?.Invoke();
    }

    public void ExitImmediately() 
    {
        _clipboard.ClearAsync();
        _hash.Clear();
        _provider.Clear();
        _cleaner.Clear(_result);
        Random.Clear();
        GC.Collect();
        ExecutionCompleted?.Invoke();
    }
}
