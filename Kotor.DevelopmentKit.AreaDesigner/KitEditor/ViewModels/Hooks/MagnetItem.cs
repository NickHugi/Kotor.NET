using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Magnets;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Hooks;

public class MagnetItem : BaseMagnetItem
{
    public override string Name => "Magnet";
    public override MagnetType MagnetType => MagnetType.Magnet;

    public MagnetItem() : base()
    {
    }
    public MagnetItem(UltimateMagnetTemplate magnet) : this()
    {
        Position = new(magnet.LocalPosition);
        Orientation = new(magnet.LocalOrientation);
    }

    public override UltimateMagnetTemplate ToModel()
    {
        return new UltimateMagnetTemplate
        {
            LocalPosition = Position.ToModel(),
            LocalOrientation = Orientation.ToModel(),
            MagnetType = MagnetType.Magnet
        };
    }
}
