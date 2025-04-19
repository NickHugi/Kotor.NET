using System;
using System.Collections.Generic;
using DynamicData;

namespace Kotor.DevelopmentKit.Base;

/// <summary>
/// Represents a index-based comparison on a SourceList object.
/// </summary>
/// <typeparam name="T">The type of elements within the source list.</typeparam>
public class SourceListIndexComparer<T> : IComparer<T> where T : notnull
{
    private SourceList<T> _source;

    public SourceListIndexComparer(SourceList<T> sauce)
    {
        _source = sauce;
    }

    public int Compare(T? x, T? y)
    {
        var a = _source.Items.IndexOf(x);
        var b = _source.Items.IndexOf(y);
        return a - b;
    }
}
