using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data.Geometry;
using Kotor.NET.Resources.KotorBWM;

namespace Kotor.NET.Resources.KotorMDL.Nodes;

public class MDLWalkmeshNode : MDLTrimeshNode
{
    public AABBNode? RootNode { get; set; }

    public MDLWalkmeshNode(string name) : base(name)
    {
    }

    public BWM GenerateBWM()
    {
        var bwm = new BWM(BWMWalkmeshType.Area);

        foreach (var face in Faces)
        {
            bwm.Faces.Add(face.Point1, face.Point2, face.Point3, face.Material);
        }

        return bwm;
    }
}
