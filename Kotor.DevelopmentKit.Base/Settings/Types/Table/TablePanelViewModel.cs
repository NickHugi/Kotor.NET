using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DynamicData.Binding;
using Kotor.DevelopmentKit.Base.Settings.Pages;
using Kotor.DevelopmentKit.Base.Settings.Types.Table;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.Settings.Types.Table;

public class TablePanelViewModel : SettingsViewModel
{
    public string Name => _attribute.Name;
    public string Description => _attribute.Description;
    public ObservableCollection<object> Value { get; set; }
    public object? SelectedItem
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public ICollection<SettingsViewModel> SelectedItemProperties
    {
        get
        {
            if (SelectedItem is null)
                return [];

            var instanceType = SelectedItem.GetType();

            return instanceType
                .GetProperties()
                .Select(x => (x, x.GetCustomAttribute<SettingAttribute>()))
                .Where(x => x.Item2 is not null)
                .Select(x => x.Item2!.GetViewModel(x.x, SelectedItem))
                .ToArray();
        }
    }
    public ReactiveCommand<Unit, Unit> AddCommand { get; } 
    public ReactiveCommand<Unit, Unit> RemoveCommand { get; }

    private readonly TableSettingAttribute _attribute;
    private readonly PropertyInfo _propertyInfo;
    private readonly object _instance;
    private readonly Type _rowType;

    public TablePanelViewModel(TableSettingAttribute attribute, PropertyInfo propertyInfo, object instance)
    {
        _attribute = attribute;
        _propertyInfo = propertyInfo;
        _instance = instance;
        _rowType = _propertyInfo.PropertyType.GetGenericArguments().First();

        var value = _propertyInfo.GetValue(_instance) as IList;
        Value = new(value.OfType<object>());

        AddCommand = ReactiveCommand.Create(AddAction);
        RemoveCommand = ReactiveCommand.Create(RemoveAction);

        this.WhenPropertyChanged(x => x.SelectedItem).Subscribe(x =>
        {
            this.RaisePropertyChanged(nameof(SelectedItemProperties));
        });
    }

    private void AddAction()
    {
        var instance = Activator.CreateInstance(_rowType);
        Value.Add(instance);
    }
    private void RemoveAction()
    {
        Value.Remove(SelectedItem);

    }
}
