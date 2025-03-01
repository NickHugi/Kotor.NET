using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Patcher.Modifiers;

public class PatcherMemory
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

    public bool TryGet(string key, out string value)
    {
        if (Contains(key))
        {
            value = Get(key);
            return true;
        }
        else
        {
            value = "";
            return false;
        }
    }
    public bool TryGet(string key, out int value)
    {
        if (Contains(key))
        {
            var valueAsString = Get(key);
            return int.TryParse(valueAsString, out value);
        }
        else
        {
            value = 0;
            return false;
        }
    }

    public void Set(string key, string value)
    {
        _values[key] = value;
    }
}
