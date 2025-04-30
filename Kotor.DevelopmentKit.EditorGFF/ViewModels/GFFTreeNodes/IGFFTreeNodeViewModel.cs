using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public interface IGFFTreeNodeViewModel
{
    public string Label { get; set; }
    public bool CanEditLabel { get; }

    public string Type { get; }
    public string Value { get; }
    public bool Expanded { get; set; }
    public IGFFTreeNodeViewModel Parent { get; }
    public ReadOnlyObservableCollection<IGFFTreeNodeViewModel> Children { get; }

    public void Delete();
}
