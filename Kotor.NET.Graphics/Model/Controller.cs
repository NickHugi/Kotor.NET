using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics.Model;

public class Controller
{
    public int ControllerType { get; set; }
    public ICollection<ControllerDataRow> ControllerData { get; set; } = [];
}

public class ControllerDataRow
{
    public float TimeKey { get; set; }
    public float[] Values { get; set; } = [];
}
