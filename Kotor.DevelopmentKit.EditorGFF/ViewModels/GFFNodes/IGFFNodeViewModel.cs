using System.Collections.ObjectModel;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public interface IGFFNodeViewModel
{
    public string Label { get; set; }
    public bool CanEditLabel { get; }

    public string Type { get; }
    public string Value { get; }

    public IGFFNodeViewModel? Parent { get; }
    public ReadOnlyObservableCollection<BaseGFFNodeViewModel> Children { get; }

    public void Delete();
}
