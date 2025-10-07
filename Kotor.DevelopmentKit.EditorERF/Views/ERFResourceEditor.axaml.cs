using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Base.DialogResults;
using Kotor.DevelopmentKit.EditorERF.ViewModels;
using Kotor.NET.Common.Data;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorERF.Views;

public partial class ERFResourceEditor : ReactiveWindow<ERFResourceEditorViewModel>
{
    public ERFResourceEditor()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            if (ViewModel is null)
                throw new Exception();

            d(ViewModel.OpenInteraction.RegisterHandlerAsync(SelectResourceToLoad));
            d(ViewModel.SaveAsInteraction.RegisterHandlerAsync(SelectResourceToSave));
            d(ViewModel.SelectImportFolderInteraction.RegisterHandlerAsync(SelectImportFolder));
            d(ViewModel.SelectFilesInteraction.RegisterHandlerAsync(SelectFiles));
        });
    }

    //public override FilePickerFileType AllValidFilePickerFileTypes => throw new System.NotImplementedException();

    //public override FilePickerOpenOptions FilePickerOpenOptions => throw new System.NotImplementedException();

    //public override FilePickerSaveOptions FilePickerSaveOptions => throw new System.NotImplementedException();

    //public override List<ResourceType> ResourceTypes => throw new System.NotImplementedException();

    public async Task<LoadFromERFWindowDialogResult> SelectResourceToLoad()
    {
        var files = await GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(new()
        {
            Title = "Open ERF/RIM File",
            AllowMultiple = false,
            FileTypeFilter = [FilePickerTypes.ERF, FilePickerTypes.Encapsulated, FilePickerTypes.All],
        });

        var file = files.FirstOrDefault();

        if (file is null)
        {
            // No file was chosen.
        }
        else
        {
            var filepath = file.Path.LocalPath;
            var resourceType = ResourceType.ByExtension(Path.GetExtension(filepath).Replace(".", ""));
            var resref = Path.GetFileNameWithoutExtension(filepath);

            return new()
            {
                FilePath = filepath,
                ResourceType = resourceType,
                ResRef = resref,
            };
        }

        // No resource was chosen.
        return null;
    }

    public async Task<SaveToERFWindowDialogResult> SelectResourceToSave()
    {
        var file = await GetTopLevel(this)!.StorageProvider.SaveFilePickerAsync(new()
        {
            Title = "Save ERF/RIM File",
            FileTypeChoices = [FilePickerTypes.ERF, FilePickerTypes.MOD, FilePickerTypes.RIM],
        });

        if (file is null)
        {
            // No file was chosen.
        }
        else
        {
            var filepath = file.Path.LocalPath;
            var resourceType = ResourceType.ByExtension(Path.GetExtension(filepath).Replace(".", ""));
            var resref = Path.GetFileNameWithoutExtension(filepath);

            return new()
            {
                FilePath = filepath,
                ResourceType = resourceType,
                ResRef = resref,
            };
        }

        // No resource was chosen.
        return null;
    }

    public async Task<string?> SelectImportFolder()
    {
        var folder = await GetTopLevel(this)!.StorageProvider.OpenFolderPickerAsync(new()
        {
            Title = "Select folder to import files from",
            AllowMultiple = false,
        });

        if (folder.Count == 0)
        {
            // No file was chosen.
            return null;
        }
        else
        {
            return folder.First().Path.LocalPath;
        }
    }

    public async Task<IEnumerable<string>> SelectFiles()
    {
        var files = await GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(new()
        {
            Title = "Select files to import",
            AllowMultiple = true,
        });

        return files.Select(x => x.Path.LocalPath).ToArray();
    }
}

public static class InteractionExtensions
{
    public static IDisposable RegisterHandlerAsync<TOutput>(
        this Interaction<Unit, TOutput> interaction,
        Func<Task<TOutput>> handler
    )
    {
        return interaction.RegisterHandler(async context =>
        {
            var result = await handler();
            context.SetOutput(result);
        });
    }
}
