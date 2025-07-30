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

public partial class EncapsulatedResourceSaverDialog : Window
{
    public EncapsulatedResourceSaverDialogViewModel Context => (EncapsulatedResourceSaverDialogViewModel)DataContext!;


    public EncapsulatedResourceSaverDialog()
    {
        InitializeComponent();
    }


    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        Context.ExceptionThrown.RegisterHandler(async interaction =>
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

    private void Save_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close(new SaveToERFWindowDialogResult
        {
            FilePath = Context.FilePath,
            ResRef = Context.ResRef,
            ResourceType = Context.ResourceType!
        });
    }
}
