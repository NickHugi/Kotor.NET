using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.Base.Settings.Types.String;

public class StringSettingAttribute : SettingAttribute
{
    public StringSettingAttribute(string name, string description) : base(name, description)
    {
    }

    public override SettingsViewModel GetViewModel(PropertyInfo propertyInfo, object instance)
    {
        return new StringPanelViewModel(this, propertyInfo, instance);
    }
}
