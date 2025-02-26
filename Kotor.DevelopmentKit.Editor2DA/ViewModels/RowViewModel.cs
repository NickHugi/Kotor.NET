using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Editor2DA.ViewModels;

public class RowViewModel : ReactiveObject
{
    private string _rowHeader = "";
    public string RowHeader
    {
        get => _rowHeader;
        set => this.RaiseAndSetIfChanged(ref _rowHeader, value);
    }

    private readonly Dictionary<string, string> _cells = new();
    public Dictionary<string, string> Cells
    {
        get => _cells;
        init => _cells = value;
    }
}
