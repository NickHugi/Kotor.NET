using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser.Model;
using Kotor.NET.Patcher.Modifiers.ForTLK;
using Kotor.NET.Resources.KotorTLK;

namespace Kotor.NET.Patcher.Parsers.LegacyINI.ForTLK;

public class ParseEditEntry
{
    public EditEntryTLKModifier Parse(KeyData data)
    {
        return new EditEntryTLKModifier()
        {
            IndexIntoTargetTLK = int.Parse(data.KeyName),
            IndexIntoSourceTLK = int.Parse(data.Value)
        };
    }
}
