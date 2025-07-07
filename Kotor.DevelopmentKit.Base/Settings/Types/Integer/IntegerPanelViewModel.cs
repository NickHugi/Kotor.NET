using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.Base.Settings.Types.Integer;

public class IntegerPanelViewModel : SettingsViewModel
{
    public string Name => _attribute.Name;
    public string Description => _attribute.Description;
    public int Value
    {
        get => (int?)_propertyInfo.GetValue(_instance) ?? throw new InvalidOperationException();
        set => _propertyInfo.SetValue(_instance, value);
    }

    private readonly IntegerSettingAttribute _attribute;
    private readonly PropertyInfo _propertyInfo;
    private readonly object _instance;

    public IntegerPanelViewModel(IntegerSettingAttribute attribute, PropertyInfo propertyInfo, object instance)
    {
        _attribute = attribute;
        _propertyInfo = propertyInfo;
        _instance = instance;
    }
}
