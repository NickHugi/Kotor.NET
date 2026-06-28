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
using Kotor.NET.Graphics.Extensions;
using Kotor.NET.Graphics.OpenGL;
using Kotor.NET.Graphics.Renderers.Descriptors;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Mode;

public class AddTileMode : BaseMode
{
    public override string Name => "Add Room";

    private Room _projectedRoom;
    private Tile _projectedTile => _projectedRoom.Objects.OfType<Tile>().Single();
    private float angle = 0;

    public List<TileTemplate> TileTemplates
    {
        get
        {
            if (SelectedKit is null)
                return [];

            if (SelectedPiece is Wall wall)
            {
                var activeGroup = wall.Template.ClassID;
                return SelectedKit.Tiles.Where(x => x.Hooks.OfType<WallHookTemplate>().Any(y => activeGroup == y.Template.ClassID)).ToList();
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

    public AddTileMode(GLEngine engine, Area area, Kit kit, object selectedPiece, DesignerSettings settings) : base(engine, area, kit, selectedPiece, settings)
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

        if (_settings.PositionSnapEnabled)
            point = point.Snap(Axis.X, _settings.PositionSnapSize).Snap(Axis.Y, _settings.PositionSnapSize);

        _projectedRoom = new Room(_area, SelectedTileTemplate);
        _projectedRoom.Position = point;
        _projectedRoom.Orientation = Quaternion.CreateFromYawPitchRoll(0, 0, angle * (float)Math.PI / 180);

        // TODO - build list of compatible magnets, use those as ways of snapping elements together
        var result = NearestMagnet(_projectedRoom.GetMagnets(), 1);
        if (result is not null)
        {
            // Target is existing wall
            // Source is cursor

            _projectedRoom.Orientation = result.Target.GlobalOrientation / result.Source.GlobalOrientation * Quaternion.CreateFromYawPitchRoll(0, 0, MathF.PI);
            _projectedRoom.Position = new();
            _projectedRoom.Position = result.Target.GlobalPosition - result.Source.GlobalPosition;

            var sourceWall = result.Source.Parent as Wall;

            if (result.Target.Parent is DoorFrame targetDoorframe)
            {
                sourceWall.SwitchTemplate(targetDoorframe.Parent.Template);
                sourceWall.DoorFrame.Enabled = false;

                RenderRoom(descriptors);
            }
            else
            {
                RenderRoom(descriptors);
                RenderPredict(descriptors);
            }
        }
        else
        {
            RenderRoom(descriptors);
        }

        // Render Magnets
        var size = (0.4f) + MathF.Sin((_engine.RunningTime % 0.75f) / 0.75f * MathF.PI) * 0.2f;
        _area.Rooms.SelectMany(x => x.GetMagnets()).Concat(_projectedRoom.GetMagnets()).ToList().ForEach(magnet =>
        {
            descriptors.Add(new BillboardDescriptor()
            {
                AllwaysOnTop = true,
                DoRender = true,
                FixedSize = false,
                Image = "magnet",
                Location = magnet.GlobalPosition,
                Size = size
            });
        });
    }
    private void RenderRoom(List<IDrawCallDescriptor> descriptors)
    {
        var roomDescriptors = new List<IDrawCallDescriptor>();
        _areaEntity.RenderRoom(_engine.AssetManager, _projectedRoom, ref roomDescriptors);
        roomDescriptors.OfType<MeshDescriptor>().ToList().ForEach(x => x.AmbientColor += new Vector3(0.5f, 0.5f, 0.5f));
        descriptors.AddRange(roomDescriptors);
    }
    private void RenderPredict(List<IDrawCallDescriptor> descriptors)
    {
        var tiles = _area.Rooms.SelectMany(x => x.Objects.OfType<Tile>()).ToList();

        foreach (var existing in tiles.SelectMany(x => x.VirtualObjects.OfType<Wall>()))
        {
            foreach (var cursor in _projectedTile.VirtualObjects.OfType<Wall>())
            {
                if (existing.GlobalPosition.ApproximatelyEquals(cursor.GlobalPosition, 0.01f))
                {
                    descriptors.RemoveAll(x => x.Tag == existing);
                    descriptors.RemoveAll(x => x.Tag == cursor);
                }
            }
        }

        foreach (var existing in tiles.SelectMany(x => x.VirtualObjects.OfType<InnerCorner>()))
        {
            foreach (var cursor in _projectedTile.VirtualObjects.OfType<InnerCorner>())
            {
                if (existing.GlobalPosition.ApproximatelyEquals(cursor.GlobalPosition, 0.01f))
                {
                    descriptors.RemoveAll(x => x.Tag == existing);
                    descriptors.RemoveAll(x => x.Tag == cursor);
                }
            }
        }

        foreach (var existing in tiles.SelectMany(x => x.VirtualObjects.OfType<OuterCorner>()))
        {
            foreach (var cursor in _projectedTile.VirtualObjects.OfType<OuterCorner>())
            {
                if (existing.GlobalPosition.ApproximatelyEquals(cursor.GlobalPosition, 0.01f))
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

        var magnet = NearestMagnet(_projectedRoom.GetMagnets(), 1);
        if (magnet is not null && magnet.Target.Parent is Wall wall)
        {
            var template = _projectedRoom.Objects.OfType<Tile>().First().Template;
            var room = wall.Parent.Parent;
            var newTile = new Tile(room);
            newTile.SwitchTemplate(template);
            newTile.GlobalPosition = _projectedRoom.Position;
            newTile.GlobalOrientation = _projectedRoom.Orientation;
            room.AddTile(newTile);
        }
        else
        {
            _area.AddRoom(_projectedRoom);
        }
    }
}
