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
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Mode;

public class SelectWallMode : BaseMode
{
    public override string Name => "Select Wall";

    public required Interaction<object, Unit> AddToSelection { get; init; }
    public required Interaction<Unit, Unit> ClearSelection { get; init; }

    public List<WallTemplate> WallTemplates
    {
        get
        {
            if (SelectedKit is null)
                return [];

            if (SelectedPiece is Wall wall)
            {
                var activeGroup = wall.Template.Group;
                return SelectedKit.Walls.Where(x => x.Group == activeGroup).ToList();
            }
            else
            {
                return SelectedKit?.Walls.ToList() ?? [];
            }
        }
    }
    public WallTemplate? SelectedWallTemplate
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    private Wall? _wall;

    public SelectWallMode(GLEngine engine, Area area, Kit kit, object selectedPiece) : base(engine, area, kit, selectedPiece)
    {
        this.WhenAnyValue(x => x.SelectedPiece)
            .Subscribe(_ => this.RaisePropertyChanged(nameof(WallTemplates)));

        this.WhenAnyValue(x => x.SelectedWallTemplate)
            .Subscribe(_ =>
            {
                if (SelectedWallTemplate is null)
                    return;
                if (SelectedPiece is not Wall wall)
                    return;
                if (wall == _wall)
                    return;

                wall.SwitchTemplate(SelectedWallTemplate);
            });
    }

    public override Task RenderIntercept(OrbitCamera camera, Point mouse, List<MeshDescriptor> descriptors)
    {
        _wall = NearestWallMagnest(camera, mouse.X, mouse.Y)?.Result;

        if (_wall is not null)
            descriptors.Where(x => x.Tag == _wall).ToList().ForEach(x => x.AmbientColor = new(1.5f, 1.5f, 1.5f));

        return Task.CompletedTask;
    }

    public override async Task Trigger()
    {
        if (_wall is null)
            return;

        await ClearSelection.Handle(Unit.Default);
        await AddToSelection.Handle(_wall);
        SelectedWallTemplate = _wall.Template;
    }
}
