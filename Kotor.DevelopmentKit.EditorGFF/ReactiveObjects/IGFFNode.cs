using System.Collections.ObjectModel;

namespace Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

public interface IGFFNode
{
    public string Label { get; set; }
    public bool CanEditLabel { get; }

    public string DisplayType { get; }
    public string DisplayValue { get; }

    public IGFFNode? Parent { get; }
    public ReadOnlyObservableCollection<BaseGFFNode> Children { get; }

    public void Delete();
}
