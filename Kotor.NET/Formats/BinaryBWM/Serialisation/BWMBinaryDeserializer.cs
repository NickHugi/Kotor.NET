using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data.Geometry;
using Kotor.NET.Resources.KotorBWM;

namespace Kotor.NET.Formats.BinaryBWM.Serialisation;

public class BWMBinaryDeserializer
{
    private BWMBinary _binary { get; }

    public BWMBinaryDeserializer(BWMBinary binary)
    {
        _binary = binary;
    }

    public BWM Deserialize()
    {
        var bwm = new BWM();

        for (int i = 0; i < _binary.FaceIndices.Count; i ++)
        {
            var indices = _binary.FaceIndices[i];
            bwm.Faces.Add(
                _binary.Vertices[indices.Index1].Clone(),
                _binary.Vertices[indices.Index2].Clone(),
                _binary.Vertices[indices.Index3].Clone(),
                (SurfaceMaterial)_binary.FaceMaterials[i]
            );
        }

        for (int i = 0; i < _binary.Edges.Count / 3; i++)
        {
            var edge = i * 3;
            bwm.Faces[i].Edge1.Transition = _binary.Edges[edge+0].Transition;
            bwm.Faces[i].Edge2.Transition = _binary.Edges[edge+1].Transition;
            bwm.Faces[i].Edge3.Transition = _binary.Edges[edge+2].Transition;
        }

        return bwm;
    }
}
