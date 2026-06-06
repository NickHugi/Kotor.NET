using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics.Renderers.Descriptors;

public class BillboardDescriptor : IDrawCallDescriptor
{
    public required bool DoRender { get; set; }
    public required string Image { get; set; }
    public required Vector3 Location { get; set; }
    public required float Size { get; set; }
    public required bool FixedSize { get; set; }
}
