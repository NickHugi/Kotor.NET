using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.OpenGL;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.ViewModels;

public class AreaDesignerViewModel : ReactiveObject
{
    public GLEngine Engine { get; set => this.RaiseAndSetIfChanged(ref field, value); }
}
