using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.FieldPanel;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldStringPanel : EditFieldPanel<StringGFFNode, String, StringEditedEventArgs>
{
    public required StringPanelViewModel ViewModel
    {
        get => (DataContext as StringPanelViewModel)!;
        set => DataContext = value;
    }


    public FieldStringPanel() : base()
    {
        InitializeComponent();
    }

    protected override void RaiseFinishedEditing()
    {
        RoutedEventArgs args = new StringEditedEventArgs(FinishedEditingEvent, this, ViewModel.Value);
        RaiseEvent(args);
    }

    protected override String GetCurrentValue()
    {
        return SourceNode?.FieldValue ?? "";
    }

    private void TextBox_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RaiseFinishedEditing();
    }
}
