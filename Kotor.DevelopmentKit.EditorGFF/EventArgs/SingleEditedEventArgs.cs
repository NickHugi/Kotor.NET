using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class SingleEditedEventArgs : RoutedEventArgs
{
    public NodePath SourcePath { get; }
    public Single NewValue { get; }

    public SingleEditedEventArgs(RoutedEvent routedEvent, object source, NodePath sourcePath, Single newValue)
        : base(routedEvent, source)
    {
        SourcePath = sourcePath;
        NewValue = newValue;
    }
}
