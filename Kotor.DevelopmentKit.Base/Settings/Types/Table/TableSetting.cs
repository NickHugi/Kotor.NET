using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Kotor.DevelopmentKit.Base.Settings.Types.Table;

public class TableSettingAttribute : SettingAttribute
{
    public TableSettingAttribute(string name, string description) : base(name, description)
    {
    }

    public override SettingsViewModel GetViewModel(PropertyInfo propertyInfo, object instance)
    {
        if (propertyInfo.PropertyType.GetInterface(nameof(IEnumerable)) is not null)
        {
            //var type = typeof(ICollection<>).MakeGenericType(propertyInfo.PropertyType.GenericTypeArguments.First());
            //Activator.CreateInstance(type, [this, propertyInfo, instance]);
            return new TablePanelViewModel(this, propertyInfo, instance);
        }
        else
        {
            throw new ArgumentException();
        }
    }
}
