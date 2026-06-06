using System.Numerics;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.Factories;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.Renderers.Descriptors;

namespace Kotor.NET.Graphics.Renderers;

public class GeometryRenderer : IRenderer
{
    public void Render(IAssetManager assets, IEnumerable<IDrawCallDescriptor> descriptors, Camera camera, Vector2 viewport)
    {
        var shader = assets.GetShader("basic");

        shader.Activate();
        shader.SetMatrix4x4("uProjection", camera.GetProjectionTransform((uint)viewport.X, (uint)viewport.Y));
        shader.SetMatrix4x4("uView", camera.GetViewTransform());
        shader.SetMatrix4x4("uMesh", Matrix4x4.Identity);
        shader.SetUniform1("uTexture1", 0);
        shader.SetUniform1("uTexture2", 1);

        descriptors.OfType<MeshDescriptor>().ToList().ForEach(x => Render(assets, shader, x));
    }

    private void Render(IAssetManager assets, IShader shader, MeshDescriptor descriptor)
    {
        if (!descriptor.DoRender)
            return;

        shader.SetMatrix4x4("uEntity", Matrix4x4.Identity);
        shader.SetMatrix4x4("uMesh", descriptor.Transform);
        shader.SetMatrix4x4Array("uFinalBonesMatrices", descriptor.BoneTransforms);
        shader.SetUniform3("uDiffuse", descriptor.DiffuseColor);
        shader.SetUniform3("uAmbient", descriptor.AmbientColor);
        shader.SetUniform1("uPickerID", descriptor.PickerID);

        var texturePlaceholder = assets.GetTexture("placeholder");
        var texture1 = assets.GetTexture(descriptor.Texture1);
        var texture2 = assets.GetTexture(descriptor.Texture2);
        if (texture1 is null) texturePlaceholder.Activate(0); else texture1.Activate(0);
        if (texture2 is null) texturePlaceholder.Activate(1); else texture2.Activate(1);

        descriptor.Mesh.Draw();
    }
}
