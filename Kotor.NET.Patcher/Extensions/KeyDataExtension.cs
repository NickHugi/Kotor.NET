using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser.Model;
using Kotor.NET.Patcher.Modifiers.For2DA.CellValues;

namespace Kotor.NET.Patcher.Extensions;

public static class KeyDataExtension
{
    public static BaseValueResolver GetValueResolver(this KeyData entry)
    {
        if (entry.Value == "high()")
        {
            return new ValueResolverForHighestUnderColumn() { ColumnHeader = entry.KeyName };
        }
        else if (entry.Value == "RowIndex")
        {
            return new ValueResolverForTargetRowIndex();
        }
        else if (entry.Value == "RowLabel")
        {
            return new ValueResolverForTargetRowHeader();
        }
        else if (entry.Value == "2DAMEMORY")
        {
            return new ValueResolverForPatcherMemory() { Key = entry.KeyName };
        }
        else if (entry.Value == "StrRef")
        {
            return new ValueResolverForPatcherMemory() { Key = entry.KeyName };
        }
        else
        {
            return new ValueResolverForConstant() { Value = entry.Value };
        }
    }
}
