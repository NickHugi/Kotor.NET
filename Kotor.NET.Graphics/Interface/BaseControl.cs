using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics.Interface;

public abstract class BaseControl
{
    public Scene Scene { get; internal set; }

    public virtual ICollection<ImageDescriptor> GetImageDescriptors(IAssetManager assets)
    {
        return [];
    }
}
