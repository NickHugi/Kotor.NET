using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData.Binding;
using Kotor.NET.Common;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ViewModels;

public class LocalizedSubStringViewModel : ReactiveObject
{
    private Language _language;
    public Language Language
    {
        get => _language;
        set => this.RaiseAndSetIfChanged(ref _language, value);
    }

    private Gender _gender;
    public Gender Gender
    {
        get => _gender;
        set => this.RaiseAndSetIfChanged(ref _gender, value);
    }

    private string _text = "";
    public string Text
    {
        get => _text;
        set => this.RaiseAndSetIfChanged(ref _text, value);
    }

    public string Label => $"{Gender.ToString()} {Language.ToString()}";

    public LocalizedSubStringViewModel()
    {
        this.WhenPropertyChanged(x => x.Gender).Subscribe(x => this.RaisePropertyChanged(nameof(Label)));
        this.WhenPropertyChanged(x => x.Language).Subscribe(x => this.RaisePropertyChanged(nameof(Label)));
    }
}
