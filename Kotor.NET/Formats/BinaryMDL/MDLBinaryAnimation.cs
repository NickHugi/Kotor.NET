using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryAnimation
{
    public MDLBinaryAnimationHeader AnimationHeader { get; set; } = new();
    public MDLBinaryNode RootNode { get; set; } = new();
    public List<MDLBinaryAnimationEvent> Events { get; set; } = new();

    public MDLBinaryAnimation()
    {
    }
    public MDLBinaryAnimation(MDLBinaryReader reader, BinaryReader mdxReader)
    {
        AnimationHeader = new MDLBinaryAnimationHeader(reader);

        reader.SetStreamPosition(AnimationHeader.GeometryHeader.RootNodeOffset);
        RootNode = new MDLBinaryNode(reader, mdxReader);

        reader.SetStreamPosition(AnimationHeader.OffsetToEventArray);
        for (int i = 0; i < AnimationHeader.EventCount; i++)
        {
            Events.Add(new MDLBinaryAnimationEvent(reader));
        }
    }

    public void Write(MDLBinaryWriter writer, BinaryWriter mdxWriter)
    {
        AnimationHeader.Write(writer);

        writer.SetStreamPosition(AnimationHeader.GeometryHeader.RootNodeOffset);
        RootNode.Write(writer, mdxWriter);

        writer.SetStreamPosition(AnimationHeader.OffsetToEventArray);
        foreach (var animationEvent in Events)
        {
            animationEvent.Write(writer);
        }
    }
}
