using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.OpenGL.Model;

namespace Kotor.NET.Graphics.OpenGL;

public class AssetManager : IAssetManager
{
    private readonly Dictionary<string, KModel> _models = new();
    private readonly Dictionary<string, IShader> _shaders = new();
    private readonly Dictionary<string, ITexture> _textures = new();


    public KModel GetModel(string name)
    {
        return _models[name.ToLower()];
    }

    public void AddModel(string name, KModel model)
    {
        _models.Add(name.ToLower(), model);
    }

    public void RemoveModel(string name)
    {
        GetModel(name).Dispose();
        _models.Remove(name.ToLower());
    }

    public bool HasModel(string name)
    {
        return _models.TryGetValue(name.ToLower(), out var _);
    }


    public ITexture? GetTexture(string name)
    {
        return _textures.TryGetValue(name.ToLower(), out var texture) ? texture : null;
    }

    public void AddTexture(string name, ITexture texture)
    {
        _textures.Add(name.ToLower(), texture);
    }

    public void RemoveTexture(string name)
    {
        GetTexture(name.ToLower()).Dispose();
        _textures.Remove(name.ToLower());
    }

    public bool HasTexture(string name)
    {
        return _textures.TryGetValue(name.ToLower(), out var _);
    }


    public IShader GetShader(string name)
    {
        return _shaders[name.ToLower()];
    }

    public void AddShader(string name, IShader shader)
    {
        _shaders.Add(name.ToLower(), shader);
    }

    public void RemoveShader(string name)
    {
        GetShader(name.ToLower()).Dispose();
        _shaders.Remove(name.ToLower());
    }

    public bool HasShader(string name)
    {
        return _shaders.TryGetValue(name.ToLower(), out var _);
    }
}
