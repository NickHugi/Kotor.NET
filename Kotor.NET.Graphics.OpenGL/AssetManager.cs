using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;

namespace Kotor.NET.Graphics.OpenGL;

public class AssetManager : IAssetManager
{
    private readonly Dictionary<string, IModel> _models = new();
    private readonly Dictionary<string, IShader> _shaders = new();
    private readonly Dictionary<string, ITexture> _textures = new();

    public void AddModel(string name, IModel model)
    {
        _models.Add(name.ToLower(), model);
    }

    public void AddShader(string name, IShader shader)
    {
        _shaders.Add(name.ToLower(), shader);
    }

    public void AddTexture(string name, ITexture texture)
    {
        _textures.Add(name.ToLower(), texture);
    }

    public IModel GetModel(string name)
    {
        return _models[name.ToLower()];
    }

    public IShader GetShader(string name)
    {
        return _shaders[name.ToLower()];
    }

    public ITexture GetTexture(string name)
    {
        return _textures[name.ToLower()];
    }

    public bool HasModel(string name)
    {
        return _models.TryGetValue(name.ToLower(), out var _);
    }

    public void UnloadAll()
    {
        // TODO
    }
}
