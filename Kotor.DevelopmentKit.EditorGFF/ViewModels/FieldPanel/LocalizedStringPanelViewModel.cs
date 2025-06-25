using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData.Binding;
using Kotor.DevelopmentKit.Base.Settings;
using Kotor.DevelopmentKit.Base.Settings.Values;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.NET.Common.Localization;
using Kotor.NET.Interfaces;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.FieldPanel;

public class LocalizedStringPanelViewModel : BaseNodePanelViewModel<LocalizedStringViewModel>
{
    private TalkTableString _talkTableString;
    public TalkTableString TalkTableString
    {
        get => _talkTableString;
        private set => this.RaiseAndSetIfChanged(ref _talkTableString, value);
    }

    private bool _loadingTalkTableString;
    public bool LoadingTalkTableString
    {
        get => _loadingTalkTableString;
        private set => this.RaiseAndSetIfChanged(ref _loadingTalkTableString, value);
    }

    private InstallationSettings? _installation;
    public InstallationSettings? Installation
    {
        get => _installation;
        set => this.RaiseAndSetIfChanged(ref _installation, value);
    }

    private readonly ITalkTableLookup _talkTableLookup;


    public LocalizedStringPanelViewModel(ITalkTableLookup talkTableLookup, LocalizedStringViewModel locstring)
    {
        Value = locstring.Clone();
        _talkTableLookup = talkTableLookup;

        this.WhenAnyValue(x => x.Installation, x => x.Value.StringRef)
            .Subscribe(x =>
            {
                if (Installation is null)
                    return;

                LoadingTalkTableString = true;
                TalkTableString = _talkTableLookup.Locate(Path.Combine(Installation.Path, "dialog.tlk"), Value.StringRef);
                LoadingTalkTableString = false;
            });
    }
}
