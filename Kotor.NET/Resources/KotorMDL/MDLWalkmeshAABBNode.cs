using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Resources.KotorMDL;

public class MDLWalkmeshAABBNode(BoundingBox boundingBox, MDLFace? face = null, MDLWalkmeshAABBNode? left = null, MDLWalkmeshAABBNode? right = null, MDLWalkmeshAABBNodeMostSignificantPlane mostSignificantPlane = MDLWalkmeshAABBNodeMostSignificantPlane.None)
{
    public BoundingBox BoundingBox { get; set; } = boundingBox;
    public MDLFace? Face { get; set; } = face;
    public MDLWalkmeshAABBNodeMostSignificantPlane MostSignificantPlane { get; set; } = mostSignificantPlane;
    public MDLWalkmeshAABBNode? LeftChild { get; set; } = left;
    public MDLWalkmeshAABBNode? RightChild { get; set; } = right;
}

public enum MDLWalkmeshAABBNodeMostSignificantPlane
{
    None = 0,
    PositiveX = 1,
    PositiveY = 2,
    PositiveZ = 4,
}
