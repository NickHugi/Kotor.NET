using System;
using System.IO;
using System.Linq;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DynamicData;
using Kotor.DevelopmentKit.Base.DialogResults;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.NET.Common.Data;
using Kotor.NET.Encapsulations;
using Kotor.NET.Resources.KotorRIM;
using Kotor.NET.Tests.Encapsulation;

namespace Kotor.DevelopmentKit.Base.Windows;

public partial class EncapsulatedResourcePickerDialog : Window
{
    public LoadFromERFWindowViewModel Context => (LoadFromERFWindowViewModel)DataContext!;

    public EncapsulatedResourcePickerDialog()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        Context.ExceptionEvent.RegisterHandler(async interaction =>
        {
            await ExceptionDialog.ShowDilaog(this, interaction.Input);
            Close();
            interaction.SetOutput(Unit.Default);
        });
    }

    private void Cancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(null);
    }

    private void Load_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (Context.ResourceList.SelectedItem is null)
            return;

        Close(new LoadFromERFWindowDialogResult
        {
            FilePath = ResourceType.KEY.IsFileSameType(Context.FilePath) ? Context.FilePath : Context.ResourceList.SelectedItem.Filepath,
            ResRef = Context.ResourceList.SelectedItem.ResRef,
            ResourceType = Context.ResourceList.SelectedItem.Type,
        });
    }
}
