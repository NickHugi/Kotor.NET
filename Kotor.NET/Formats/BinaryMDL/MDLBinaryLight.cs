using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Resources;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryLight
{
    public List<float> FlareSizes { get; set; } = new();
    public List<float> FlarePositions { get; set; } = new();
    public List<Vector3> FlareColourShifts { get; set; } = new();
    public List<int> FlareTextureNameOffsets { get; set; } = new();
    public List<string> FlareTextures { get; set; } = new();

    public MDLBinaryLight()
    {
    }
    public MDLBinaryLight(MDLBinaryLightHeader lightHeader, MDLBinaryReader reader)
    {
        reader.SetStreamPosition(lightHeader.OffsetToFlareSizeArray);
        for (int i = 0; i < lightHeader.FlareSizeArrayCount; i++)
        {
            FlareSizes.Add(reader.ReadSingle());
        }

        reader.SetStreamPosition(lightHeader.OffsetToFlarePositionArray);
        for (int i = 0; i < lightHeader.FlarePositionArrayCount; i++)
        {
            FlarePositions.Add(reader.ReadSingle());
        }

        reader.SetStreamPosition(lightHeader.OffsetToFlareColorShiftArray);
        for (int i = 0; i < lightHeader.FlareColorShiftArrayCount; i++)
        {
            FlareColourShifts.Add(reader.ReadVector3());
        }

        reader.SetStreamPosition(lightHeader.OffsetToFlareTextureNameOffsetsArray);
        for (int i = 0; i < lightHeader.FlareTextureNameCount; i++)
        {
            FlareTextureNameOffsets.Add(reader.ReadInt32());
        }

        foreach (var textureNameOffset in FlareTextureNameOffsets)
        {
            reader.SetStreamPosition(textureNameOffset);
            FlareTextures.Add(reader.ReadTerminatedString('\0'));
        }
    }

    public void Write(MDLBinaryLightHeader lightHeader, MDLBinaryWriter writer)
    {
        writer.SetStreamPosition(lightHeader.OffsetToFlareSizeArray);
        foreach(var flareSize in FlareSizes)
        {
            writer.Write(flareSize);
        }

        writer.SetStreamPosition(lightHeader.OffsetToFlarePositionArray);
        foreach (var flarePosition in FlarePositions)
        {
            writer.Write(flarePosition);
        }

        writer.SetStreamPosition(lightHeader.OffsetToFlareColorShiftArray);
        foreach (var flareColourShift in FlareColourShifts)
        {
            writer.Write(flareColourShift);
        }

        writer.SetStreamPosition(lightHeader.OffsetToFlareTextureNameOffsetsArray);
        foreach (var flareTextureOffset in FlareTextureNameOffsets)
        {
            writer.Write(flareTextureOffset);
        }

        for (int i = 0; i < FlareTextureNameOffsets.Count(); i++) 
        {
            writer.SetStreamPosition(FlareTextureNameOffsets[i]);
            writer.Write(FlareTextures[i] + '\0');
        }
    }
}
