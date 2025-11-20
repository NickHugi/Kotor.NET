using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser.Model;
using Kotor.NET.Patcher.Extensions;
using Kotor.NET.Patcher.Modifiers.ForGFF;
using Kotor.NET.Patcher.Modifiers.ForGFF.Directive;
using Kotor.NET.Patcher.Modifiers.ForGFF.Modifiers;
using Kotor.NET.Patcher.Modifiers.ForGFF.Values;

namespace Kotor.NET.Patcher.Parsers.LegacyINI.ForGFF;

public class ParseModifyExistingField
{
    public EditFieldGFFModifier Parse(KeyData entry)
    {
        return new()
        {
            Label = ParseLabel(entry),
            Path = ParsePath(entry),
            Value = ParseValue(entry),
            LocalizedStringDirective = ParseDirective(entry)
        };
    }

    private string ParseLabel(KeyData entry)
    {
        var label = entry.KeyName.Split(@"\").Last();

        if (label.Contains("("))
        {
            label = label.Substring(0, label.IndexOf("("));
        }

        return label;
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

    private ILocalizedStringDirective? ParseDirective(KeyData entry)
    {
        var directiveString = entry.KeyName.GetStringBetween("(", ")");

        if (directiveString is null)
        {
            return null;
        }
        else if (directiveString.StartsWith("lang"))
        {
            return new SubstringLocalizedStringDirective()
            {
                LanguageID = int.Parse(directiveString.Replace("lang", ""))
            };
        }
        else if (directiveString.StartsWith("strref"))
        {
            return new StringRefLocalizedStringDirective();
        }
        else
        {
            return null;
        }
    }
}
