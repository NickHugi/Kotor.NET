using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Interfaces;

public interface INodeSerializer
{
    string Serialize(BaseGFFNodeViewModel node);
}
