using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class UInt64EditedEventArgs : RoutedEventArgs
{
    public NodePath SourcePath { get; }
    public UInt64 NewValue { get; }

    public UInt64EditedEventArgs(RoutedEvent routedEvent, object source, NodePath sourcePath, UInt64 newValue)
        : base(routedEvent, source)
    {
        SourcePath = sourcePath;
        NewValue = newValue;
    }
}
