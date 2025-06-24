using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Kotor.DevelopmentKit.Base.ViewModels;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base;

public partial class SettingsDialog : ReactiveWindow<SettingsDialogViewModel>
{
    public SettingsDialog()
    {
        InitializeComponent();


        this.WhenAnyValue(x => x.ViewModel)
            .WhereNotNull()
            .Subscribe(x =>
            {
                x.CloseInteraction.RegisterHandler(interaction =>
                {
                    Close();
                    interaction.SetOutput(Unit.Default);
                });

            });
    }
}
