using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.OpenGL.Model;

namespace Kotor.NET.Graphics.OpenGL;

public class AssetManager : IAssetManager
{
    private readonly Dictionary<string, KModel> _models = new();
    private readonly Dictionary<string, IShader> _shaders = new();
    private readonly Dictionary<string, ITexture> _textures = new();

    public void AddModel(string name, KModel model)
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

    public KModel GetModel(string name)
    {
        return _models[name.ToLower()];
    }

    public IShader GetShader(string name)
    {
        return _shaders[name.ToLower()];
    }

    public ITexture? GetTexture(string name)
    {
        return _textures.TryGetValue(name.ToLower(), out var texture) ? texture : null;
    }

    public bool HasModel(string name)
    {
        return _models.TryGetValue(name.ToLower(), out var _);
    }

    public bool HasTexture(string name)
    {
        return _textures.TryGetValue(name.ToLower(), out var _);
    }

    public void UnloadAll()
    {
        // TODO
    }
}
