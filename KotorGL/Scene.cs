﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using KotorGL.SceneObjects;
using Silk.NET.OpenGLES;

namespace KotorGL
{
    public class Scene
    {
        public Camera Camera = new();

        private List<SceneObject> _objects = new();
        private Graphics _graphics;
        private Frame _frame = new();
        private GL _gl;

        public Scene(GL gl, Graphics graphics)
        {
            _gl = gl;
            _graphics = graphics;
        }

        int VertexArrayObject;
        Shader shader;
        public void Init()
        {
            _gl.DebugMessageCallback(DebugMessageDelegate, IntPtr.Zero);
            _gl.Enable(EnableCap.DebugOutput);

            _graphics.Shaders.Add("kotor", new Shader(_gl, "kotor"));
            _graphics.Shaders.Add("test", new Shader(_gl, "test"));

            CubeObject.InitializeVertexArray(_gl, _graphics);
            TriangleObject.InitializeVertexArray(_gl, _graphics);
        }

        public void Render(uint width, uint height)
        {
            if (width != Camera.Width || height != Camera.Height)
            {
                Camera.Width = width;
                Camera.Height = height;
                _gl.Viewport(0, 0, width*2, height*2);
            }

            foreach (var sceneObject in _objects)
            {
                sceneObject.GetRenderables(_graphics).ForEach(renderable => _frame.Add(renderable));
            }

            _frame.RenderToView(_gl, _graphics, Camera);
        }

        public void AddObject(SceneObject sceneObject)
        {
            _objects.Add(sceneObject);
        }

        private static DebugProc DebugMessageDelegate = OnDebugMessage;
        private static void OnDebugMessage(GLEnum source, GLEnum type, int id, GLEnum severity, int length, nint pMessage, nint pUserParam)
        {
            string message = Marshal.PtrToStringAnsi(pMessage, length);

            if (type == GLEnum.DebugTypeError)
            {
                throw new Exception(message);
            }
        }
    }
}
