using Kotor.NET.Common.Data.Geometry;

namespace Kotor.NET.Tools;

public interface IAABBTreeBuilder
{
    public AABBNode Build(List<Face> faces);
}

