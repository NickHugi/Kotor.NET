using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData.Binding;
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

    private readonly ITalkTableLookup _talkTableLookup;


    public LocalizedStringPanelViewModel(ITalkTableLookup talkTableLookup, LocalizedStringViewModel locstring, NodePath path)
    {
        SourcePath = path;
        Value = locstring.Clone();
        _talkTableLookup = talkTableLookup;

        this.WhenAnyValue(x => x.Value)
            .WhereNotNull()
            .Select(x => x.WhenAnyValue(y => y.StringRef))
            .Switch()
            .Subscribe(x =>
            {
                LoadingTalkTableString = true;
                TalkTableString = _talkTableLookup.Locate(@"C:\Program Files (x86)\Steam\steamapps\common\swkotor\dialog.tlk", Value.StringRef);
                LoadingTalkTableString = false;
            });
    }
}
