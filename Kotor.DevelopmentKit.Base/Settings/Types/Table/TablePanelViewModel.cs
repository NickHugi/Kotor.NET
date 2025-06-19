using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Settings.Types.Table;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.Settings.Types.Table;

public class TablePanelViewModel : SettingsViewModel
{
    public string Name => _attribute.Name;
    public string Description => _attribute.Description;
    public ICollection Value
    {
        get => (ICollection?)_propertyInfo.GetValue(_instance) ?? throw new InvalidOperationException();
        set => _propertyInfo.SetValue(_instance, value);
    }
    public object? SelectedItem
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    private readonly TableSettingAttribute _attribute;
    private readonly PropertyInfo _propertyInfo;
    private readonly object _instance;

    public TablePanelViewModel(TableSettingAttribute attribute, PropertyInfo propertyInfo, object instance)
    {
        _attribute = attribute;
        _propertyInfo = propertyInfo;
        _instance = instance;
    }
}
