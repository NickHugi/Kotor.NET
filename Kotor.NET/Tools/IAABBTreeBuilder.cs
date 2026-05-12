using Kotor.NET.Common.Data.Geometry;
using Kotor.NET.Resources.KotorBWM;

namespace Kotor.NET.Tools;

public interface IAABBTreeBuilder
{
    public AABBNode Build(List<IFace> faces);
}

