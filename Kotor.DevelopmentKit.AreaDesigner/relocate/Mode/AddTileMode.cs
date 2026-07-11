using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Avalonia;
using DynamicData;
using DynamicData.Binding;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.ViewModels;
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

    private Room _projectedRoom = default!;
    private WorldObject _projectedTile => _projectedRoom.Objects.Single();
    private float angle = 0;

    public List<WorldObjectTemplate> TileTemplates
    {
        get
        {
            if (Kits is null)
                return [];

            if (SelectedWorldObject?.Type == WorldObjectType.Wall)
            {
                var activeGroup = SelectedWorldObject.Template.ClassID;
                return _objects.Where(x => x.Type == WorldObjectType.Tile).Where(x => x.Magnets.OfType<MagnetTemplate>().Any(y => activeGroup == y.Template.ClassID)).ToList();
            }
            else
            {
                return _objects.Where(x => x.Type == WorldObjectType.Tile).ToList() ?? [];
            }
        }
    }
    public WorldObjectTemplate? SelectedTileTemplate
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public AddTileMode(GLEngine engine, Area area, ObservableCollection<KitItem> kits, WorldObject activeWorldObject, DesignerSettings settings) : base(engine, area, kits, activeWorldObject, settings)
    {
        Kits.ToObservableChangeSet().AutoRefresh(x => x.Active).Subscribe(_ => this.RaisePropertyChanged(nameof(TileTemplates)));
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

        var result = NearbyMagnets(_projectedRoom.AllMagnets, 1)
            .Where(x => x.Source.IsTileMagnet && x.Target.IsTileMagnet)
            .FirstOrDefault();

        if (result is not null)
        {
            // Target is existing wall
            // Source is cursor

            _projectedRoom.Orientation = result.Target.GlobalOrientation / result.Source.GlobalOrientation * Quaternion.CreateFromYawPitchRoll(0, 0, MathF.PI);
            _projectedRoom.Position = new();
            _projectedRoom.Position = result.Target.GlobalPosition - result.Source.GlobalPosition;

            //var sourceWall = result.Source.Parent;
            var sourceWall = _projectedRoom.AllObjects.FirstOrDefault(x => x.Template == result.Source.Template.Template && x.GlobalPosition == result.Source.GlobalPosition);

            if (result.Target.Parent.Type == WorldObjectType.DoorFrame)
            {
                sourceWall.SwitchTemplate(result.Target.Parent.ParentMagnet.Parent.Template);
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
        _area.Rooms.SelectMany(x => x.AllMagnets)
            .Concat(_projectedRoom.AllMagnets)
            .Where(x => x.IsTileMagnet)
            .ToList()
            .ForEach(magnet =>
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
        var tiles = _area.Rooms.SelectMany(x => x.Objects.Where(x => x.Type == WorldObjectType.Tile)).ToList();

        foreach (var existing in tiles.SelectMany(x => x.AttachedObjects.Where(x => x.Type == WorldObjectType.Wall)))
        {
            foreach (var cursor in _projectedTile.AttachedObjects.Where(x => x.Type == WorldObjectType.Wall))
            {
                if (existing.GlobalPosition.ApproximatelyEquals(cursor.GlobalPosition, 0.01f))
                {
                    descriptors.RemoveAll(x => x.Tag == existing);
                    descriptors.RemoveAll(x => x.Tag == cursor);
                }
            }
        }

        foreach (var existing in tiles.SelectMany(x => x.AttachedObjects.Where(x => x.Type == WorldObjectType.InnerCorner)))
        {
            foreach (var cursor in _projectedTile.AttachedObjects.Where(x => x.Type == WorldObjectType.InnerCorner))
            {
                if (existing.GlobalPosition.ApproximatelyEquals(cursor.GlobalPosition, 0.01f))
                {
                    descriptors.RemoveAll(x => x.Tag == existing);
                    descriptors.RemoveAll(x => x.Tag == cursor);
                }
            }
        }

        foreach (var existing in tiles.SelectMany(x => x.AttachedObjects.Where(x => x.Type == WorldObjectType.OuterCorner)))
        {
            foreach (var cursor in _projectedTile.AttachedObjects.Where(x => x.Type == WorldObjectType.OuterCorner))
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

        var magnet = NearbyMagnets(_projectedRoom.AllMagnets, 1)
            .Where(x => x.Source.IsTileMagnet && x.Target.IsTileMagnet)
            .FirstOrDefault();

        if (magnet is not null && magnet.Target.IsHook && magnet.Target.Template?.Template?.Type == WorldObjectType.Wall)
        {
            var template = _projectedRoom.Objects.Where(x => x.Type == WorldObjectType.Tile).First().Template;
            var room = magnet.Target.Parent.Room;
            var newTile = new WorldObject(room, null, template, Guid.NewGuid(), WorldObjectType.Tile);
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
