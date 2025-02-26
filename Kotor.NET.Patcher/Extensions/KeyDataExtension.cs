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
    public static ICellValue GetCellValue(this KeyData entry)
    {
        if (entry.Value == "high()")
        {
            return new CellValueHighest() { ColumnHeader = entry.KeyName };
        }
        else if (entry.Value == "RowIndex")
        {
            return new CellValueRowIndex();
        }
        else if (entry.Value == "RowLabel")
        {
            return new CellValueRowHeader();
        }
        else if (entry.Value == "2DAMEMORY")
        {
            return new CellValue2DAMemory() { Key = entry.KeyName };
        }
        else if (entry.Value == "StrRef")
        {
            return new CellValueTLKMemory() { Key = entry.KeyName };
        }
        else
        {
            return new CellValueConstant() { Value = entry.Value };
        }
    }
}
