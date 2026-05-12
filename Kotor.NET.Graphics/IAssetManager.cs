using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.OpenGL.Model;

namespace Kotor.NET.Graphics;

public interface IAssetManager : IDisposable
{
    IMesh Quad { get; }

    public KModel GetModel(string model);
    public void AddModel(string name, KModel model);
    public void RemoveModel(string name);
    public bool HasModel(string name);

    public ITexture GetTexture(string texture);
    public void AddTexture(string name, ITexture texture);
    public void RemoveTexture(string name);
    public bool HasTexture(string name);

    public IShader GetShader(string shader);
    public void AddShader(string name, IShader shader);
    public void RemoveShader(string name);
    public bool HasShader(string name);
}
