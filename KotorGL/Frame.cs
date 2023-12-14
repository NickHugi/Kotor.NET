using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorGL
{
    public class Frame
    {
        // list of things to renderss

        private IList<IRenderable> _renderables = new List<IRenderable>();

        public void Add(IRenderable renderable)
        {
            _renderables.Add(renderable);
        }

        public void RenderToView()
        {
            _renderables.Clear();
        }
    }
}
