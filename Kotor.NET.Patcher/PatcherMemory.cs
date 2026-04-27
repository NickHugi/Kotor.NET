namespace Kotor.NET.Patcher;

public class PatcherMemory()
{
    private Dictionary<string, object> _tokens { get; } = [];

    private void Set<T>(string token, T value)
    {
        _tokens.Add(token, value);
    }
      
    public T Get<T>(string token)
    {
        return (T)_tokens[token];
    }
}
