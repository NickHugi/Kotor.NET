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
using Kotor.DevelopmentKit.AreaDesigner.Views;
using Kotor.NET.Common.Data.Geometry;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Model;
using Kotor.NET.Graphics.OpenGL;
using Kotor.NET.Graphics.Renderers.Descriptors;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Mode;

public class AddObjectMode : BaseMode
{
    public List<WorldObjectTemplate> ObjectTemplates
    {
        get
        {
            return _objects.Where(x => x.GetType() == typeof(WorldObjectTemplate)).ToList();
        }
    }
    public WorldObjectTemplate? SelectedObjectTemplate
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    private WorldObject _addObject = default!;
    private float angle = 0;

    public AddObjectMode(GLEngine engine, Area area, ObservableCollection<KitItem> kits, WorldObject activeWorldObject, DesignerSettings settings) : base(engine, area, kits, activeWorldObject, settings)
    {
        Kits.ToObservableChangeSet().AutoRefresh(x => x.Active).Subscribe(_ => this.RaisePropertyChanged(nameof(ObjectTemplates)));
    }

    public override void Update(float delta, AreaScene scene)
    {
        base.Update(delta, scene);

        if (SelectedObjectTemplate is not null)
        {
            var ray = scene.ActiveCamera.ProjectRay((int)scene.Mouse.X, (int)scene.Mouse.Y, _engine.Width, _engine.Height);
            var point = ray.FindPointOnPlane(Axis.Z, 0);

            _addObject = new(_area.Rooms.Last(), null, SelectedObjectTemplate, Guid.NewGuid(), WorldObjectType.Generic);
            _addObject.LocalPosition = point;
            _addObject.LocalOrientation = Quaternion.CreateFromYawPitchRoll(0, 0, angle * (float)Math.PI / 180);

            scene.Projection.Clear();
            scene.Projection.Add(_addObject);
        }
    }

    public override void Render(float delta, AreaScene scene, ref ICollection<IDrawCallDescriptor> descriptors)
    {
        base.Render(delta, scene, ref descriptors);
    }

    public override void MousePress(Inputs inputs)  
    {
        if (inputs.AreMouseButtonsDown([0]) && inputs.AreKeysDown([]))
        {
            PlaceObject();
        }
    }

    public void PlaceObject()
    {
        if (SelectedObjectTemplate is null)
            return;

        // TODO - need intelligent way of picking room
        var room = _area.Rooms.First();
        room.AddObject(_addObject);
    }
}
