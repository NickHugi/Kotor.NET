using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

namespace Kotor.DevelopmentKit.EditorGFF.EventArgs;

public class Vector3EditedEventArgs : RoutedEventArgs
{
    public NodePath SourcePath { get; }
    public Vector3ViewModel NewValue { get; }

    public Vector3EditedEventArgs(RoutedEvent routedEvent, object source, NodePath sourcePath, Vector3ViewModel newValue)
        : base(routedEvent, source)
    {
        SourcePath = sourcePath;
        NewValue = newValue.Clone();
    }
}
