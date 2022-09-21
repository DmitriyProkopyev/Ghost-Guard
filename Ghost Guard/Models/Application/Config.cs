namespace Ghost_Guard.Models.Application;

public static class Config
{
    private static string _devicePath;

    private const string AuthorizationToken = "\\token.dat";

    public const string K1 = "\\k1.dat";

    public const int AddedLength = 8;

    public static char[] Alphabet => new[]
    {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 
        'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
        'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
        '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '!', '"', '$',
        '%', '&', "'"[0], '(', ')', '+', ',', '-', '.', '/', ':', ';', '<',
        '=', '>', '?', '@', '[', ']', '^', '_', '{', '|', '}', '~', '`'
    };

    public static byte[] ConstHashKeyPart { get; } = new byte[32];

    public static int HashKeySize => 512;

    public static string VersionsFileName => "\\versions.dat";

    public static string DeviceAuthorizationTokenPath => _devicePath + AuthorizationToken;

    public static string UsbAuthorizationTokenPath => AppContext.BaseDirectory + AuthorizationToken;

    public static ConsoleColor FontColor => ConsoleColor.Green;

    public static void ConnectDevice(string path) => _devicePath = path;
}