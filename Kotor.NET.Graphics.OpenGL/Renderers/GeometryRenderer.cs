using System.Numerics;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.Model;

namespace Kotor.NET.Graphics.OpenGL.Renderers;

public class GeometryRenderer : IRenderer
{
    public void Render(IAssetManager assets, Scene scene, Camera camera, uint width, uint height)
    {
        var descriptors = new List<MeshDescriptor>();

        foreach (var entity in scene.Entities)
        {
            if (entity is AnimatedModel prop)
            {
                descriptors.AddRange(GetRenderablesForModel(assets, prop.Animations, prop.Model));
            }
        }

        var shader = assets.GetShader("basic");

        shader.Activate();
        shader.SetMatrix4x4("projection", camera.GetProjectionTransform(width, height));
        shader.SetMatrix4x4("view", camera.GetViewTransform());
        shader.SetMatrix4x4("mesh", Matrix4x4.Identity);
        shader.SetUniform1("texture1", 0);
        shader.SetUniform1("texture2", 1);

        descriptors.ForEach(x => Render(assets, shader, x));
    }

    private List<MeshDescriptor> GetRenderablesForModel(IAssetManager assets, ICollection<AnimationItem> animations, string modelName)
    {
        var model = assets.GetModel(modelName);
        var nodes = model.GetAllNodes();

        model.Root.GenerateTransform(animations);

        return nodes.ToList().SelectMany(node => node.GetMeshDescriptors()).ToList();
    }
    private void Render(IAssetManager assets, IShader shader, MeshDescriptor descriptor)
    {
        if (!descriptor.DoRender)
            return;

        shader.SetMatrix4x4("entity", Matrix4x4.Identity);
        shader.SetMatrix4x4("mesh", descriptor.Transform);
        shader.SetMatrix4x4Array("finalBonesMatrices", descriptor.BoneTransforms);
        shader.SetUniform1("entityID", descriptor.EntityID);

        var texturePlaceholder = assets.GetTexture("placeholder");
        var texture1 = assets.GetTexture(descriptor.Texture1);
        var texture2 = assets.GetTexture(descriptor.Texture2);
        if (texture1 is null) texturePlaceholder.Activate(); else texture1.Activate();
        //if (texture2 is null) texturePlaceholder.Activate(); else texture2.Activate();

        descriptor.Mesh.Draw();
    }
}
