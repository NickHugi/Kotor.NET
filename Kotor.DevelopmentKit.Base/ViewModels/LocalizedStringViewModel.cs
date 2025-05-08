using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Collections;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ViewModels;

public class LocalizedStringViewModel : ReactiveObject
{
    private int _stringref;
    public int StringRef
    {
        get => _stringref;
        set => this.RaiseAndSetIfChanged(ref _stringref, value);
    }

    private AvaloniaList<LocalizedSubStringViewModel> _substrings = new();
    public AvaloniaList<LocalizedSubStringViewModel> SubStrings
    {
        get => _substrings;
    }

    private LocalizedSubStringViewModel? _selectedSubstring;
    public LocalizedSubStringViewModel? SelectedSubstring
    {
        get => _selectedSubstring;
        set => this.RaiseAndSetIfChanged(ref _selectedSubstring, value);
    }

    public void AddSubString()
    {
        SubStrings.Add(new());
    }
    public void RemoveSelectedSubString()
    {
        if (SelectedSubstring is not null)
        {
            SubStrings.Remove(SelectedSubstring);
        }
    }
}
