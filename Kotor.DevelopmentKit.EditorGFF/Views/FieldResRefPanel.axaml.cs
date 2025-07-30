using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.FieldPanel;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldResRefPanel : EditFieldPanel<ResRefGFFNode, ReactiveResRef, ResRefEditedEventArgs>
{
    public required ResRefPanelViewModel ViewModel
    {
        get => (DataContext as ResRefPanelViewModel)!;
        set => DataContext = value;
    }


    public FieldResRefPanel() : base()
    {
        InitializeComponent();
    }

    protected override void RaiseFinishedEditing()
    {
        RoutedEventArgs args = new ResRefEditedEventArgs(FinishedEditingEvent, this, ViewModel.Value);
        RaiseEvent(args);
    }

    protected override ReactiveResRef GetCurrentValue()
    {
        return SourceNode?.FieldValue.Clone() ?? new();
    }

    private void TextBox_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RaiseFinishedEditing();
    }
}
