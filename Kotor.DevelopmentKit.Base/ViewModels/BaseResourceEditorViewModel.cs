using System;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Settings.Values;
using Kotor.NET.Common.Data;
using Kotor.NET.Encapsulations;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ViewModels;

public interface IResourceEditorViewModel<TViewModel, TModel>
    where TViewModel : ReactiveObject
    where TModel : new()
{
    public string FilePath { get; set; }
    public ResourceType ResourceType { get; set; }
    public TViewModel Resource { get; set; }
    public string ResRef { get; set; }

    public Interaction<Exception, Unit> ExceptionInteraction { get; }

    public void LoadModel(TModel model);
    public TModel BuildModel();

    public void NewFile();

    public Task LoadFromFile(string filepath, ResRef resref, ResourceType resourceType);
    public Task LoadFromFile(string filepath);
    public Task LoadFromFile();

    public Task SaveToFile(string filepath, ResRef resref, ResourceType resourceType);
    public Task SaveToFile(string filepath);
    public Task SaveToFile();

    public byte[] SerializeModelToBytes();
}

public abstract class BaseResourceEditorViewModel<TViewModel, TModel>
    : ReactiveObject, IResourceEditorViewModel<TViewModel, TModel>
    where TViewModel : ReactiveObject
    where TModel : new()
{
    /// <summary>
    /// The path only including either the last directory leading up to the file, or if the file
    /// is contained within an ERF-like format, then the filename of that is used in the last directory
    /// name's stead.
    /// </summary>
    public string FilePathSnippet
    {
        get
        {
            if (FilePath is null)
            {
                return "";
            }
            else if (Encapsulation.IsPathEncapsulatedInFile(FilePath))
            {
                var encapsulatorName = FilePath.Split(Path.DirectorySeparatorChar).Last();
                return $"{encapsulatorName}/{ResourceFilename}";
            }
            else
            {
                var lastDirectory = Path.GetDirectoryName(FilePath).Split(Path.DirectorySeparatorChar).Last();
                return $"{lastDirectory}/{ResourceFilename}";
            }
        }
    }
    public abstract string WindowTitlePrefix { get; }
    public string WindowTitle => WindowTitlePrefix + ((FilePathSnippet == "") ? ("") : ($" - {FilePathSnippet}"));

    public string ResourceFilename => (ResRef is null || ResourceType is null) ? Path.GetFileName(FilePath) : $"{ResRef}.{ResourceType.Extension}";
    public bool FilePathAssigned => FilePath is not null;

    private string? _filepath = default!;
    public string? FilePath
    {
        get => _filepath;
        set => this.RaiseAndSetIfChanged(ref _filepath, value);
    }

    private string _resref = default!;
    public string ResRef
    {
        get => _resref;
        set => this.RaiseAndSetIfChanged(ref _resref, value);
    }

    private ResourceType _resourceType = default!;
    public ResourceType ResourceType
    {
        get => _resourceType;
        set => this.RaiseAndSetIfChanged(ref _resourceType, value);
    }

    private TViewModel _resource = default!;
    public TViewModel Resource
    {
        get => _resource;
        set => this.RaiseAndSetIfChanged(ref _resource, value);
    }

    private readonly Interaction<Exception, Unit> _loadingError = new();
    public Interaction<Exception, Unit> ExceptionInteraction => this._loadingError;

    public DefaultSettingsRoot Settings { get; }

    public InstallationSettings SelectedInstallation
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    
    public BaseResourceEditorViewModel(DefaultSettingsRoot settings)
    {
        Settings = settings;

        this.ObservableForProperty(x => x.FilePath)
            .Subscribe(x => this.RaisePropertyChanged(nameof(FilePathAssigned)));

        this.WhenAnyValue(x => x.ResRef, x => x.ResourceType, x => x.FilePath)
            .Subscribe(x => this.RaisePropertyChanged(nameof(WindowTitle)));
    }


    public void NewFile()
    {
        FilePath = null;
        LoadModel(new());
    }

    public async Task LoadFromFile(string filepath, ResRef resref, ResourceType resourceType)
    {
        FilePath = filepath;
        ResRef = resref.Get();
        ResourceType = resourceType;
        await LoadFromFile();
    }
    public async Task LoadFromFile(string filepath)
    {
        FilePath = filepath;
        ResRef = Path.GetFileNameWithoutExtension(filepath);
        ResourceType = ResourceType.ByExtension(Path.GetExtension(filepath).Replace(".", ""));
        await LoadFromFile();
    }
    public async Task LoadFromFile()
    {
        try
        {
            if (Encapsulation.IsPathEncapsulation(FilePath))
            {
                var capsule = Encapsulation.LoadFromPath(FilePath);
                var data = capsule.Read(ResRef, ResourceType);
                var model = DeserializeModel(data);
                LoadModel(model);
            }
            else
            {
                var twoda = DeserializeModel(FilePath);
                LoadModel(twoda);
            }
        }
        catch (Exception ex)
        {
            NewFile();
            await _loadingError.Handle(ex);
        }
    }

    public async Task SaveToFile(string filepath, ResRef resref, ResourceType resourceType)
    {
        FilePath = filepath;
        ResRef = resref.Get();
        ResourceType = resourceType;
        await SaveToFile();
    }
    public async Task SaveToFile(string filepath)
    {
        FilePath = filepath;
        await SaveToFile();
    }
    public async Task SaveToFile()
    {
        try
        {
            if (Encapsulation.IsPathEncapsulation(FilePath!))
            {
                var capsule = Encapsulation.LoadFromPath(FilePath);
                capsule.Write(ResRef, ResourceType, SerializeModelToBytes());
            }
            else
            {
                SerializeModelToFile();
            }
        }
        catch (Exception ex)
        {
            await _loadingError.Handle(ex);
        }
    }

    public abstract void LoadModel(TModel model);
    public abstract TModel BuildModel();

    public abstract TModel DeserializeModel(byte[] bytes);
    public abstract TModel DeserializeModel(string path);

    public abstract byte[] SerializeModelToBytes();
    public abstract void SerializeModelToFile();
}
