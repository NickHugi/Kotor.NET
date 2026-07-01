using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Hooks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class DoorFrame : IWorldObject
{
    public Wall Parent { get; }
    public Wall? AdjacentWall
    {
        get
        {
            var area = Parent.Parent.Parent.Parent;
            var walls = area.Rooms
                .SelectMany(x => x.Objects.OfType<Tile>())
                .SelectMany(x => x.VirtualObjects.OfType<Wall>())
                .ToList();

            return walls.FirstOrDefault(x => x != Parent && AllMagnets.Any(y => y.GlobalPosition == x.GlobalPosition));
        }
    }

    private IReadOnlyCollection<Magnet> AllMagnets
    {
        get
        {
            return Template.Magnets.Select(x => new Magnet(this)
            {
                Type = MagnetType.Hook,
                LocalPosition = x.LocalPosition,
                LocalOrientation = x.LocalOrientation,
            }).ToList();
        }
    }
    public IReadOnlyCollection<Magnet> Magnets
    {
        get => AllMagnets.Where(x => x.GlobalPosition != Parent.GlobalPosition).ToList();
    }
    public WorldObjectType Type => WorldObjectType.Prop;

    public string KitID { get; private set; } = "";
    public string TemplateID { get; private set; } = "";
    public DoorFrameTemplate Template => Kit.Manager.Get(KitID).DoorFrame(TemplateID);

    public string? GroupID { get; set; }

    public bool Enabled { get; set; } = true;

    public IEnumerable<DoorFrameHook> Hooks => Template.Magnets.OfType<DoorFrameHookTemplate>().Select(x => new DoorFrameHook(this, x));

    public Vector3 LocalPosition
    {
        get => new();//Template.Magnets.First().LocalPosition;
        set => throw new NotImplementedException(); // TODO
    }
    public Quaternion LocalOrientation
    {
        get => Quaternion.Identity; //Template.Magnets.First().LocalOrientation;
        set => throw new NotImplementedException(); // TODO
    }
    public Matrix4x4 LocalTransform => Matrix4x4.CreateRotationZ(MathF.PI) * Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

    public Vector3 GlobalPosition
    {
        get => Matrix4x4.Decompose(GlobalTransform, out _, out _, out var value) ? value : new();
        set => throw new NotImplementedException(); // TODO
    }
    public Quaternion GlobalOrientation
    {
        get => Matrix4x4.Decompose(GlobalTransform, out _, out var value, out _) ? value : new();
        set => throw new NotImplementedException(); // TODO
    }
    public Matrix4x4 GlobalTransform => LocalTransform * Parent.GlobalTransform;

    public bool Visible => Enabled;

    public DoorFrame(Wall parent, DoorFrameTemplate template)
    {
        Parent = parent;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(WorldObjectTemplate template)
    {
        if (template is not DoorFrameTemplate doorframeTemplate)
            throw new ArgumentException();

        SwitchTemplate(doorframeTemplate);
    }
    public void SwitchTemplate(DoorFrameTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.TemplateID;
    }
}
