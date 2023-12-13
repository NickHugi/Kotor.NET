using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorGL.SceneObjects
{
    public class CubeObject : SceneObject
    {
        public override List<IRenderable> GetRenderables(Graphics graphics)
        {
            return new();
        }

        public static void InitializeVertexArray(Graphics graphics)
        {
            
        }
    }
}
