using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Templates;

public class TileItem : WorldObjectItem
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
            Magnets = Hooks.Select(x => x.ToModel()).ToArray(),
            Model = null,
            ClassID = null,
        };
    }
}
