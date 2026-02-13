using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.OpenGL.Model;

namespace Kotor.NET.Graphics;

public interface IAssetManager
{
    public KModel GetModel(string model);
    public IShader GetShader(string shader);
    public ITexture GetTexture(string texture);

    void AddModel(string name, KModel model);
    void AddShader(string name, IShader shader);
    void AddTexture(string name, ITexture texture);

    bool HasModel(string name);
    bool HasTexture(string name);
}
