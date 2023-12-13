using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorGL
{
    public interface IRenderable
    {
        public Shader Shader { get; }
        public Texture Texture1 { get; }
        public Texture Texture2 { get; }

        public void Render();
    }
}
