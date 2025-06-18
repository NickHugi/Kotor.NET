using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.Base.Settings.Types;

public abstract class SettingAttribute : Attribute
{
    public string Name { get; }
    public string Description { get; }

    public SettingAttribute(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public abstract SettingsViewModel GetViewModel(PropertyInfo propertyInfo, object instance);
}
