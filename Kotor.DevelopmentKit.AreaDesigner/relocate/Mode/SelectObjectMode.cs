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

public class SelectObjectMode : BaseMode
{
    public override string Name => "Select Object";

    public required Interaction<object, Unit> AddToSelection { get; init; }
    public required Interaction<Unit, Unit> ClearSelection { get; init; }

    public List<ObjectTemplate> ObjectTemplates
    {
        get
        {
            return SelectedKit?.Objects.ToList() ?? [];
        }
    }
    public ObjectTemplate? SelectedObjectTemplate
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    private Object? _projectedObject;

    public SelectObjectMode(GLEngine engine, Area area, Kit kit, object selectedPiece) : base(engine, area, kit, selectedPiece)
    {
        this.WhenAnyValue(x => x.SelectedObjectTemplate)
            .Subscribe(_ =>
            {
                if (SelectedObjectTemplate is null)
                    return;
                if (SelectedPiece is not Object @object)
                    return;
                if (@object == _projectedObject)
                    return;

                @object.SwitchTemplate(SelectedObjectTemplate);
            });
    }

    public override Task RenderIntercept(OrbitCamera camera, Point mouse, List<IDrawCallDescriptor> descriptors)
    {
        _projectedObject = IntersectingObject(camera, mouse.X, mouse.Y)?.Result;

        if (_projectedObject is not null)
            descriptors.Where(x => x.Tag == _projectedObject).OfType<MeshDescriptor>().ToList().ForEach(x => x.AmbientColor = new(1.5f, 1.5f, 1.5f));

        return Task.CompletedTask;
    }

    public override async Task Trigger()
    {
        if (_projectedObject is null)
            return;

        await ClearSelection.Handle(Unit.Default);
        await AddToSelection.Handle(_projectedObject);
        SelectedObjectTemplate = _projectedObject.Template;
    }
}
