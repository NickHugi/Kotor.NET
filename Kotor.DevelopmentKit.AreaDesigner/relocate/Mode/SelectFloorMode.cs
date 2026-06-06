using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Kotor.DevelopmentKit.AreaDesigner.Views;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Model;
using Kotor.NET.Graphics.OpenGL;
using Kotor.NET.Graphics.Renderers.Descriptors;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Mode;

public class SelectFloorMode : BaseMode
{
    public override string Name => "Select Floor";

    public required Interaction<object, Unit> AddToSelection { get; init; }
    public required Interaction<Unit, Unit> ClearSelection { get; init; }

    public List<FloorTemplate> FloorTemplates
    {
        get
        {
            if (SelectedPiece is not Floor floor)
                return [];

            return SelectedKit?.Floors.Where(x => x.Group == floor.Template.Group).ToList() ?? [];
        }
    }
    public FloorTemplate? SelectedFloorTemplate
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    private Floor? _floorAtCursor;

    public SelectFloorMode(GLEngine engine, Area area, Kit kit, object selectedPiece) : base(engine, area, kit, selectedPiece)
    {
        this.WhenAnyValue(x => x.SelectedPiece)
            .Subscribe(_ => this.RaisePropertyChanged(nameof(FloorTemplates)));

        this.WhenAnyValue(x => x.SelectedFloorTemplate)
            .Subscribe(_ =>
            {
                if (SelectedFloorTemplate is null)
                    return;
                if (SelectedPiece is not Floor @object)
                    return;
                if (@object == _floorAtCursor)
                    return;

                @object.SwitchTemplate(SelectedFloorTemplate);
            });
    }

    public override Task RenderIntercept(OrbitCamera camera, Point mouse, List<IDrawCallDescriptor> descriptors)
    {
        _floorAtCursor = IntersectingFloor(camera, mouse.X, mouse.Y)?.Result;

        if (_floorAtCursor is not null)
            descriptors.Where(x => x.Tag == _floorAtCursor).OfType<MeshDescriptor>().ToList().ForEach(x => x.AmbientColor = new(1.5f, 1.5f, 1.5f));

        return Task.CompletedTask;
    }

    public override async Task Trigger()
    {
        if (_floorAtCursor is null)
            return;

        await ClearSelection.Handle(Unit.Default);
        await AddToSelection.Handle(_floorAtCursor);
        SelectedFloorTemplate = _floorAtCursor.Template;
    }
}
