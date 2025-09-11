using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Extensions;

namespace Kotor.DevelopmentKit.Base.Settings.Types.Enum;

public class EnumPanelViewModel : SettingsViewModel
{
    public string Name => _attribute.Name;
    public string Description => _attribute.Description;
    public System.Enum Value
    {
        get => (System.Enum?)_propertyInfo.GetValue(_instance) ?? throw new InvalidOperationException();
        set => _propertyInfo.SetValue(_instance, value);
    }
    public ICollection<ComboBoxOption> Options { get; }
    public ComboBoxOption SelectedItem
    {
        get => field;
        set
        {
            Value = (System.Enum)value.Value;
            field = value;
        }
    }

    private readonly EnumSettingAttribute _attribute;
    private readonly PropertyInfo _propertyInfo;
    private readonly object _instance;

    public EnumPanelViewModel(EnumSettingAttribute attribute, PropertyInfo propertyInfo, object instance)
    {
        _attribute = attribute;
        _propertyInfo = propertyInfo;
        _instance = instance;

        Options = System.Enum.GetValues(Value.GetType()).Cast<System.Enum>().Select(x =>
        {
            return new ComboBoxOption(x.GetEnumDescription(), x);
        }).ToList();
        SelectedItem = Options.First(x => x.Value.Equals(Value)); 
    }
}

public class ComboBoxOption
{
    public string Name { get; }
    public object Value { get; }

    public ComboBoxOption(string name, object value)
    {
        Name = name;
        Value = value;
    }
}
