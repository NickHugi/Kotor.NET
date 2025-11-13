using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser.Model;
using Kotor.NET.Patcher.Modifiers.ForGFF;
using Kotor.NET.Patcher.Modifiers.ForGFF.Modifiers;
using Kotor.NET.Patcher.Modifiers.ForGFF.Values;

namespace Kotor.NET.Patcher.Parsers.LegacyINI.ForGFF;

public class ParseModifyExistingField
{
    public EditFieldUnknownGFFModifier Parse(KeyData entry)
    {
        return new()
        {
            Label = ParseLabel(entry),
            Path = ParsePath(entry),
            Value = ParseValue(entry),
        };
    }

    private string ParseLabel(KeyData entry)
    {
        return entry.KeyName.Split(@"\").Last();
    }

    private BindingPath ParsePath(KeyData entry)
    {
        var path = entry.KeyName.Split(@"\").SkipLast(1);
        return new BindingPath(path);
    }

    private BaseValue ParseValue(KeyData entry)
    {
        if (entry.Value.StartsWith("2DAMEMORY"))
        {
            return new ValueMemory { Key = entry.Value };
        }
        else if (entry.Value.StartsWith("StrRef"))
        {
            return new ValueMemory { Key = entry.Value };
        }
        else
        {
            return new ValueStringConstant { Value = entry.Value };
        }
    }
}
