using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using DynamicData;
using Kotor.DevelopmentKit.Base.Windows;
using Kotor.NET.Common.Data;
using Kotor.NET.Encapsulations;
using Kotor.NET.Tests.Encapsulation;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ViewModels;

public class EncapsulatedResourcePickerDialogViewModel : ReactiveObject
{
    private EncapsulatedResourceListViewModel _resourceList = default!;
    public EncapsulatedResourceListViewModel ResourceList
    {
        get => _resourceList;
        private set => this.RaiseAndSetIfChanged(ref _resourceList, value);
    }

    private ResourceType[] _resourceTypeOptions = default!;
    public ResourceType[] ResourceTypeOptions
    {
        get => _resourceTypeOptions;
        private set => this.RaiseAndSetIfChanged(ref _resourceTypeOptions, value);
    }

    private ResourceType _resourceType = default!;
    public ResourceType ResourceType
    {
        get => _resourceType;
        set => this.RaiseAndSetIfChanged(ref _resourceType, value);
    }

    private string _filepath = "";
    public string FilePath
    {
        get => _filepath;
        set => this.RaiseAndSetIfChanged(ref _filepath, value);
    }

    private readonly Interaction<Exception, Unit> _loadingError = new();
    public Interaction<Exception, Unit> ExceptionEvent => this._loadingError;


    public EncapsulatedResourcePickerDialogViewModel()
    {
        this.ObservableForProperty(x => x.ResourceType)
            .Subscribe(x =>
            {
                if (ResourceList is not null)
                {
                    ResourceList.ResourceTypeFilter = [x.Value];
                }
            });
    }


    public EncapsulatedResourcePickerDialogViewModel LoadModel(string filepath, IEnumerable<ResourceType> resourceTypeOptions)
    {
        FilePath = filepath;
        ResourceList = new();
        ResourceTypeOptions = resourceTypeOptions.ToArray();
        ResourceType = _resourceTypeOptions.First();

        Task.Run(() =>
        {
            try
            {
                var encapsulator = Encapsulation.LoadFromPath(filepath);
                AvaloniaScheduler.Instance.Schedule(() => ResourceList.LoadModel(encapsulator, null));
            }
            catch (Exception ex)
            {
                AvaloniaScheduler.Instance.Schedule(async () => await _loadingError.Handle(ex));
            }
        });

        return this;
    }
}
