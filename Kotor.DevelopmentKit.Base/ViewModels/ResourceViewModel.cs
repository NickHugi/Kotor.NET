using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ViewModels;

public class ResourceViewModel : ReactiveObject
{
    private string _filepath = default!;
    public required string Filepath
    {
        get => _filepath;
        set => this.RaiseAndSetIfChanged(ref _filepath, value);
    }

    private string _resref = default!;
    public required string ResRef
    {
        get => _resref;
        set => this.RaiseAndSetIfChanged(ref _resref, value);
    }

    private ResourceType _type = default!;
    public required ResourceType Type
    {
        get => _type;
        set => this.RaiseAndSetIfChanged(ref _type, value);
    }
}
