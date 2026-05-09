using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Common.Data.Geometry;

public abstract class AABBNode
{
    public BoundingBox BoundingBox { get; init; }

    public AABBNode(BoundingBox boundingBox)
    {
        BoundingBox = boundingBox;
    }

    public abstract IEnumerable<AABBNode> GetDescendants();
}
