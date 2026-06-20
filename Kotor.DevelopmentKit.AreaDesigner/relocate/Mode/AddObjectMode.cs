using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Avalonia;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
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
    public override string Name => "Add Object";

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

    private WorldObject _addObject = null;
    private float angle = 0;

    public AddObjectMode(GLEngine engine, Area area, Kit? kit, object selectedPiece, DesignerSettings settings) : base(engine, area, kit, selectedPiece, settings)
    {
    }

    public override async Task RenderIntercept(OrbitCamera camera, Point mouse, List<IDrawCallDescriptor> descriptors)
    {
        if (SelectedObjectTemplate is null)
            return;

        var ray = camera.ProjectRay((int)mouse.X, (int)mouse.Y, _engine.Width, _engine.Height);
        var point = ray.FindPointOnPlane(Axis.Z, 0);

        // todo - should be placed in room where walkmesh intersects.
        // todo - should not be hardcoded
        _addObject = new(_area.Rooms.Last(), SelectedObjectTemplate); 
        _addObject.LocalPosition = point;
        _addObject.LocalOrientation = Quaternion.CreateFromYawPitchRoll(0, 0, angle * (float)Math.PI / 180);

        var roomDescriptors = new List<IDrawCallDescriptor>();
        _areaEntity.RenderObject(_engine.AssetManager, _addObject, ref roomDescriptors);
        roomDescriptors.OfType<MeshDescriptor>().ToList().ForEach(x => x.AmbientColor = new Vector3(1.5f, 1.5f, 1.5f));
        descriptors.AddRange(roomDescriptors);
    }

    public override async Task Trigger()
    {
        if (SelectedObjectTemplate is null)
            return;

        // todo - add to room within bounds of the cursor
        var room = _area.Rooms.First();
        room.AddObject(_addObject);
    }
}
