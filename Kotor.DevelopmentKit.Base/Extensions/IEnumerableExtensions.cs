using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Avalonia.Collections;

namespace Kotor.DevelopmentKit.Base.Extensions;

public static class IEnumerableExtensions
{
    public static AvaloniaDictionary<TKey, TElement> ToAvaloniaDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        where TKey : notnull
    {

        var d = new AvaloniaDictionary<TKey, TElement>();
        foreach (TSource element in source)
        {
            d.Add(keySelector(element), elementSelector(element));
        }

        return d;
    }
}
