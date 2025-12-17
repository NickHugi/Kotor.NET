using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;

namespace Kotor.NET.Graphics;

public interface IAssetManager
{
    public static IAssetManager Manager { get; set; }

    public IShader GetShader(string shader);
    public IModel GetModel(string model);
    public ITexture GetTexture(string texture);
    void AddModel(string name, IModel model);
    void AddShader(string name, IShader shader);
    void AddTexture(string name, ITexture texture);
}
