using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class Int16EditedEventArgs : RoutedEventArgs
{
    public NodePath SourcePath { get; }
    public Int16 NewValue { get; }

    public Int16EditedEventArgs(RoutedEvent routedEvent, object source, NodePath sourcePath, Int16 newValue)
        : base(routedEvent, source)
    {
        SourcePath = sourcePath;
        NewValue = newValue;
    }
}
