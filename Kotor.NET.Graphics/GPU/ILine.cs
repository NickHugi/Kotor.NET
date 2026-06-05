using Kotor.NET.Graphics.Renderers.Descriptors;

namespace Kotor.NET.Graphics.GPU;

public interface ILine : IDisposable
{
    public unsafe void Draw(IEnumerable<LineDescriptor> lines);
}
