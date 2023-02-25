using Ghost_Guard.Models.Infrastructure;
using Ghost_Guard.Models.Infrastructure.Serialization;

namespace Ghost_Guard.Models.Application;

public class VersionsProvider : IVersionsProvider
{
    private readonly Dictionary<HashIndex, int> _data;
    private readonly IFileAdapter<KeyValuePair<HashIndex, int>> _adapter;

    public VersionsProvider()
    {
        _adapter = DI.Container.GetInstance<IFileAdapter<KeyValuePair<HashIndex, int>>>();
        _data = new Dictionary<HashIndex, int>(_adapter.Read());
    }

    public int GetModifier(HashIndex hash) 
        => _data.TryGetValue(hash, out int modifier) ? modifier : 0;

    public void UpgradeModifier(HashIndex hash) => ChangeModifier(hash, 1);

    public void DowngradeModifier(HashIndex hash) => ChangeModifier(hash, -1);

    private void ChangeModifier(HashIndex hash, int amount)
    {
        if (_data.TryGetValue(hash, out int modifier))
        {
            modifier += amount;

            if (modifier < 0)
                modifier = 0;

            _data[hash] = modifier; 
        }
        else
        {
            if (amount < 0)
                return;

            _data[hash] = amount;
        }

        _adapter.Write(_data);
    }

    public void Clear()
    {
        foreach (var hash in _data.Keys)
        {
            _data[hash] = 0;
            hash.Clear();
        }
    }
}
