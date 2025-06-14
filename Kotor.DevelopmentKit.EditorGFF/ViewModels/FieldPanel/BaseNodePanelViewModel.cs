using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.EditorGFF.Models;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.FieldPanel;

public interface INodePanelViewModel
{
    public NodePath SourcePath { get; set; }
    public abstract object Value { set; }
}

public abstract class BaseNodePanelViewModel<TValue> : ReactiveObject, INodePanelViewModel
{
    private TValue _value;
    public TValue Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }

    object INodePanelViewModel.Value
    {
        set => Value = (TValue)value;
    }

    private NodePath _sourcePath;
    public NodePath SourcePath
    {
        get => _sourcePath;
        set => this.RaiseAndSetIfChanged(ref _sourcePath, value);
    }
}
