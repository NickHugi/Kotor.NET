using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.Base.Settings.Types.Boolean;

public class BooleanSettingAttribute : SettingAttribute
{
    public BooleanSettingAttribute(string name, string description) : base(name, description)
    {
    }

    public override SettingsViewModel GetViewModel(PropertyInfo propertyInfo, object instance)
    {
        return new BooleanPanelViewModel(this, propertyInfo, instance);
    }
}
