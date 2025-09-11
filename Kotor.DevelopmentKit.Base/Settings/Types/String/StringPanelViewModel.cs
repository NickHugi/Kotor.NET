using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.Base.Settings.Types.String;

public class StringPanelViewModel : SettingsViewModel
{
    public string Name => _attribute.Name;
    public string Description => _attribute.Description;
    public string Value
    {
        get => (string?)_propertyInfo.GetValue(_instance) ?? throw new InvalidOperationException();
        set => _propertyInfo.SetValue(_instance, value);
    }

    private readonly StringSettingAttribute _attribute;
    private readonly PropertyInfo _propertyInfo;
    private readonly object _instance;

    public StringPanelViewModel(StringSettingAttribute attribute, PropertyInfo propertyInfo, object instance)
    {
        _attribute = attribute;
        _propertyInfo = propertyInfo;
        _instance = instance;
    }
}
