using System.Numerics;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.Model;

namespace Kotor.NET.Graphics.Renderers;

public class GeometryRenderer : IRenderer
{
    public void Render(IAssetManager assets, Scene scene, Camera camera, uint width, uint height, Action<List<MeshDescriptor>> renderInterceptor)
    {
        var descriptors = scene.Entities.SelectMany(x => x.GetMeshDescriptors(assets)).ToList();

        if (renderInterceptor is not null)
            renderInterceptor(descriptors);

        var shader = assets.GetShader("basic");

        shader.Activate();
        shader.SetMatrix4x4("projection", camera.GetProjectionTransform(width, height));
        shader.SetMatrix4x4("view", camera.GetViewTransform());
        shader.SetMatrix4x4("mesh", Matrix4x4.Identity);
        shader.SetUniform1("texture1", 0);
        shader.SetUniform1("texture2", 1);

        descriptors.ForEach(x => Render(assets, shader, x));
    }

    private void Render(IAssetManager assets, IShader shader, MeshDescriptor descriptor)
    {
        if (!descriptor.DoRender)
            return;

        shader.SetMatrix4x4("entity", Matrix4x4.Identity);
        shader.SetMatrix4x4("mesh", descriptor.Transform);
        shader.SetMatrix4x4Array("finalBonesMatrices", descriptor.BoneTransforms);
        shader.SetUniform3("diffuse", descriptor.DiffuseColor);
        shader.SetUniform3("ambient", descriptor.AmbientColor);
        shader.SetUniform1("pickerID", descriptor.PickerID);

        var texturePlaceholder = assets.GetTexture("placeholder");
        var texture1 = assets.GetTexture(descriptor.Texture1);
        var texture2 = assets.GetTexture(descriptor.Texture2);
        if (texture1 is null) texturePlaceholder.Activate(0); else texture1.Activate(0);
        if (texture2 is null) texturePlaceholder.Activate(1); else texture2.Activate(1);

        descriptor.Mesh.Draw();
    }
}
