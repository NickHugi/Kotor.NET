using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Kotor.DevelopmentKit.Base.Settings.Types;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.Settings;

public abstract class BaseSettingPage : ReactiveObject
{
    [JsonIgnore]
    public abstract string PageName { get; }
    public virtual IReadOnlyCollection<BaseSettingPage> Pages { get; } = [];

    public SettingsViewModel[] Properties => GetType()
        .GetProperties()
        .Select(x => (x, x.GetCustomAttribute<SettingAttribute>()))
        .Where(x => x.Item2 is not null)
        .Select(x => x.Item2!.GetViewModel(x.x, this))
        .ToArray();
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
