using System;
using System.Collections.Generic;
using System.Linq;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Hooks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class Tile : UltimateWorldObject, IDeleteable
{
    public Room Parent { get; }

    public IReadOnlyCollection<Magnet> Magnets
    {
        get => [];
        // TODO
        //get => AttachedObjects.SelectMany(x =>
        //{
        //    IEnumerable<Magnet> magnets = [];

        //    if (x.Type == WorldObjectType.Wall)
        //    {
        //        if (x.DoorFrame is null)
        //        {
        //            magnets = x.Magnets;
        //        }
        //        else
        //        {
        //            magnets = x.DoorFrame.Magnets;
        //        }
        //    }

        //    return magnets.Where(x =>
        //        (x.Parent is DoorFrame doorframe)
        //        || (x.Parent is Wall wall && wall?.LinkedTile is null));
        //}).ToList();
    }

    public TileTemplate Template => Kit.Manager.Get(KitID).Tile(TemplateID);

    public Tile(Room parent, TileTemplate template) : base(parent, null, template, Guid.NewGuid(), WorldObjectType.Tile)
    {
        Parent = parent;
    }

    public void SwitchTemplate(UltimateWorldObjectTemplate template)
    {
        //if (template is not TileTemplate tileTemplate)
            throw new ArgumentException();

        //SwitchTemplate(tileTemplate);
    }
    public void SwitchTemplate(TileTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.TemplateID;

        var kit = Kit.Manager.Get(KitID);

        var virtualObjects = new List<UltimateWorldObject>();
        AttachedObjects = virtualObjects;

        virtualObjects.AddRange(
        [
            .. base.Magnets.Where(x => x.Template is WallHookTemplate).Select(x => new UltimateWorldObject(Parent, x, x.Template.Template, Guid.NewGuid(), WorldObjectType.Wall)),
            .. base.Magnets.Where(x => x.Template is FloorHookTemplate).Select(x => new UltimateWorldObject(Parent, x, x.Template.Template, Guid.NewGuid(), WorldObjectType.Floor)),
            .. base.Magnets.Where(x => x.Template is CeilingHookTemplate).Select(x => new UltimateWorldObject(Parent, x, x.Template.Template, Guid.NewGuid(), WorldObjectType.Ceiling)),
            .. base.Magnets.Where(x => x.Template is CornerHookTemplate).Select(x => new UltimateWorldObject(Parent, x, x.Template.Template, Guid.NewGuid(), WorldObjectType.InnerCorner)),
            //.. base.Magnets.Where(x => x.Template is CornerHookTemplate).Select(x => new InnerCorner(this, x, x.Template.Template as InnerCornerTemplate)),
            // TODO
        ]);
    }

    public void Delete()
    {
        Parent.DeleteTile(this);
    }
}
