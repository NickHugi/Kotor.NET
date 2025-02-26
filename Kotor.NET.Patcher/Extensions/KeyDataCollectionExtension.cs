using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser.Model;
using Kotor.NET.Patcher.Parsers;

namespace Kotor.NET.Patcher.Extensions;

public static class KeyDataCollectionExtension
{
    public static bool TryGetInt32(this KeyDataCollection section, string key, out int value)
    {
        var entry = section.FirstOrDefault(x => x.KeyName.Equals(key, StringComparison.OrdinalIgnoreCase));
        value = 0;

        if (entry is null)
        {
            return false;
        }
        if (int.TryParse(entry.Value, out value))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool TryGetString(this KeyDataCollection section, string key, out string value)
    {
        var entry = section.FirstOrDefault(x => x.KeyName.Equals(key, StringComparison.OrdinalIgnoreCase));
        value = "";

        if (entry is null)
        {
            return false;
        }
        else
        {
            value = entry.Value;
            return true;
        }
    }
}
