using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Collections;
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

    private readonly AvaloniaDictionary<string, string> _cells = new();
    public AvaloniaDictionary<string, string> Cells
    {
        get => _cells;
        init => this.RaiseAndSetIfChanged(ref _cells, value);
    }
}
