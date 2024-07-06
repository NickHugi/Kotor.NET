using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryDanglymesh
{
    public List<float> Constraints { get; set; } = new();
    public List<Vector3> Data { get; set; } = new();

    public MDLBinaryDanglymesh(MDLBinaryDanglyHeader danglymeshHeader, MDLBinaryReader reader)
    {
        reader.SetStreamPosition(danglymeshHeader.OffsetToContraintArray);
        for (int i = 0; i < danglymeshHeader.ContraintArrayCount; i++)
        {
            var constraint = reader.ReadSingle();
            Constraints.Add(constraint);
        }

        reader.SetStreamPosition(danglymeshHeader.OffsetToDataArray);
        for (int i = 0; i < danglymeshHeader.ContraintArrayCount; i++)
        {
            var data = reader.ReadVector3();
            Data.Add(data);
        }
    }

    public void Write(MDLBinaryDanglyHeader danglymeshHeader, MDLBinaryWriter writer)
    {
        writer.SetStreamPosition(danglymeshHeader.OffsetToContraintArray);
        foreach (var constraint in Constraints)
        {
            writer.Write(constraint);
        }

        writer.SetStreamPosition(danglymeshHeader.OffsetToDataArray);
        foreach (var data in Data)
        {
            writer.Write(data);
        }
    }
}
