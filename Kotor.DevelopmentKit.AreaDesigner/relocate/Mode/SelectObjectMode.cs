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
    public required Interaction<WorldObject, Unit> AddToSelection { get; init; }
    public required Interaction<Unit, Unit> ClearSelection { get; init; }

    public IEnumerable<WorldObjectTemplate> ObjectTemplates
    {
        get
        {
            return SelectedWorldObject switch
            {
                _ => _objects.Where(x => SelectedWorldObject is null || x.Type == SelectedWorldObject.Type).ToList()
            };
        }
    }
    public WorldObjectTemplate? SelectedObjectTemplate
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    private bool _isTranslating;
    private Axis? _transformAxis;
    private WorldObject? _projectedObject;

    public SelectObjectMode(GLEngine engine, Area area, ObservableCollection<KitItem> kits, WorldObject activeWorldObject, DesignerSettings settings) : base(engine, area, kits, activeWorldObject, settings)
    {
        this.WhenAnyValue(x => x.SelectedWorldObject)
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
                if (SelectedWorldObject is not WorldObject @object)
                    return;
                if (@object == _projectedObject)
                    return;

                @object.SwitchTemplate(SelectedObjectTemplate);
            });
    }

    public override void Update(float delta, AreaScene scene)
    {
        base.Update(delta, scene);

        _projectedObject = RaycastWorldObject(scene.ActiveCamera as OrbitCamera, scene.Mouse.X, scene.Mouse.Y).FirstOrDefault()?.Result;

        if (_isTranslating && SelectedWorldObject is not null)
        {
            TranslateSelect(scene);
        }
    }

    public override void Render(float delta, AreaScene scene, ref ICollection<IDrawCallDescriptor> descriptors)
    {
        base.Render(delta, scene, ref descriptors);

        if (_projectedObject is not null)
        {
            descriptors.Where(x => x.Tag == _projectedObject).OfType<MeshDescriptor>().ToList().ForEach(x => x.AmbientColor += new Vector3(0.3f));
        }
        if (SelectedWorldObject is not null)
        {
            descriptors.Where(x => x.Tag == SelectedWorldObject).OfType<MeshDescriptor>().ToList().ForEach(x => x.AmbientColor += new Vector3(0.3f));
        }
        if (_isTranslating && SelectedWorldObject is not null)
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
                Start = SelectedWorldObject.GlobalPosition + start,
                End = SelectedWorldObject.GlobalPosition - start,
                Thickness = 0.5f
            });
        }
    }

    public override void MousePress(Inputs inputs)
    {
        if (inputs.AreMouseButtonsDown(0) && inputs.AreKeysDown())
        {
            SelectAtMouse();
        }
    }

    public override void KeyPress(Inputs inputs, int key)
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
    }

    public void SelectAtMouse()
    {
        ClearSelection.Handle(Unit.Default).Wait();

        if (_projectedObject is not null)
        {
            AddToSelection.Handle(_projectedObject).Wait();
        }
    }

    public void TranslateSelect(AreaScene scene)
    {
        var ray = scene.ActiveCamera.ProjectRay((int)scene.Mouse.X, (int)scene.Mouse.Y, 1109, 703);

        if (_transformAxis.HasValue)
        {
            SelectedWorldObject.LocalPosition = ray.SolveLine(_transformAxis.Value, SelectedWorldObject.LocalPosition);
        }
        else
        {
            var point = ray.FindPointOnPlane(Axis.Z, 0);
            SelectedWorldObject.LocalPosition = new(point.X, point.Y, SelectedWorldObject.LocalPosition.Z);
        }
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
}
