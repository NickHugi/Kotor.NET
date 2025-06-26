using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.Interfaces;

public interface INodeDeserializer
{
    BaseGFFNodeViewModel Deserialize(BaseGFFNodeViewModel hook, string text);
}
