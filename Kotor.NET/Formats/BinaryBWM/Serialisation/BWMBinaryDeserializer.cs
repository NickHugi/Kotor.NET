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
        var type = _binary.FileHeader.WalkmeshType == 1
            ? BWMWalkmeshType.Area
            : BWMWalkmeshType.Placeable;

        var bwm = new BWM(type);
        bwm.Position = _binary.FileHeader.Position;

        for (int i = 0; i < _binary.FaceIndices.Count; i ++)
        {
            var indices = _binary.FaceIndices[i];
            bwm.Faces.Add(
                _binary.Vertices[indices.Index1],
                _binary.Vertices[indices.Index2],
                _binary.Vertices[indices.Index3],
                (SurfaceMaterial)_binary.FaceMaterials[i]
            );
        }

        foreach (var edge in _binary.Edges)
        {
            var faceIndex = edge.EdgeIndex / 3;
            var edgeIndex = edge.EdgeIndex % 3;

            if (edgeIndex == 0)
                bwm.Faces[faceIndex].Edge1.Transition = edge.Transition;
            if (edgeIndex == 1)
                bwm.Faces[faceIndex].Edge2.Transition = edge.Transition;
            if (edgeIndex == 2)
                bwm.Faces[faceIndex].Edge3.Transition = edge.Transition;
        }

        return bwm;
    }
}
