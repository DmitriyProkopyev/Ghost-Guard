namespace Ghost_Guard.Models.Infrastructure;

public static class DI
{
    public static readonly Container Container;

    static DI()
    {
        Container = new Container();
        Container.Options.DefaultLifestyle = Lifestyle.Singleton;
    }
}
