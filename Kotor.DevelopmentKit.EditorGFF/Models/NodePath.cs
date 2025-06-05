using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.EditorGFF.Models;

public class NodePath : IEnumerable<object>
{
    public string? Head => _path.First().ToString();
    public string? Tail => _path.Last().ToString();

    private readonly IEnumerable<object> _path;


    public NodePath(IEnumerable<object> path)
    {
        _path = path;
        ValidatePath();
    }
    public NodePath(string node)
    {
        _path = [node];
        ValidatePath();
    }

    public IEnumerator<object> GetEnumerator()
    {
        return _path.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return _path.GetEnumerator();
    }

    private void ValidatePath()
    {
        //if (_path.Count() == 0)
        //{
        //    throw new NotSupportedException();
        //}
        if (_path.Any(x => x.GetType() != typeof(string) && x.GetType() != typeof(int)))
        {
            throw new NotSupportedException();
        }
    }

    public static implicit operator NodePath(object[] path) => new(path);
}
