using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.Settings;

public abstract class BaseSettingPage : ReactiveObject
{
    [JsonIgnore]
    public abstract string PageName { get; }

    public virtual IReadOnlyCollection<BaseSettingPage> Pages { get; } = [];
}

public class StubPage : BaseSettingPage
{
    public override string PageName { get; }
    public override IReadOnlyCollection<BaseSettingPage> Pages { get; }

    public StubPage(string pageName, IReadOnlyCollection<BaseSettingPage> pages)
    {
        PageName = pageName;
        Pages = pages;
    }
}
