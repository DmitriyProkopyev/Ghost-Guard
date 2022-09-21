namespace Ghost_Guard.Models.Infrastructure.OsAdapters;

public abstract class OsAdapter
{
    public static OsAdapter Create(PlatformID platform)
    {
        switch (platform)
        {
            case PlatformID.Unix:
                return new UnixAdapter();
            case PlatformID.MacOSX:
                return new MacOsAdapter();
            case PlatformID.Win32NT:
                return new WindowsAdapter();
            default:
                throw new ApplicationException("Unknown operating system");
        }
    }

    public abstract void Setup(Action onExit);
}