using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser.Model;
using Kotor.NET.Patcher.Modifiers.ForSSF;
using Kotor.NET.Patcher.Modifiers.ForTLK.Modifiers;
using Kotor.NET.Resources.KotorTLK;

namespace Kotor.NET.Patcher.Parsers.LegacyINI.ForTLK;

public class ParseAddEntry
{
    public AddEntryTLKModifier Parse(KeyData data)
    {
        return new AddEntryTLKModifier()
        {
            Key = data.KeyName,
            IndexIntoSourceTLK = int.Parse(data.Value)
        };
    }
}
