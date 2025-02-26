using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Patcher.Modifiers;

public class MemoryTLK
{
    private Dictionary<string, int> _values = new();

    public bool Contains(string key)
    {
        return _values.ContainsKey(key);
    }

    public int Get(string key)
    {
        return _values[key];
    }

    public void Set(string key, int value)
    {
        _values[key] = value;
    }
}
