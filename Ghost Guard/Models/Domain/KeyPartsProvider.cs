using Ghost_Guard.Models.Infrastructure;
using Ghost_Guard.Models.Infrastructure.Serialization;

namespace Ghost_Guard.Models.Domain;

public class KeyPartsProvider<TKey>
{
    private readonly Dictionary<HashIndex, TKey> _indexedKeys;
    private readonly IFileAdapter<KeyValuePair<HashIndex, TKey>> _adapter;
    private readonly Func<TKey, TKey> _clear;

    private bool _initialized;

    private Dictionary<HashIndex, TKey> IndexedKeys
    {
        get
        {
            if (_initialized) 
                return _indexedKeys;
            
            foreach (var pair in _adapter.Read())
                _indexedKeys.Add(pair.Key, pair.Value);

            _initialized = true;
            return _indexedKeys;
        }
    }

    public IEnumerable<HashIndex> HashIndexes => _indexedKeys.Keys;
    
    public KeyPartsProvider(Func<TKey?, TKey> clear)
    {
        _adapter = DI.Container.GetInstance<IFileAdapter<KeyValuePair<HashIndex, TKey>>>();
        _clear = clear;
        _indexedKeys = new Dictionary<HashIndex, TKey>();
        _initialized = false;
    }

    public bool Contains(HashIndex hashIndex) => _indexedKeys.ContainsKey(hashIndex);

    public TKey TakeKey(HashIndex hashIndex)
    {
        if (!Contains(hashIndex))
            throw new ArgumentException("Hash index is wrong");

        var result = IndexedKeys[hashIndex];
        IndexedKeys.Remove(hashIndex);
        _adapter.Write(IndexedKeys);
        return result;
    }

    public void Add(TKey key, HashIndex index)
    {
        if (index is null)
            throw new ArgumentNullException();
        
        _indexedKeys.Add(index, key);
        _adapter.Write(_indexedKeys);
    }

    public void Clear()
    {
        foreach (var key in IndexedKeys.Keys)
            key.Clear();

        foreach (var value in IndexedKeys.Values)
            _clear.Invoke(value);
    }
}
