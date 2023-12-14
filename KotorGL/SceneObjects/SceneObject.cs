using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorGL.SceneObjects
{
    public abstract class SceneObject
    {
        public abstract List<IRenderable> GetRenderables(Graphics graphics);
    }
}
