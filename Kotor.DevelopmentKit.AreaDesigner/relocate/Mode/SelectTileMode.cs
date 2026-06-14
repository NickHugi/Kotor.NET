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

public class SelectTileMode : BaseMode
{
    public override string Name => "Select Tile";

    public required Interaction<object, Unit> AddToSelection { get; init; }
    public required Interaction<Unit, Unit> ClearSelection { get; init; }

    private Tile? _tileAtCursor;

    public SelectTileMode(GLEngine engine, Area area, Kit kit, object selectedPiece) : base(engine, area, kit, selectedPiece)
    {
    }

    public override Task RenderIntercept(OrbitCamera camera, Point mouse, List<IDrawCallDescriptor> descriptors)
    {
        _tileAtCursor = IntersectingFloor(camera, mouse.X, mouse.Y)?.Result.Parent;

        if (_tileAtCursor is not null)
        {
            descriptors
                .Where(x => x.Tag == _tileAtCursor.Floor || x.Tag == _tileAtCursor.Ceiling || _tileAtCursor.Walls.Contains(x.Tag))
                .OfType<MeshDescriptor>()
                .ToList()
                .ForEach(x => x.AmbientColor = new(1.5f, 1.5f, 1.5f));
        }

        return Task.CompletedTask;
    }

    public override async Task Trigger()
    {
        if (_tileAtCursor is null)
            return;

        await ClearSelection.Handle(Unit.Default);
        await AddToSelection.Handle(_tileAtCursor);
    }
}
