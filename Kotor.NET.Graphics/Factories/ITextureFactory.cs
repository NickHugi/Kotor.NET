using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;

namespace Kotor.NET.Graphics.Factories;

public interface ITextureFactory
{
    public ITexture FromSource();
    public ITexture FromFile(string texture);
    public ITexture FromStream(Stream texture);
    public ITexture FromEmbeddedResource(string texture);
}
