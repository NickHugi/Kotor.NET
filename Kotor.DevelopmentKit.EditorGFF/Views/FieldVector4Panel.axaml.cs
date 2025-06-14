using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.FieldPanel;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldVector4Panel : EditFieldPanel<Vector4GFFNodeViewModel, Vector4ViewModel, Vector4EditedEventArgs>
{
    public required Vector4PanelViewModel ViewModel
    {
        get => (DataContext as Vector4PanelViewModel)!;
        set => DataContext = value;
    }


    public FieldVector4Panel() : base()
    {
        InitializeComponent();
    }

    protected override void RaiseFinishedEditing()
    {
        RoutedEventArgs args = new Vector4EditedEventArgs(FinishedEditingEvent, this, ViewModel.SourcePath, ViewModel.Value);
        RaiseEvent(args);
    }

    protected override Vector4ViewModel GetCurrentValue()
    {
        return SourceNode?.FieldValue.Clone() ?? new();
    }

    private void NumericUpDown_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RaiseFinishedEditing();
    }
}
