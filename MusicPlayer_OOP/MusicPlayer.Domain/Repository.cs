using System.Collections.Generic;
using System.Linq;

namespace MusicPlayer.Domain;

public class Repository<T> where T : class
{
    private readonly List<T> _items = new();

    public void Add(T item)
    {
        if (item is null) throw new System.ArgumentNullException(nameof(item));
        _items.Add(item);
    }

    public bool Remove(T item)
    {
        if (item is null) throw new System.ArgumentNullException(nameof(item));
        return _items.Remove(item);
    }

    public IReadOnlyList<T> GetAll() => _items.ToList().AsReadOnly();
}
