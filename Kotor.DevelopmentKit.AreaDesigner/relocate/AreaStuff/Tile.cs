using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Hooks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class Tile : UltimateWorldObject, IDeleteable
{
    public Room Parent { get; }

    public IReadOnlyCollection<Magnet> Magnets
    {
        get => AttachedObjects.SelectMany(x =>
        {
            IEnumerable<Magnet> magnets = [];

            if (x is Wall wall)
            {
                if (wall.DoorFrame is null)
                {
                    magnets = wall.Magnets;
                }
                else
                {
                    magnets = wall.DoorFrame.Magnets;
                }
            }

            return magnets.Where(x =>
                (x.Parent is DoorFrame doorframe)
                || (x.Parent is Wall wall && wall?.LinkedTile is null));
        }).ToList();
    }

    public TileTemplate Template => Kit.Manager.Get(KitID).Tile(TemplateID);

    public Tile(Room parent, TileTemplate template) : base(parent, template, Guid.NewGuid())
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
            ..template.Magnets.OfType<FloorHookTemplate>().Select(x => new Floor(this, kit.Floor(x.TemplateID))),
            ..template.Magnets.OfType<CeilingHookTemplate>().Select(x => new Ceiling(this, kit.Ceiling(x.TemplateID))),
            ..template.Magnets.OfType<WallHookTemplate>().Select(x => new Wall(this, kit.Wall(x.TemplateID), x)),
            // TODO
            //..template.Magnets.OfType<CornerHookTemplate>().Select(x => new InnerCorner(this, kit.InnerCorner(x.InnerTemplateID), x)),
            //..template.Magnets.OfType<CornerHookTemplate>().Select(x => new OuterCorner(this, kit.OuterCorner(x.OuterTemplateID), x)),
        ]);
    }

    public void Delete()
    {
        Parent.DeleteTile(this);
    }
}
