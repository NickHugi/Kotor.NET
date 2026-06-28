using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class TileItem : ObjectItem
{
    public override WorldObjectType WorldObjectType => WorldObjectType.Tile;

    public TileItem() : base()
    {
    }
    public TileItem(TileTemplate tile) : base(tile)
    {
    }

    public override TileTemplate ToModel()
    {
        return new TileTemplate
        {
            KitID = KitID,
            TemplateID = TemplateID,
            Name = Name,
            Hooks = Hooks.Select(x => x.ToModel()).ToArray(),
            Model = null,
            ClassID = null,
        };
    }
}
