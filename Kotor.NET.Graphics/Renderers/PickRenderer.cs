using System.Numerics;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.Renderers.Descriptors;

namespace Kotor.NET.Graphics.Renderers;

public class PickRenderer : IRenderer
{
    public void Render(IAssetManager assets, IEnumerable<IDrawCallDescriptor> descriptors, Camera camera, Vector2 viewport)
    {
        var shader = assets.GetShader("picker");

        shader.Activate();
        shader.SetMatrix4x4("projection", camera.GetProjectionTransform((uint)viewport.X, (uint)viewport.Y));
        shader.SetMatrix4x4("view", camera.GetViewTransform());
        shader.SetMatrix4x4("mesh", Matrix4x4.Identity);

        descriptors.OfType<MeshDescriptor>().ToList().ForEach(x => Render(assets, shader, x));
    }

    private void Render(IAssetManager assets, IShader shader, MeshDescriptor descriptor)
    {
        if (!descriptor.DoRender)
            return;

        shader.SetMatrix4x4("entity", Matrix4x4.Identity);
        shader.SetMatrix4x4("mesh", descriptor.Transform);
        shader.SetMatrix4x4Array("finalBonesMatrices", descriptor.BoneTransforms);
        shader.SetUniform1("entityID", descriptor.PickerID);

        descriptor.Mesh.Draw();
    }
}
