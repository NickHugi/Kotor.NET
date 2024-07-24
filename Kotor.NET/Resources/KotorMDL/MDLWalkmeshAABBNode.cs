using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Resources.KotorMDL;

public class MDLWalkmeshAABBNode(BoundingBox boundingBox, MDLFace? face = null, MDLWalkmeshAABBNode? left = null, MDLWalkmeshAABBNode? right = null)
{
    public BoundingBox BoundingBox { get; set; } = boundingBox;
    public MDLFace? Face { get; set; } = face;
    public MDLWalkmeshAABBNode? LeftChild { get; set; } = left;
    public MDLWalkmeshAABBNode? RightChild { get; set; } = right;
}
