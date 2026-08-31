using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.Views;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.ViewModels;

public class AreaSettingsViewModel : ReactiveObject
{
    public string AreaID
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public string TextureID
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public bool OverrideTextures
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public AreaSettingsViewModel(Area area)
    {
        AreaID = area.AreaID;
        TextureID = area.TextureID;
        OverrideTextures = area.OverrideTextures;
    }

    public void Apply(Area area)
    {
        area.AreaID = AreaID;
        area.TextureID = TextureID;
        area.OverrideTextures = OverrideTextures;
    }
}
