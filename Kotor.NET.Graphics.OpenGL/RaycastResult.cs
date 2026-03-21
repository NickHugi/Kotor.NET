using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics.OpenGL;

public class RaycastResult<T>
{
    public T Result { get; }
    public float Distance { get; }

    public RaycastResult(T result, float distance)
    {
        Result = result;
        Distance = distance;
    }
}
