using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class WorldObjectTypeItem
{
    public string Name
    {
        get => (Value is null)
            ? "All"
            : Value.ToString();
    }

    public WorldObjectType? Value { get; }

    public WorldObjectTypeItem(WorldObjectType? worldObjectType)
    {
        Value = worldObjectType;
    }

    public static WorldObjectTypeItem All = new(null);
}
