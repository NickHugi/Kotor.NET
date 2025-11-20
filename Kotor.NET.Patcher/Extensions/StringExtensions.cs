using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Patcher.Extensions;

internal static class StringExtensions
{
    internal static string? GetStringBetween(this string input, string left, string right)
    {
        var indexLeft = input.IndexOf(left);
        var indexRight = input.IndexOf(right);

        if (indexLeft < indexRight && indexLeft != -1 && indexRight != -1)
        {
            return input.Substring(indexLeft + 1, indexRight - indexLeft - 1);
        }
        else
        {
            return null;
        }
    }
}
