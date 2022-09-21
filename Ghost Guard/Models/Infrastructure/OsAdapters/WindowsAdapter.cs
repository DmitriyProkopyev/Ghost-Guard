using System.Runtime.InteropServices;

namespace Ghost_Guard.Models.Infrastructure.OsAdapters;

public class WindowsAdapter : OsAdapter
{
    private delegate bool ConsoleEventDelegate(int eventType);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

    private Action _onExit;

    public override void Setup(Action onExit)
    {
        _onExit = onExit;
        var handler = new ConsoleEventDelegate(OnApplicationExit);
        SetConsoleCtrlHandler(handler, true);
    }

    private bool OnApplicationExit(int eventType)
    {
        _onExit();
        return false;
    }
}
