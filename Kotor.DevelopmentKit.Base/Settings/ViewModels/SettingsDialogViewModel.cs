using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Settings.Attributes;
using Kotor.DevelopmentKit.Base.Settings.Interfaces;
using Kotor.DevelopmentKit.Base.Settings.Types;
using Kotor.DevelopmentKit.Base.Settings.Values;
using Kotor.NET.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.Settings.ViewModels;

public class SettingsDialogViewModel : ReactiveObject
{
    public IReadOnlyCollection<SettingsPageViewModel> Pages
    {
        get
        {
            return _settings
                .GetType()
                .GetProperties()
                .Where(x => x.PropertyType.GetCustomAttribute<PageAttribute>() is not null)
                .Select(x => x.PropertyType.GetCustomAttribute<PageAttribute>()!.GetViewModel(x.GetValue(_settings)))
                .ToList();
        }
    }
    public SettingsPageViewModel? SelectedPage
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }

    public Interaction<Unit, Unit> CloseInteraction { get; }

    private readonly DefaultSettingsRoot _settings;
    private readonly ISaveSettingsService _saveSettingsService;
    private readonly ILoadSettingsService _loadSettingsService;


    public SettingsDialogViewModel(
        DefaultSettingsRoot settings,
        ISaveSettingsService saveSettingsService,
        ILoadSettingsService loadSettingsService)
    {
        _settings = settings;
        _saveSettingsService = saveSettingsService;
        _loadSettingsService = loadSettingsService;

        SaveCommand = ReactiveCommand.Create(Save);
        CancelCommand = ReactiveCommand.Create(Cancel);

        CloseInteraction = new();
    }

    public void Save()
    {
        _saveSettingsService.Save(DefaultSettingsRoot.SettingsFilepath, _settings);

        Close();
    }

    public void Cancel()
    {
        var settings = _loadSettingsService.Load(DefaultSettingsRoot.SettingsFilepath, _settings.GetType());
        JsonConvert.PopulateObject(JsonConvert.SerializeObject(settings), _settings, new JsonSerializerSettings
        {
            ContractResolver = new CollectionClearingContractResolver()
        });

        Close();
    }

    public void Close()
    {
        CloseInteraction.Handle(Unit.Default).Subscribe();
    }
}

public class CollectionClearingContractResolver : DefaultContractResolver
{
    protected override JsonArrayContract CreateArrayContract(Type objectType)
    {
        var c = base.CreateArrayContract(objectType);
        c.OnDeserializingCallbacks.Add((obj, streamingContext) =>
        {
            var list = obj as IList;
            if (list != null && !list.IsReadOnly)
                list.Clear();
        });
        return c;
    }
}
