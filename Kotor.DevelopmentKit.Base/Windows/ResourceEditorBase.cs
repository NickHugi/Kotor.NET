using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Base.DialogResults;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.NET.Common.Data;
using Kotor.NET.Encapsulations;
using Kotor.NET.Resources.KotorERF;
using Kotor.NET.Resources.KotorRIM;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.Windows;

public abstract class ResourceEditorBase<TEditorViewModel, TResourceViewModel, TResourceModel> : Window
    where TEditorViewModel : IResourceEditorViewModel<TResourceViewModel, TResourceModel>
    where TResourceViewModel : ReactiveObject
    where TResourceModel : new()
{
    public abstract FilePickerOpenOptions FilePickerOpenOptions { get; }
    public abstract FilePickerSaveOptions FilePickerSaveOptions { get; }
    public abstract List<ResourceType> ResourceTypes { get; }

    public TEditorViewModel Context => (TEditorViewModel)DataContext!;


    protected async Task<SaveToERFWindowDialogResult?> SaveResourcePicker()
    {
        var file = await TopLevel.GetTopLevel(this)!.StorageProvider.SaveFilePickerAsync(FilePickerSaveOptions);

        if (file is null)
        {
            // No file was chosen.
        }
        else if (Encapsulation.IsPathEncapsulation(file.Path.LocalPath))
        {
            var encapsulatorPicker = new SaveToERFWindow();
            encapsulatorPicker.DataContext = new SaveToERFWindowViewModel().LoadModel(file.Path.LocalPath, ResourceTypes);

            return await encapsulatorPicker.ShowDialog<SaveToERFWindowDialogResult>(this);
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

    protected async Task<LoadFromERFWindowDialogResult> OpenResourcePicker()
    {
        var files = await GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(FilePickerOpenOptions);

        var file = files.FirstOrDefault();

        if (file is null)
        {
            // No file was chosen.
        }
        else if (Encapsulation.IsPathEncapsulation(file.Path.LocalPath))
        {
            var encapsulatorPicker = new LoadFromERFWindow();
            encapsulatorPicker.DataContext = new LoadFromERFWindowViewModel().LoadModel(file.Path.LocalPath, ResourceTypes);

            return await encapsulatorPicker.ShowDialog<LoadFromERFWindowDialogResult>(this);
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


    public void NewFile()
    {
        Context.NewFile();
    }

    public async Task OpenFile()
    {
        var resource = await OpenResourcePicker();

        if (resource is not null)
        {
            Context.LoadFromFile(resource.FilePath, resource.ResRef, resource.ResourceType);
        }
    }

    public async void SaveFile()
    {
        SaveFileAlternativesDialogResult alternativeSave = SaveFileAlternativesDialogResult.ToCurrent;

        if (Context.FilePath is null)
        {
            return;
        }
        else if (ResourceType.FromFilepath(Context.FilePath) == ResourceType.KEY)
        {
            // You cannot save directly to the KEY/BIF files. Consider saving into the override folder instead.
            var context = new SaveFileAlternativesDialogViewModel() { FilePath = Context.FilePath };
            alternativeSave = await new SaveFileAlternativesDialog() { Context = context }.ShowDialog<SaveFileAlternativesDialogResult>(this);
        }
        else if (ResourceType.FromFilepath(Context.FilePath) == ResourceType.RIM)
        {
            // You cannot save directly to a RIM file. Did you want to save to a corresponding MOD file or the override instead?
            var context = new SaveFileAlternativesDialogViewModel() { FilePath = Context.FilePath };
            alternativeSave = await new SaveFileAlternativesDialog() { Context = context }.ShowDialog<SaveFileAlternativesDialogResult>(this);
        }

        if (alternativeSave == SaveFileAlternativesDialogResult.ToOverride)
        {
            // TODO - requires kotor game setup
            // TODO - save file in override folder
        }
        else if (alternativeSave == SaveFileAlternativesDialogResult.ToFile)
        {
            await SaveFileAs();
        }
        else if (alternativeSave == SaveFileAlternativesDialogResult.ToMOD)
        {
            if (ResourceType.RIM.IsFileSameType(Context.FilePath))
            {
                var modFilepath = Context.FilePath.Replace(".rim", ".mod");
                var data = Context.SerializeModelToBytes();

                if (File.Exists(modFilepath))
                {
                    var erf = ERF.FromFile(modFilepath);
                    erf.AddOrReplace(Context.ResRef, Context.ResourceType, data);
                    ERF.ToFile(erf, modFilepath);
                }
                else
                {
                    var erf = ERF.FromRIM(RIM.FromFile(Context.FilePath));
                    erf.AddOrReplace(Context.ResRef, Context.ResourceType, data);
                    ERF.ToFile(erf, modFilepath);
                }

                Context.FilePath = modFilepath;
            }
            else
            {
                throw new NotImplementedException(); // TODO
            }
        }
        else if (alternativeSave == SaveFileAlternativesDialogResult.ToCurrent)
        {
            Context.SaveToFile();
        }
        else if (alternativeSave == SaveFileAlternativesDialogResult.None)
        {
            return;
        }
    }

    public async Task SaveFileAs()
    {
        var resource = await SaveResourcePicker();

        if (resource is not null)
        {
            Context.SaveToFile(resource.FilePath, resource.ResRef, resource.ResourceType);
        }
    }

    public void ResetFile()
    {
        Context.LoadFromFile();
    }


    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        Context.ExceptionInteraction.RegisterHandler(async interaction =>
        {
            await ExceptionDialog.ShowDilaog(this, interaction.Input);
            interaction.SetOutput(Unit.Default);
        });
    }
}
