using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Settings.Types;
using Kotor.DevelopmentKit.Base.Settings.ViewModels;

namespace Kotor.DevelopmentKit.Base.Settings.Attributes;

public class PageAttribute : Attribute
{
    public string Name { get; }

    public PageAttribute(string name)
    {
        Name = name;
    }

    public SettingsPageViewModel GetViewModel(object instance)
    {
        var instanceType = instance.GetType();
        var pageAttribute = instanceType.GetCustomAttribute<PageAttribute>();

        var properties = instanceType
            .GetProperties()
            .Select(x => (x, x.GetCustomAttribute<SettingAttribute>()))
            .Where(x => x.Item2 is not null)
            .Select(x => x.Item2!.GetViewModel(x.x, instance))
            .ToArray();

        var pages = instanceType
            .GetProperties()
            .Select(x => (x.GetValue(instance), x.PropertyType.GetCustomAttribute<PageAttribute>()))
            .Where(x => x.Item2 is not null)
            .Select(x => GetViewModel(x.Item1))
            .ToList();

        return new(pageAttribute.Name, pages, properties);
    }
}
