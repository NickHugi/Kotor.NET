using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.Views;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Model;
using Kotor.NET.Graphics.OpenGL;
using Kotor.NET.Graphics.Renderers.Descriptors;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Mode;

public class SelectRoomMode : BaseMode
{
    public override string Name => "Select Room";

    public required Interaction<object, Unit> AddToSelection { get; init; }
    public required Interaction<Unit, Unit> ClearSelection { get; init; }

    private Room? _roomAtCursor;

    public SelectRoomMode(GLEngine engine, Area area, Kit kit, object selectedPiece) : base(engine, area, kit, selectedPiece)
    {
    }

    public override Task RenderIntercept(OrbitCamera camera, Point mouse, List<IDrawCallDescriptor> descriptors)
    {
        _roomAtCursor = IntersectingFloor(camera, mouse.X, mouse.Y)?.Result.Parent.Parent;

        if (_roomAtCursor is not null)
        {
            foreach (var tile in _roomAtCursor.Objects.OfType<Tile>())
            {
                descriptors
                    .Where(x => x.Tag == tile.Floor || x.Tag == tile.Ceiling || tile.Walls.Contains(x.Tag))
                    .OfType<MeshDescriptor>()
                    .ToList()
                    .ForEach(x => x.AmbientColor = new(1.5f, 1.5f, 1.5f));
            }
        }

        return Task.CompletedTask;
    }

    public override async Task Trigger()
    {
        if (_roomAtCursor is null)
            return;

        await ClearSelection.Handle(Unit.Default);
        await AddToSelection.Handle(_roomAtCursor);
    }
}
