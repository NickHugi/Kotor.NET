using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Avalonia;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Model;
using Kotor.NET.Graphics.OpenGL;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Mode;

public class AddObjectMode : BaseMode
{
    public override string Name => "Add Object";

    private Object _addObject = null;
    private float angle = 0;

    public AddObjectMode(GLEngine engine, Area area) : base(engine, area)
    {
    }

    public override async Task RenderIntercept(OrbitCamera camera, Point mouse, List<MeshDescriptor> descriptors)
    {
        var ray = camera.ProjectRay((int)mouse.X, (int)mouse.Y, _engine.Width, _engine.Height);
        var point = ray.FindPointOnPlane(Axis.Z, 0);

        // todo - should be placed in room where walkmesh intersects.
        // todo - should not be hardcoded
        _addObject = new(_area.Rooms.Last(), Kit.Manager.Get("sandral").Object("sandral_object_0")); 
        _addObject.LocalPosition = point;
        _addObject.LocalOrientation = Quaternion.CreateFromYawPitchRoll(0, 0, angle * (float)Math.PI / 180);

        var roomMeshDescriptors = new List<MeshDescriptor>();
        _areaEntity.RenderObject(_engine.AssetManager, _addObject, ref roomMeshDescriptors);
        roomMeshDescriptors.ForEach(x => x.AmbientColor = new Vector3(1.5f, 1.5f, 1.5f));
        descriptors.AddRange(roomMeshDescriptors);
    }

    public override async Task Trigger()
    {
        // todo - add to room within bounds of the cursor
        var room = _area.Rooms.First();
        room.AddObject(_addObject);
    }
}
