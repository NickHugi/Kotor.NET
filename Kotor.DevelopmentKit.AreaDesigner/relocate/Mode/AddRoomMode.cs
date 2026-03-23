using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Model;
using Kotor.NET.Graphics.OpenGL;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Mode;

public class AddRoomMode : BaseMode
{
    public override string Name => "Add Room";

    private Room _addRoomRoom = new Room(null);
    private float angle = 0;

    public AddRoomMode(GLEngine engine, Area area) : base(engine, area)
    {
    }

    public override async Task RenderIntercept(OrbitCamera camera, Point mouse, List<MeshDescriptor> descriptors)
    {
        var ray = camera.ProjectRay((int)mouse.X, (int)mouse.Y, _engine.Width, _engine.Height);
        var point = ray.FindPointOnPlane(Axis.Z, 0);

        _addRoomRoom = new Room(null);
        _addRoomRoom.Position = point;
        _addRoomRoom.Orientation = Quaternion.CreateFromYawPitchRoll(0, 0, angle * (float)Math.PI / 180);

        (var newWall, var oldWall, var distance) = NearestAdjacentWall(_addRoomRoom);
        if (oldWall is not null)
        {
            newWall.Template = oldWall.Template;
            newWall.DoorFrame.Enabled = false;

            if (oldWall.DoorFrame is not null)
            {
                _addRoomRoom.Orientation = oldWall.Orientation / newWall.Orientation * Quaternion.CreateFromYawPitchRoll(0, 0, MathF.PI);
                _addRoomRoom.Position = oldWall.DoorFrame.Hooks.First().Position;
                _addRoomRoom.Position += newWall.Parent.Position - newWall.DoorFrame.Hooks.Last().Position;
            }
            else
            {
                _addRoomRoom.Position = new(-1000, 0, 0);
            }
        }

        var roomMeshDescriptors = new List<MeshDescriptor>();
        _areaEntity.RenderRoom(_engine.AssetManager, _addRoomRoom, ref roomMeshDescriptors);
        roomMeshDescriptors.ForEach(x => x.AmbientColor = new Vector3(1.5f, 1.5f, 1.5f));
        descriptors.AddRange(roomMeshDescriptors);
    }

    public override async Task Trigger()
    {
        _area.AddRoom(_addRoomRoom);
    }
}
