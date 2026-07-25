using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Input;
using DynamicData;
using DynamicData.Binding;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.ViewModels;
using Kotor.DevelopmentKit.AreaDesigner.Views;
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
                return _objects.Where(x => x.Type == WorldObjectType.Tile).Where(x => x.Magnets.OfType<MagnetTemplate>().Any(y => activeGroup == y.Template?.ClassID)).ToList();
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

    public override void Update(float delta, AreaScene scene)
    {
        if (SelectedTileTemplate is null)
            return;

        var ray = scene.ActiveCamera.ProjectRay((int)scene.Mouse.X, (int)scene.Mouse.Y, _engine.Width, _engine.Height);
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

            var sourceWall = _projectedRoom.AllObjects.FirstOrDefault(x => x.Template == result.Source.MagnetTemplate.Template && x.GlobalPosition == result.Source.GlobalPosition);

            if (result.Target.Parent.Type == WorldObjectType.DoorFrame)
            {
                sourceWall.SwitchTemplate(result.Target.Parent.ParentMagnet.Parent.Template);
                _projectedRoom.Position = new();
                _projectedRoom.Position = result.Target.GlobalPosition - sourceWall.Magnets.First(x => x.WorldObjectTemplate?.Type == WorldObjectType.DoorFrame).GlobalPosition;
            }
        }

        scene.Projection.Clear();
        scene.Projection.Add(_projectedTile);
    }

    public override void MousePress(Inputs inputs)
    {
        if (inputs.AreMouseButtonsDown(0) && inputs.AreKeysDown())
        {
            PlaceTile();
        }
    }

    public override void MouseMove(Inputs inputs, AreaScene scene)
    {
        base.MouseMove(inputs, scene);
    }

    public override void MouseScroll(Inputs inputs, AreaScene scene, Vector2 scroll)
    {
        base.MouseScroll(inputs, scene, scroll);
    }

    public void PlaceTile()
    {
        if (SelectedTileTemplate is null)
            return;

        var magnet = NearbyMagnets(_projectedRoom.AllMagnets, 1)
            .Where(x => x.Source.IsTileMagnet && x.Target.IsTileMagnet)
            .FirstOrDefault();

        if (magnet is not null && magnet.Target.IsHook && magnet.Target.MagnetTemplate?.Template?.Type == WorldObjectType.Wall)
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

