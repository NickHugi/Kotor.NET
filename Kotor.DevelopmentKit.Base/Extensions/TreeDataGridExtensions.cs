using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Selection;

namespace Kotor.DevelopmentKit.Base.Extensions;

public static class TreeDataGridExtensions
{
    public static readonly AttachedProperty<object?> SelectedItemProperty =
        AvaloniaProperty.RegisterAttached<TreeDataGrid, object?>(
            "SelectedItem",
            typeof(TreeDataGridExtensions),
            coerce: CoerceIsClickHandled
            );

    public static object? GetSelectedItem(TreeDataGrid control) =>
        control.RowSelection?.SelectedItem;

    public static void SetSelectedItem(TreeDataGrid control, object? value) =>
        control.RowSelection?.Clear();

    private static object? CoerceIsClickHandled(AvaloniaObject element, object? value)
    {
        if (element is TreeDataGrid tree)
        {
            tree.SelectionChanging -= OnSelectionChanging;
            tree.SelectionChanging += OnSelectionChanging;
        }

        return value;
    }

    private static void OnSelectionChanging(object? sender, CancelEventArgs args)
    {
        if (sender is TreeDataGrid tree && tree.RowSelection is not null)
        {
            EventHandler<TreeSelectionModelSelectionChangedEventArgs> onSelectionChanged = null!;
            onSelectionChanged = (sender, args) =>
            {
                tree.SetCurrentValue(SelectedItemProperty, args.SelectedItems.FirstOrDefault());
                tree.RowSelection.SelectionChanged -= onSelectionChanged;
            };
            tree.RowSelection.SelectionChanged += onSelectionChanged;
        }
    }
}
