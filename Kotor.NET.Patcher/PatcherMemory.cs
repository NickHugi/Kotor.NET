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

    public byte GetByte(string key)
    {
        if (!Contains(key))
        {
            throw new PatchingException($"No value is stored in memory for key '{key}'.");
        }
        else if (byte.TryParse(_values[key], out var value))
        {
            return value;
        }
        else
        {
            throw new PatchingException($"Failed to convert value stored in '{key}' to a UInt8.");
        }
    }
    public sbyte GetSByte(string key)
    {
        if (!Contains(key))
        {
            throw new PatchingException($"No value is stored in memory for key '{key}'.");
        }
        else if (sbyte.TryParse(_values[key], out var value))
        {
            return value;
        }
        else
        {
            throw new PatchingException($"Failed to convert value stored in '{key}' to a Int8.");
        }
    }
    public ushort GetUShort(string key)
    {
        if (!Contains(key))
        {
            throw new PatchingException($"No value is stored in memory for key '{key}'.");
        }
        else if (ushort.TryParse(_values[key], out var value))
        {
            return value;
        }
        else
        {
            throw new PatchingException($"Failed to convert value stored in '{key}' to a UInt16.");
        }
    }
    public short GetShort(string key)
    {
        if (!Contains(key))
        {
            throw new PatchingException($"No value is stored in memory for key '{key}'.");
        }
        else if (short.TryParse(_values[key], out var value))
        {
            return value;
        }
        else
        {
            throw new PatchingException($"Failed to convert value stored in '{key}' to a Int16.");
        }
    }
    public uint GetUInt(string key)
    {
        if (!Contains(key))
        {
            throw new PatchingException($"No value is stored in memory for key '{key}'.");
        }
        else if (uint.TryParse(_values[key], out var value))
        {
            return value;
        }
        else
        {
            throw new PatchingException($"Failed to convert value stored in '{key}' to a UInt32.");
        }
    }
    public int GetInt(string key)
    {
        if (!Contains(key))
        {
            throw new PatchingException($"No value is stored in memory for key '{key}'.");
        }
        else if (int.TryParse(_values[key], out var value))
        {
            return value;
        }
        else
        {
            throw new PatchingException($"Failed to convert value stored in '{key}' to a Int32.");
        }
    }
    public ulong GetULong(string key)
    {
        if (!Contains(key))
        {
            throw new PatchingException($"No value is stored in memory for key '{key}'.");
        }
        else if (ulong.TryParse(_values[key], out var value))
        {
            return value;
        }
        else
        {
            throw new PatchingException($"Failed to convert value stored in '{key}' to a UInt64.");
        }
    }
    public long GetLong(string key)
    {
        if (!Contains(key))
        {
            throw new PatchingException($"No value is stored in memory for key '{key}'.");
        }
        else if (long.TryParse(_values[key], out var value))
        {
            return value;
        }
        else
        {
            throw new PatchingException($"Failed to convert value stored in '{key}' to a Int64.");
        }
    }
    public float GetFloat(string key)
    {
        if (!Contains(key))
        {
            throw new PatchingException($"No value is stored in memory for key '{key}'.");
        }
        else if (float.TryParse(_values[key], out var value))
        {
            return value;
        }
        else
        {
            throw new PatchingException($"Failed to convert value stored in '{key}' to a Single.");
        }
    }
    public double GetDouble(string key)
    {
        if (!Contains(key))
        {
            throw new PatchingException($"No value is stored in memory for key '{key}'.");
        }
        else if (double.TryParse(_values[key], out var value))
        {
            return value;
        }
        else
        {
            throw new PatchingException($"Failed to convert value stored in '{key}' to a Double.");
        }
    }
    public string GetString(string key)
    {
        if (!Contains(key))
        {
            throw new PatchingException($"No value is stored in memory for key '{key}'.");
        }
        else
        {
            return _values[key];
        }
    }

    public string Get(string key)
    {
        if (!Contains(key))
            throw new PatchingException($"No value is stored in memory for key '{key}'.");

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
