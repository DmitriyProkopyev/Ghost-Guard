using Ghost_Guard.Models.Application;

namespace Ghost_Guard.ViewModels;

public class ConsoleInterface
{
    private readonly Dictionary<string, Action> _actions;
    private readonly ApplicationContainer _container;

    private bool _working = true;

    public ConsoleInterface(ApplicationContainer container)
    {
        _container = container ?? throw new ArgumentNullException(nameof(container));
        _actions = new Dictionary<string, Action>
        {
            /*["Get Password"] = ReadPassword + _container.GetPassword,
            ["Upgrade Password"] = ReadPassword + _container.UpgradePassword,
            ["Downgrade Password"] = ReadPassword + _container.DowngradePassword,*/
            
            ["Create Hash Key"] = _container.Create,
            ["Authorize"] = _container.Authorize,
            ["Register New Device"] = _container.RegisterDevice,
            ["Register New Usb"] = _container.RegisterUsb
        };
    }

    public void Start()
    {
        string[] lines = _actions.Keys.ToArray();
        int cursor = 0;

        while (_working)
        {
            string capsLock = "CapsLock is " + (Console.CapsLock ? "On" : "Off");
            Console.WriteLine(capsLock);
            Console.WriteLine("_______\n\n");

            Console.WriteLine("Choose action\n");
            Console.WriteLine("_______\n\n");

            for (int i = 0; i < lines.Length; i++)
            {
                Console.Write(lines[i]);

                if (i == cursor)
                    Console.Write(" >>>");

                Console.WriteLine("\n\n");
            }

            var key = Console.ReadKey().Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    cursor--;
                    if (cursor < 0)
                        cursor = lines.Length - 1;
                    break;
                case ConsoleKey.DownArrow:
                    cursor++;
                    if (cursor >= lines.Length)
                        cursor = 0;
                    break;
                case ConsoleKey.Enter:
                    _actions[lines[cursor]].Invoke();
                    break;
            }

            Console.Clear();
        }

        Exit(5);
    }

    private void Exit(int delay)
    {
        Console.WriteLine($"You have {delay} seconds before exiting");
        _container.Exit(delay);
    }

    private void ReadPassword()
    {
        _working = false;

        Console.WriteLine("Message:\n");
        _container.TakeData(ReadInput());

        Console.WriteLine("Key:\n");
        _container.TakeData(ReadInput());
    }

    private IEnumerable<byte> ReadInput()
    {
        var key = Console.ReadKey(true).Key;

        while (key != ConsoleKey.Enter)
        {
            key = default;

            key = Console.ReadKey(true).Key;
            yield return (byte)key;
        }
    }
}
