using System;
using System.IO;
using System.Linq;
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

    public void LoadModel(TModel model);
    public TModel BuildModel();

    public void NewFile();

    public void LoadFromFile(string filepath, ResRef resref, ResourceType resourceType);
    public void LoadFromFile(string filepath);
    public void LoadFromFile();

    public void SaveToFile(string filepath, ResRef resref, ResourceType resourceType);
    public void SaveToFile(string filepath);
    public void SaveToFile();

    public byte[] SerializeModelToBytes();
}

public abstract class ResourceEditorViewModelBase<TViewModel, TModel>
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


    public ResourceEditorViewModelBase()
    {
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

    public void LoadFromFile(string filepath, ResRef resref, ResourceType resourceType)
    {
        FilePath = filepath;
        ResRef = resref.Get();
        ResourceType = resourceType;
        LoadFromFile();
    }
    public void LoadFromFile(string filepath)
    {
        FilePath = filepath;
        ResRef = Path.GetFileNameWithoutExtension(filepath);
        ResourceType = ResourceType.ByExtension(Path.GetExtension(filepath).Replace(".", ""));
        LoadFromFile();
    }
    public void LoadFromFile()
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

    public void SaveToFile(string filepath, ResRef resref, ResourceType resourceType)
    {
        FilePath = filepath;
        ResRef = resref.Get();
        ResourceType = resourceType;
        SaveToFile();
    }
    public void SaveToFile(string filepath)
    {
        FilePath = filepath;
        SaveToFile();
    }
    public void SaveToFile()
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

    public abstract void LoadModel(TModel model);
    public abstract TModel BuildModel();

    public abstract TModel DeserializeModel(byte[] bytes);
    public abstract TModel DeserializeModel(string path);

    public abstract byte[] SerializeModelToBytes();
    public abstract void SerializeModelToFile();
}
