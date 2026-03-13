using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Model;

namespace Kotor.NET.Graphics.Entities;

public abstract class Entity
{
    public Scene Scene { get; internal set; }
    public int ID => Scene.Entities.ToList().IndexOf(this);

    public Matrix4x4 Transformation { get; set; } = Matrix4x4.Identity;

    public abstract void Update(IAssetManager assetManager, float delta);

    public virtual ICollection<MeshDescriptor> GetMeshDescriptors(IAssetManager assets)
    {
        return [];
    }
}
