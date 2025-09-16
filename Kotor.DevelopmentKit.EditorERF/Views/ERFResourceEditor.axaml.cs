using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Kotor.DevelopmentKit.Base.Views;
using Kotor.DevelopmentKit.EditorERF.ViewModels;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorERF;

namespace Kotor.DevelopmentKit.EditorERF.Views;

public partial class ERFResourceEditor : ResourceEditorBase<ERFResourceEditorViewModel, ERFViewModel, ERF>
{
    public ERFResourceEditor()
    {
        InitializeComponent();
    }

    public override FilePickerFileType AllValidFilePickerFileTypes => throw new System.NotImplementedException();

    public override FilePickerOpenOptions FilePickerOpenOptions => throw new System.NotImplementedException();

    public override FilePickerSaveOptions FilePickerSaveOptions => throw new System.NotImplementedException();

    public override List<ResourceType> ResourceTypes => throw new System.NotImplementedException();
}
