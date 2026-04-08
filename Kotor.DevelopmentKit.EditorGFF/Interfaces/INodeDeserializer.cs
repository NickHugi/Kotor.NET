using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

namespace Kotor.DevelopmentKit.EditorGFF.Interfaces;

public interface INodeDeserializer
{
    BaseGFFNode Deserialize(BaseGFFNode hook, string text);
}
