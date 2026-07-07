using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.BinaryTPC.Serialisation;
using Kotor.NET.Graphics.Factories;
using Kotor.NET.Graphics.GPU;
using Silk.NET.OpenGL;

namespace Kotor.NET.Graphics.OpenGL.Factories;

public class TGATextureFactory(GL _gl) : ITextureFactory
{
    public ITexture FromEmbeddedResource(string texture)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var textureStream = assembly.GetManifestResourceStream(texture)!;
        return FromStream(textureStream);
    }
    public ITexture FromFile(string texture)
    {
        using var textureStream = File.OpenRead(texture);
        return FromStream(textureStream);
    }
    public unsafe ITexture FromStream(Stream stream)
    {
        var tpc = new TGABinaryDeserializer(new Formats.BinaryTGA.TGABinary(stream)).Deserialize();
        return new TPCTextureFactory(_gl).FromTPC(tpc);
    }
}
