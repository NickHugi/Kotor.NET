using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class CeilingTabViewModel : ReactiveObject
{
    public ObservableCollection<CeilingItem> CeilingItems { get; }
    public CeilingItem? SelectedCeilingItem
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public CeilingTabViewModel()
    {
        CeilingItems = [];
    }
    public CeilingTabViewModel(Kit kit) : this()
    {
        CeilingItems = new ObservableCollection<CeilingItem>(kit.Ceilings.Select(x => new CeilingItem(x)));
    }
}
