using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ViewModels;

public class SaveFileAlternativesDialogViewModel : ReactiveObject
{
    private string _filepath = "";
    public string FilePath
    {
        get => _filepath;
        init => this.RaiseAndSetIfChanged(ref _filepath, value);
    }

    private string? _overrideFolder = null;
    public string? OverrideFolder
    {
        get => _overrideFolder;
        init => this.RaiseAndSetIfChanged(ref _overrideFolder, value);
    }

    public string Message
    {
        get => ResourceType.FromFilepath(_filepath) == ResourceType.KEY
            ? "Cannot override resources stored in KEY/BIF format."
            : "Cannot save resources to RIM format.";
    }

    public bool ShowOptionForMOD
    {
        get => ResourceType.FromFilepath(_filepath) == ResourceType.RIM;
    }

    public bool ShowOptionForOverride
    {
        get => _overrideFolder is not null;
    }
}
