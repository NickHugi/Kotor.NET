using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.Modifiers.ForGFF;

public class BindingPath
{
    public IEnumerable<string> Navigation { get; }

    public BindingPath(string path)
    {
        Navigation = path.Split(@"\");
    }
    public BindingPath(IEnumerable<string> navigation)
    {
        Navigation = navigation.ToArray();
    }

    public GFFStruct ResolveStruct(GFFStruct start)
    {
        if (Navigate(start) is GFFStruct end)
        {
            return end;
        }
        else
        {
            throw new PatchingException($"Failed to navigate to struct at '{this}'.");
        }
    }

    public GFFList ResolveList(GFFStruct start)
    {
        if (Navigate(start) is GFFList end)
        {
            return end;
        }
        else
        {
            throw new PatchingException($"Failed to navigate to list at '{this}'.");
        }
    }

    public override string ToString()
    {
        return string.Join(@"\", Navigation);
    }

    private object Navigate(GFFStruct start)
    {
        object location = start;

        foreach (var step in Navigation)
        {
            if (location is GFFStruct @struct)
            {
                location = Navigate(@struct, step);
            }
            else if (location is GFFList @list)
            {
                location = Navigate(@list, step);
            }
        }

        return location;
    }
    private object Navigate(GFFStruct @struct, string step)
    {
        return @struct.GetStruct(step) ?? throw new PatchingException($"Failed to navigate to '{this}'.");
    }
    private object Navigate(GFFList @list, string step)
    {
        if (int.TryParse(step, out var listIndex))
        {
            return @list.ElementAt(listIndex) ?? throw new PatchingException($"Failed to navigate to '{this}'.");
        }
        else
        {
            return new PatchingException($"Failed to navigate to '{this}'.");
        }
    }
}
