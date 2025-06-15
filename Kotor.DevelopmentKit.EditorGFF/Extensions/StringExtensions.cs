using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Extensions;

public static class StringExtensions
{
    public static string GetUniqueLabel(this string label, IStructGFFNodeViewModel? structNode)
    {
        if (structNode is null)
            return label;
        if (structNode.GetField(label) is null)
            return label;

        int count = 1;
        var newLabel = label;

        do
        {
            count++;

            var suffix = " (" + count + ")";
            newLabel = label + suffix;
        } while (structNode.GetField(newLabel) is not null);

        return newLabel;
    }
}
