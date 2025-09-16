using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorERF.ReactiveObjects;

public class ResourceItem : ReactiveObject
{
    public required string? ResRef
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public required ResourceType ResourceType
    {
        get;
        set
        {
            this.RaiseAndSetIfChanged(ref field, value);
        }
    }

    public required byte[] Data
    {
        get;
        set
        {
            this.RaiseAndSetIfChanged(ref field, value);
        }
    }
}
