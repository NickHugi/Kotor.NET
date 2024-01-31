using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Maths;

namespace KotorGL
{
    public class Camera
    {
        public float X { get; set; } = 0.0f;
        public float Y { get; set; } = 0.0f;
        public float Z { get; set; } = -2.0f;
        public uint Width { get; set; } = 400;
        public uint Height { get; set; } = 400;
        public float Pitch { get; set; }
        public float Yaw { get; set; }
        public float Near { get; set; } = 0.001f;
        public float Far { get; set; } = 1000;
        public float FieldOfView { get; set; }
        public float AspectRatio => (float)Width / (float)Height;

        public Matrix4x4 GetView()
        {
            var view = Matrix4x4.CreateTranslation(X, Y, Z);
            //var view = Matrix4x4.CreateLookAt(new(0, 2, 2), new(0, 0, 0), new(0, 0, 1));
            //view = view * Matrix4x4.CreateFromYawPitchRoll(Yaw, Pitch, 0);
            //Matrix4x4.Invert(view, out view);
            return view; 
        }

        public Matrix4x4 GetProjection()
        {
            //Matrix4X4.CreatePersp
            return Matrix4x4.CreatePerspectiveFieldOfView(1.39f, AspectRatio, Near, Far);
            //return Matrix4x4.CreateTranslation(0, 0, 0) * Matrix4x4.CreatePerspectiveFieldOfView(1.39f, AspectRatio, Near, Far);
        }
    }
}
