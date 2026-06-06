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

public class SelectCeilingMode : BaseMode
{
    public override string Name => "Select Ceiling";

    public required Interaction<object, Unit> AddToSelection { get; init; }
    public required Interaction<Unit, Unit> ClearSelection { get; init; }

    public List<CeilingTemplate> CeilingTemplates
    {
        get
        {
            if (SelectedPiece is not Ceiling ceiling)
                return [];

            return SelectedKit?.Ceilings.Where(x => x.Group == ceiling.Template.Group).ToList() ?? [];
        }
    }
    public CeilingTemplate? SelectedCeilingTemplate
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    private Ceiling? _ceilingAtCursor;

    public SelectCeilingMode(GLEngine engine, Area area, Kit kit, object selectedPiece) : base(engine, area, kit, selectedPiece)
    {
        this.WhenAnyValue(x => x.SelectedPiece)
            .Subscribe(_ => this.RaisePropertyChanged(nameof(CeilingTemplate)));

        this.WhenAnyValue(x => x.SelectedCeilingTemplate)
            .Subscribe(_ =>
            {
                if (SelectedCeilingTemplate is null)
                    return;
                if (SelectedPiece is not Ceiling @object)
                    return;
                if (@object == _ceilingAtCursor)
                    return;

                @object.SwitchTemplate(SelectedCeilingTemplate);
            });
    }

    public override Task RenderIntercept(OrbitCamera camera, Point mouse, List<IDrawCallDescriptor> descriptors)
    {
        _ceilingAtCursor = IntersectingCeiling(camera, mouse.X, mouse.Y)?.Result;

        if (_ceilingAtCursor is not null)
            descriptors.Where(x => x.Tag == _ceilingAtCursor).OfType<MeshDescriptor>().ToList().ForEach(x => x.AmbientColor = new(1.5f, 1.5f, 1.5f));

        return Task.CompletedTask;
    }

    public override async Task Trigger()
    {
        if (_ceilingAtCursor is null)
            return;

        await ClearSelection.Handle(Unit.Default);
        await AddToSelection.Handle(_ceilingAtCursor);
        SelectedCeilingTemplate = _ceilingAtCursor.Template;
    }
}
