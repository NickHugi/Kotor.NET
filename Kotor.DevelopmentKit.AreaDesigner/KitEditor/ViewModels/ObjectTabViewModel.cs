using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class ObjectTabViewModel : ReactiveObject
{
    public ObservableCollection<ObjectItem> ObjectItems { get; }
    public ObjectItem? SelectedObjectItem
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public ObjectTabViewModel()
    {
        ObjectItems = [];
    }
    public ObjectTabViewModel(Kit kit) : this()
    {
        ObjectItems = new ObservableCollection<ObjectItem>(kit.Objects.Select(x => new ObjectItem(x)));
    }
}
