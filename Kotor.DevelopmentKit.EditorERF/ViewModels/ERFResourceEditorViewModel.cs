using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Base.DialogResults;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorERF;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorERF.ViewModels;

public class ERFResourceEditorViewModel : BaseResourceEditorViewModel<ERFViewModel>
{
    public override string WindowTitlePrefix => "ERF Editor";

    public ActionHistory<ERFResourceEditorViewModel> History
    {
        get => field;
    }

    public string ImportFolderPath
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = "";

    public int SelectedIndex
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public Interaction<Unit, LoadFromERFWindowDialogResult> OpenInteraction { get; }
    public Interaction<Unit, SaveToERFWindowDialogResult> SaveAsInteraction { get; }
    public Interaction<Unit, string> SelectImportFolderInteraction { get; }
    public Interaction<Unit, IEnumerable<string>> SelectFilesInteraction { get; }

    public ERFResourceEditorViewModel() : base(null)
    {
        History = new(this);

        Resource = new();
        OpenInteraction = new();
        SaveAsInteraction = new();
        SelectImportFolderInteraction = new();
        SelectFilesInteraction = new();
    }

    public override void NewFile()
    {
        FilePath = null;
        Resource.Load(new ERF());
    }

    protected override void DeserializeAndLoad(byte[] data)
    {
        using var memoryStream = new MemoryStream(data);
        Resource.Deserialize(ResourceType, memoryStream);
    }
    protected override void DeserializeAndLoad(string filepath)
    {
        var fileStream = File.OpenRead(filepath);
        Resource.Deserialize(ResourceType, fileStream);
    }

    public override byte[] SerializeModelToBytes()
    {
        return Resource.Serialize(ResourceType);
    }
    public override void SerializeModelToFile()
    {
        using var fileStream = File.OpenWrite(FilePath);
        Resource.Serialize(ResourceType, fileStream);
    }

    public void New()
    {

        FilePath = null;
        ResRef = "";
        ResourceType = null;
        Resource.Load(new ERF());
    }

    public async Task Open()
    {
        var result = await OpenInteraction.Handle(Unit.Default);
        if (result is not null)
        {
            FilePath = result.FilePath;
            ResRef = result.ResRef;
            ResourceType = result.ResourceType;
            DeserializeAndLoad(FilePath);
        }
    }

    public void Save()
    {
        SerializeModelToFile();
    }

    public async Task SaveAs()
    {
        var result = await SaveAsInteraction.Handle(Unit.Default);
        if (result is not null)
        {
            FilePath = result.FilePath;
            ResRef = result.ResRef;
            ResourceType = result.ResourceType;
            SerializeModelToFile();
        }
    }

    public void Reset()
    {
        if (FilePath is not null)
        {
            DeserializeAndLoad(FilePath);
        }
    }

    public void Undo()
    {
        History.Undo();
    }

    public void Redo()
    {
        History.Redo();
    }

    public async Task SelectImportFolder()
    {
        var result = await SelectImportFolderInteraction.Handle(Unit.Default);
        if (result is not null)
        {
            ImportFolderPath = result;
        }
    }

    public void Import()
    {
        if (!Directory.Exists(ImportFolderPath))
        {
            // TODO - popup error
            return;
        }

        foreach (var file in Directory.GetFiles(ImportFolderPath))
        {
            Resource.AddOrOverrideResource(new()
            {
                ResRef = Path.GetFileNameWithoutExtension(file),
                ResourceType = ResourceType.FromFilepath(file),
                Data = File.ReadAllBytes(file)
            });
        }
    }

    public void RemoveSelected()
    {
        Resource.Resources.RemoveAt(SelectedIndex);
    }

    public void Clear()
    {
        Resource.Resources.Clear();
    }

    public async Task AddFiles()
    {
        var files = await SelectFilesInteraction.Handle(Unit.Default);

        foreach (var file in files)
        {
            Resource.AddOrOverrideResource(new()
            {
                ResRef = Path.GetFileNameWithoutExtension(file),
                ResourceType = ResourceType.FromFilepath(file),
                Data = File.ReadAllBytes(file)
            });
        }
    }

    public async Task ExtractFiles()
    {
        // TODO
    }
}
