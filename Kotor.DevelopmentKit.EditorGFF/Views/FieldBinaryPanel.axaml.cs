using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
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
        RoutedEventArgs args = new BinaryEditedEventArgs(FinishedEditingEvent, this, ViewModel.SourcePath, ViewModel.Value);
        RaiseEvent(args);
    }

    protected override byte[] GetCurrentValue()
    {
        return SourceNode?.FieldValue ?? [];
    }
}
