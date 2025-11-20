using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorGFF.Builder;

public class GFFListBuilder
{
    private readonly GFFList _list;

    internal GFFListBuilder(GFFList list)
    {
        _list = list;
    }

    public GFFListBuilder AddStruct(GFFStructID id = default, Action<GFFStructBuilder>? builder = null)
    {
        var @struct = _list.Add(id);
        if (builder is not null)
            builder(new(@struct));
        return this;
    }
}
