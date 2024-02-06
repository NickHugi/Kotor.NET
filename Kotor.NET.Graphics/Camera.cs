using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Maths;

namespace Kotor.NET.Graphics
{
    public class Camera
    {
        public float TargetX { get; set; } = 0.0f;
        public float TargetY { get; set; } = 0.0f;
        public float TargetZ { get; set; } = 0.0f;
        public uint Width { get; set; } = 400;
        public uint Height { get; set; } = 400;
        public float Distance { get; set; } = 10;
        public float Pitch { get; set; } = (float)(Math.PI / 4);
        public float Yaw { get; set; }
        public float Near { get; set; } = 0.001f;
        public float Far { get; set; } = 1000;
        public float FieldOfView { get; set; }
        public float AspectRatio => (float)Width / (float)Height;

        public Matrix4x4 GetView()
        {
            var target = new Vector3(TargetX, TargetY, TargetZ);

            var xzLen = Math.Cos(Yaw);
            var angle = new Vector3((float)(xzLen * Math.Cos(-Pitch)), (float)Math.Sin(Yaw), (float)(xzLen * Math.Sin(Pitch)));

            var view = Matrix4x4.CreateLookAt(Vector3.Normalize(angle) * Distance, target, new(0, 0, 1));

            // need some kind of matrix to change world to be z up
            //var view = Matrix4x4.CreateTranslation(X, Y, Z);
            //view = view * Matrix4x4.CreateFromYawPitchRoll(Yaw, Pitch, 0);

            return view; 
        }

        public Matrix4x4 GetProjection()
        {
            return Matrix4x4.CreatePerspectiveFieldOfView(1.39f, AspectRatio, Near, Far);
        }
    }
}
