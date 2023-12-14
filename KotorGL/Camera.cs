using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KotorGL
{
    public class Camera
    {
        public float X { get; set; } = 5;
        public float Y { get; set; }
        public float Z { get; set; }
        public int Width { get; set; } = 600;
        public int Height { get; set; } = 400;
        public float Pitch { get; set; }
        public float Yaw { get; set; }
        public float Near { get; set; } = 1;
        public float Far { get; set; } = 1000;
        public float FieldOfView { get; set; }

        public Matrix4x4 GetView()
        {
            var view = Matrix4x4.Identity * Matrix4x4.CreateTranslation(X, Y, Z);
            //view = view * Matrix4x4.CreateFromYawPitchRoll(Yaw, Pitch, 0);
            //Matrix4x4.Invert(view, out view);
            return view; 
        }

        public Matrix4x4 GetProjection()
        {
            return Matrix4x4.CreatePerspective(Width, Height, Near, Far);
        }
    }
}
