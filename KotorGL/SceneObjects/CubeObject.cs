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
            InitializeVertexArray();
            return new() { new Renderable(graphics.GetVAO(":cube"), graphics.GetShader("default"), null, null) };
        }

        public static void InitializeVertexArray(Graphics graphics)
        {
            if (graphics.)  
        }
    }
}
