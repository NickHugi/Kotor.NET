using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorGFF.Builder;

public class GFFBuilder
{
    public GFF New(Action<GFFStructBuilder> builder)
    {
        var gff = new GFF();
        builder(new(gff.Root));
        return gff;
    }
}
