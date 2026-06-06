using System.Numerics;

namespace Kotor.NET.Graphics.Renderers.Descriptors;

public class LineDescriptor : IDrawCallDescriptor
{
    public required Vector3 Start { get; init; }
    public required Vector3 End { get; init; }
    public Vector4 Color { get; init; } = new(1, 1, 1, 1);
    public required float Thickness { get; init; }
    public object? Tag { get; set; }
}
