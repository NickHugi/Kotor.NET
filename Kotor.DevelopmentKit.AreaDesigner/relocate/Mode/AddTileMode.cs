using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Avalonia;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using Kotor.NET.Common.Data.Geometry;
using Kotor.NET.Extensions;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.OpenGL;
using Kotor.NET.Graphics.Renderers.Descriptors;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Mode;

public class AddTileMode : BaseMode
{
    public override string Name => "Add Room";

    private Room _addRoomRoom;
    private float angle = 0;

    public List<TileTemplate> TileTemplates
    {
        get
        {
            if (SelectedKit is null)
                return [];

            if (SelectedPiece is Wall wall)
            {
                var activeGroup = wall.Template.Group;
                return SelectedKit.Tiles.Where(x => x.Walls.Any(y => activeGroup == y.DefaultTemplate.Group)).ToList();
            }
            else
            {
                return SelectedKit?.Tiles.ToList() ?? [];
            }
        }
    }
    public TileTemplate? SelectedTileTemplate
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public AddTileMode(GLEngine engine, Area area, Kit kit, object selectedPiece) : base(engine, area, kit, selectedPiece)
    {
        this.WhenAnyValue(x => x.SelectedKit).Subscribe(_ =>
        {
            this.RaisePropertyChanged(nameof(TileTemplates));
        });
    }

    public override async Task RenderIntercept(OrbitCamera camera, Point mouse, List<IDrawCallDescriptor> descriptors)
    {
        if (SelectedTileTemplate is null)
            return;

        var ray = camera.ProjectRay((int)mouse.X, (int)mouse.Y, _engine.Width, _engine.Height);
        var point = ray.FindPointOnPlane(Axis.Z, 0);
        var render = true;

        _addRoomRoom = new Room(_area, SelectedTileTemplate);
        _addRoomRoom.Position = point;
        _addRoomRoom.Orientation = Quaternion.CreateFromYawPitchRoll(0, 0, angle * (float)Math.PI / 180);

        // TODO - build list of compatible magnets, use those as ways of snapping elements together
        var result = NearestAdjacentWall(_addRoomRoom, 1);
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

                RenderPredict(descriptors);
            }
        }
        else
        {
            RenderRoom(descriptors);
        }

        // Render Magnets
        var size = (0.5f) + MathF.Sin((_engine.RunningTime % 0.75f) / 0.75f * MathF.PI) * 0.25f;
        _area.Rooms.SelectMany(x => x.GetMagnets()).Concat(_addRoomRoom.GetMagnets()).ToList().ForEach(magnet =>
        {
            descriptors.Add(new BillboardDescriptor()
            {
                AllwaysOnTop = true,
                DoRender = true,
                FixedSize = false,
                Image = "magnet",
                Location = magnet.Position,
                Size = size
            });
        });
    }
    private void RenderRoom(List<IDrawCallDescriptor> descriptors)
    {
        var roomDescriptors = new List<IDrawCallDescriptor>();
        _areaEntity.RenderRoom(_engine.AssetManager, _addRoomRoom, ref roomDescriptors);
        roomDescriptors.OfType<MeshDescriptor>().ToList().ForEach(x => x.AmbientColor += new Vector3(0.5f, 0.5f, 0.5f));
        descriptors.AddRange(roomDescriptors);
    }
    private void RenderPredict(List<IDrawCallDescriptor> descriptors)
    {
        foreach (var existing in _area.Rooms.SelectMany(x => x.Walls))
        {
            foreach (var cursor in _addRoomRoom.Walls)
            {
                if (existing.Position.ApproximatelyEquals(cursor.Position, 0.01f))
                {
                    descriptors.RemoveAll(x => x.Tag == existing);
                    descriptors.RemoveAll(x => x.Tag == cursor);
                }
            }
        }

        foreach (var existing in _area.Rooms.SelectMany(x => x.InnerCorners))
        {
            foreach (var cursor in _addRoomRoom.InnerCorners)
            {
                if (existing.Position.ApproximatelyEquals(cursor.Position, 0.01f))
                {
                    descriptors.RemoveAll(x => x.Tag == existing);
                    descriptors.RemoveAll(x => x.Tag == cursor);
                }
            }
        }

        foreach (var existing in _area.Rooms.SelectMany(x => x.InnerCorners))
        {
            foreach (var cursor in _addRoomRoom.OuterCorners)
            {
                if (existing.Position.ApproximatelyEquals(cursor.Position, 0.01f))
                {
                    descriptors.RemoveAll(x => x.Tag == existing);
                    descriptors.RemoveAll(x => x.Tag == cursor);
                }
            }
        }
    }

    public override async Task Trigger()
    {
        if (SelectedTileTemplate is null)
            return;

        var result = NearestAdjacentWall(_addRoomRoom, 1);
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
