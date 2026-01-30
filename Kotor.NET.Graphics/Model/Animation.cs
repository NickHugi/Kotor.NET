using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics.Model;

public class Animation
{
    public string Name { get; set; }
    public float Length { get; set; }
    public float Transition { get; set; }
    public BaseNode Root { get; set; }
}
