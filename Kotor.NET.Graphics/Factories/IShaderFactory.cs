using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;

namespace Kotor.NET.Graphics.Factories;

public interface IShaderFactory
{
    public IShader FromSource(string vertexShader, string fragmentShader);
    public IShader FromFile(string vertexShader, string fragmentShader);
    public IShader FromStream(Stream vertexShader, Stream fragmentShader);
    public IShader FromEmbeddedResource(string vertexShader, string fragmentShader);
}
