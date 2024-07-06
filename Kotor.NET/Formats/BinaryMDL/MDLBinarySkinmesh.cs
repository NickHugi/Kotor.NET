using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinarySkinmesh
{
    public List<float> Bonemap { get; set; } = new();
    public List<Vector3> TBones { get; set; } = new();
    public List<Vector4> QBones { get; set; } = new();
    public List<float> Array8 { get; set; } = new();

    public MDLBinarySkinmesh()
    {
    }
    public MDLBinarySkinmesh(MDLBinarySkinmeshHeader skinHeader, MDLBinaryReader reader)
    {
        reader.SetStreamPosition(skinHeader.BonemapOffset);
        for (int i = 0; i < skinHeader.BonemapCount; i++)
        {
            var index = reader.ReadSingle();
            Bonemap.Add(index);
        }

        reader.SetStreamPosition(skinHeader.TBonesOffset);
        for (int i = 0; i < skinHeader.TBonesCount; i++)
        {
            var tbone = reader.ReadVector3();
            TBones.Add(tbone);
        }

        reader.SetStreamPosition(skinHeader.QBonesOffset);
        for (int i = 0; i < skinHeader.QBonesCount; i++)
        {
            var qbone = reader.ReadVector4();
            QBones.Add(qbone);
        }

        reader.SetStreamPosition(skinHeader.Array8Offset);
        for (int i = 0; i < skinHeader.Array8Count; i++)
        {
            var value = reader.ReadSingle();
            Array8.Add(value);
        }
    }

    public void Write(MDLBinarySkinmeshHeader skinHeader, MDLBinaryWriter writer)
    {
        writer.SetStreamPosition(skinHeader.BonemapOffset);
        foreach (var index in Bonemap)
        {
            writer.Write(index);
        }

        writer.SetStreamPosition(skinHeader.TBonesOffset);
        foreach (var tbone in TBones)
        {
            writer.Write(tbone);
        }

        writer.SetStreamPosition(skinHeader.QBonesOffset);
        foreach (var qbone in QBones)
        {
            writer.Write(qbone);
        }

        writer.SetStreamPosition(skinHeader.Array8Offset);
        foreach (var value in Array8)
        {
            writer.Write(value);
        }
    }
}
