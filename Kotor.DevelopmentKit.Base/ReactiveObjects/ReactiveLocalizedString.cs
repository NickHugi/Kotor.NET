using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Collections;
using DynamicData.Binding;
using Kotor.NET.Common.Data;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ReactiveObjects;

public class ReactiveLocalizedString : ReactiveObject
{
    private int _stringref;
    public int StringRef
    {
        get => _stringref;
        set => this.RaiseAndSetIfChanged(ref _stringref, value);
    }

    private AvaloniaList<ReactiveLocalizedSubString> _substrings = new();
    public AvaloniaList<ReactiveLocalizedSubString> SubStrings
    {
        get => _substrings;
        init => _substrings = value;
    }

    private ReactiveLocalizedSubString? _selectedSubstring;
    public ReactiveLocalizedSubString? SelectedSubstring
    {
        get => _selectedSubstring;
        set => this.RaiseAndSetIfChanged(ref _selectedSubstring, value);
    }

    public bool IsSubStringSelected => _selectedSubstring is not null;


    public ReactiveLocalizedString()
    {
        this.WhenPropertyChanged(x => x.SelectedSubstring).Subscribe(x => this.RaisePropertyChanged(nameof(IsSubStringSelected)));
    }

    public void AddSubString()
    {
        SubStrings.Add(new());
    }
    public void AddSubString(ReactiveLocalizedSubString substring)
    {
        SubStrings.Add(substring);
    }

    public void RemoveSelectedSubString()
    {
        if (SelectedSubstring is not null)
        {
            SubStrings.Remove(SelectedSubstring);
        }
    }

    public LocalisedString AsModel()
    {
        return new(SubStrings.Select(x => x.AsModel()))
        {
            StringRef = StringRef
        };
    }
    public ReactiveLocalizedString Clone()
    {
        return new()
        {
            StringRef = _stringref,
            SubStrings = [.. _substrings.Select(x => x.Clone())],
        };
    }
}
