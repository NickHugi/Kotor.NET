using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Input;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using Kotor.DevelopmentKit.AreaDesigner.ViewModels;
using Kotor.DevelopmentKit.AreaDesigner.Views;
using Kotor.NET.Common.Data.Geometry;
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
            if (Kits is null)
                return [];

            return SelectedPiece switch
            {
                //TODO
                //Wall _ => SelectedKit.Objects.OfType<ObjectTemplate>().ToList(),
                //Floor _ => SelectedKit.Objects.OfType<ObjectTemplate>().ToList(),
                _ => _objects.ToList()
            };
        }
    }
    public ObjectTemplate? SelectedObjectTemplate
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    private bool _isTranslating;
    private Axis? _transformAxis;
    private IWorldObject? _projectedObject;
    private Point _mousePrevious;

    public SelectObjectMode(GLEngine engine, Area area, ObservableCollection<KitItem> kits, object selectedPiece, DesignerSettings settings) : base(engine, area, kits, selectedPiece, settings)
    {
        this.WhenAnyValue(x => x.SelectedPiece)
            .Subscribe(_ =>
            {
                this.RaisePropertyChanged(nameof(ObjectTemplates));
                this.RaisePropertyChanged(nameof(SelectedObjectTemplate));
            });

        this.WhenAnyValue(x => x.SelectedObjectTemplate)
            .Subscribe(_ =>
            {
                if (SelectedObjectTemplate is null)
                    return;
                if (SelectedPiece is not IWorldObject @object)
                    return;
                if (@object == _projectedObject)
                    return;

                @object.SwitchTemplate(SelectedObjectTemplate);
            });
    }

    public float ScreenDeltaToWorldMovement(Vector3 axis, Vector2 mouseDelta, OrbitCamera camera, int screenWidth, int screenHeight)
    {
        var p0 = Vector3.Zero;
        var p1 = p0 + axis;

        var screenP0 = camera.WorldToScreen(p0, screenWidth, screenHeight);
        var screenP1 = camera.WorldToScreen(p1, screenWidth, screenHeight);

        Vector2 screenAxis = screenP1 - screenP0;
        float screenAxisLength = screenAxis.Length();

        if (screenAxisLength < 1e-6f)
            return 0f;

        Vector2 dir = screenAxis / screenAxisLength;

        // Project mouse delta onto the screen-space axis direction
        float screenMovement = Vector2.Dot(mouseDelta, dir);

        // screenAxisLength = pixels per 1 world unit along this axis
        float worldMovement = screenMovement / screenAxisLength;

        return worldMovement;
    }

    public override Task RenderIntercept(OrbitCamera camera, Point mouse, List<IDrawCallDescriptor> descriptors) // Viewport viewport
    {
        var mouseDelta = mouse - _mousePrevious;
        _mousePrevious = mouse;

        _projectedObject = IntersectingObject(camera, mouse.X, mouse.Y)?.Result;
        if (_projectedObject is not null)
            descriptors.Where(x => x.Tag == _projectedObject).OfType<MeshDescriptor>().ToList().ForEach(x => x.AmbientColor = new(1.5f, 1.5f, 1.5f));

        if (_isTranslating && SelectedPiece is WorldObject obj)
        {
            var ray = camera.ProjectRay((int)mouse.X, (int)mouse.Y, 1109, 703);

            if (_transformAxis.HasValue)
            {
                obj.LocalPosition = ray.SolveLine(_transformAxis.Value, obj.LocalPosition);
            }
            else
            {
                var point = ray.FindPointOnPlane(Axis.Z, 0);
                obj.LocalPosition = new(point.X, point.Y, obj.LocalPosition.Z);
            }
        }
        if (SelectedPiece is WorldObject @object && _isTranslating)
        {
            Vector3 start = _transformAxis switch
            {
                Axis.X => new(500, 0, 0),
                Axis.Y => new(0, 500, 0),
                Axis.Z => new(0, 0, 500),
                _ => new()
            };
            Vector4 color = _transformAxis switch
            {
                Axis.X => new(1, 0, 0, 1),
                Axis.Y => new(0, 1, 0, 1),
                Axis.Z => new(0, 0, 1, 1),
                _ => new()
            };

            descriptors.Add(new LineDescriptor()
            {
                Color = color,
                Start = @object.GlobalPosition + start,
                End = @object.GlobalPosition - start,
                Thickness = 0.5f
            });
        }

        return Task.CompletedTask;
    }

    public override async Task Trigger()
    {
        if (_projectedObject is null)
            return;

        await ClearSelection.Handle(Unit.Default);
        await AddToSelection.Handle(_projectedObject);
        //SelectedObjectTemplate = _projectedObject.Template;
    }

    public override Task KeyPress(Inputs inputs, int key)
    {
        if (key == 50) // G
        {
            _isTranslating = true;
            _transformAxis = null;
        }
        if (key == 67) // X
        {
            if (_isTranslating)
                _transformAxis = Axis.X;
        }
        if (key == 68) // Y
        {
            if (_isTranslating)
                _transformAxis = Axis.Y;
        }
        if (key == 69) // Z
        {
            if (_isTranslating)
                _transformAxis = Axis.Z;
        }

        return Task.CompletedTask;
    }
}
