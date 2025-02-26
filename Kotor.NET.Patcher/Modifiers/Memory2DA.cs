using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Patcher.Modifiers;

public class Memory2DA
{
    private Dictionary<string, string> _values = new();

    public bool Contains(string key)
    {
        return _values.ContainsKey(key);
    }

    public string Get(string key)
    {
        return _values[key];
    }

    public void Set(string key, string value)
    {
        _values[key] = value;
    }
}
