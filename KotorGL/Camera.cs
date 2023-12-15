using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace KotorGL
{
    public class Camera
    {
        public float X { get; set; } = 0f;
        public float Y { get; set; } = 0f;
        public float Z { get; set; } = 1f;
        public int Width { get; set; } = 800;
        public int Height { get; set; } = 450;
        public float Pitch { get; set; }
        public float Yaw { get; set; }
        public float Near { get; set; } = 0.001f;
        public float Far { get; set; } = 1000;
        public float FieldOfView { get; set; }

        public Matrix4 GetView()
        {
            var view = Matrix4.Identity * Matrix4.CreateTranslation(X, Y, Z);
            //view = view * Matrix4x4.CreateFromYawPitchRoll(Yaw, Pitch, 0);
            //Matrix4x4.Invert(view, out view);
            return view; 
        }

        public Matrix4 GetProjection()
        {
            return Matrix4.CreatePerspectiveFieldOfView(1.39f, Width/Height, Near, Far);
        }
    }
}
