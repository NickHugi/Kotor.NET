using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics;

public interface IRenderFrame
{
    void AddObject(IRenderObject renderObject);
    public void Render();
}
