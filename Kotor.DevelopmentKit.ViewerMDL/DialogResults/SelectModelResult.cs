using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Tests.Encapsulation;

namespace Kotor.DevelopmentKit.ViewerMDL.DialogResults;

public class SelectModelResult(ResourceInfo mdl, ResourceInfo mdx)
{
    public ResourceInfo MDL { get; } = mdl;
    public ResourceInfo MDX { get; } = mdx;
}
