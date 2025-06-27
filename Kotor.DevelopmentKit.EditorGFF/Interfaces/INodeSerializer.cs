using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

namespace Kotor.DevelopmentKit.EditorGFF.Interfaces;

public interface INodeSerializer
{
    string Serialize(BaseGFFNode node);
}
