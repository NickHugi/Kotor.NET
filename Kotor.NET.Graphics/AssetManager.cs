using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;

namespace Kotor.NET.Graphics;

public interface IAssetManager
{
    public IShader GetShader(string shader);
    public IVertexArrayObject GetVAO(string vao);
    public IModel GetModel(string model);
    public ITexture GetTexture(string texture);
}
