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
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Mode;

public class AddTileMode : BaseMode
{
    public override string Name => "Add Room";

    private Room _addRoomRoom;
    private float angle = 0;

    public List<TileTemplate> TileTemplates => SelectedKit?.Tiles.ToList() ?? [];
    public TileTemplate? SelectedTileTemplate
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public AddTileMode(GLEngine engine, Area area, Kit kit) : base(engine, area, kit)
    {
        this.WhenAnyValue(x => x.SelectedKit).Subscribe(_ =>
        {
            this.RaisePropertyChanged(nameof(TileTemplates));
        });
    }

    public override async Task RenderIntercept(OrbitCamera camera, Point mouse, List<MeshDescriptor> descriptors)
    {
        if (SelectedTileTemplate is null)
            return;

        var ray = camera.ProjectRay((int)mouse.X, (int)mouse.Y, _engine.Width, _engine.Height);
        var point = ray.FindPointOnPlane(Axis.Z, 0);
        var render = true;

        _addRoomRoom = new Room(SelectedTileTemplate);
        _addRoomRoom.Position = point;
        _addRoomRoom.Orientation = Quaternion.CreateFromYawPitchRoll(0, 0, angle * (float)Math.PI / 180);

        var result = NearestAdjacentWall(_addRoomRoom);
        if (result is not null)
        {
            // Target is existing wall
            // Source is cursor

            if (result.Target.DoorFrame is not null)
            {
                result.Source.SwitchTemplate(result.Target.Template);
                result.Source.DoorFrame.Enabled = false;

                _addRoomRoom.Orientation = result.Target.Orientation / result.Source.Orientation * Quaternion.CreateFromYawPitchRoll(0, 0, MathF.PI);
                _addRoomRoom.Position = result.Target.DoorFrame.Hooks.First().Position;
                _addRoomRoom.Position += result.Source.Parent.Position - result.Source.DoorFrame.Hooks.Last().Position;

                RenderRoom(descriptors);
            }
            else
            {
                _addRoomRoom.Orientation = result.Target.Orientation / result.Source.Orientation * Quaternion.CreateFromYawPitchRoll(0, 0, MathF.PI);
                _addRoomRoom.Position = result.Target.Position;
                _addRoomRoom.Position += result.Source.Parent.Position - result.Source.Position;

                RenderRoom(descriptors);

                // TODO: More accurately predict/visualize placement result
                descriptors.RemoveAll(x => x.Tag == result.Target);
                descriptors.RemoveAll(x => x.Tag == result.Source);
            }
        }
        else
        {
            RenderRoom(descriptors);
        }
    }
    private void RenderRoom(List<MeshDescriptor> descriptors)
    {
        var roomMeshDescriptors = new List<MeshDescriptor>();
        _areaEntity.RenderRoom(_engine.AssetManager, _addRoomRoom, ref roomMeshDescriptors);
        roomMeshDescriptors.ForEach(x => x.AmbientColor += new Vector3(0.5f, 0.5f, 0.5f));
        descriptors.AddRange(roomMeshDescriptors);
    }

    public override async Task Trigger()
    {
        if (SelectedTileTemplate is null)
            return;

        var result = NearestAdjacentWall(_addRoomRoom);
        if (result is not null && result.Target.DoorFrame is null)
        {
            result.Target.Extend(_addRoomRoom.Tiles.First().Template);
        }
        else
        {
            _area.AddRoom(_addRoomRoom);
        }
    }
}
