using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.FieldPanel;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldBinaryPanel : EditFieldPanel<BinaryGFFNodeViewModel, byte[], BinaryEditedEventArgs>
{
    public required BinaryPanelViewModel ViewModel
    {
        get => (DataContext as BinaryPanelViewModel)!;
        set => DataContext = value;
    }


    public FieldBinaryPanel() : base()
    {
        InitializeComponent();
    }

    protected override void RaiseFinishedEditing()
    {
        RoutedEventArgs args = new BinaryEditedEventArgs(FinishedEditingEvent, this, ViewModel.Value);
        RaiseEvent(args);
    }

    protected override byte[] GetCurrentValue()
    {
        return SourceNode?.FieldValue ?? [];
    }

    private async void Import_Click(object? sender, RoutedEventArgs e)
    {
        var files = await TopLevel.GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(new());

        if (files is not null && files.Count > 0)
        {
            var file = files.First();
            ViewModel.Value = File.ReadAllBytes(file.Path.LocalPath);
        }
    }

    private async void Export_Click(object? sender, RoutedEventArgs e)
    {
        var file = await TopLevel.GetTopLevel(this)!.StorageProvider.SaveFilePickerAsync(new());

        if (file is not null)
        {
            File.WriteAllBytes(file.Path.LocalPath, ViewModel.Value);
        }
    }
}
