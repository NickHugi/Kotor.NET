using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;

namespace Kotor.DevelopmentKit.Base.Views;

public class Conditional : TemplatedControl
{
    private Control _control;
    public Control Control => _control;

    public object DeferredContent { get; set; }

    public static readonly StyledProperty<bool> LoadProperty = AvaloniaProperty.Register<Conditional, bool>("Load");

    public bool Load
    {
        get => GetValue(LoadProperty);
        set => SetValue(LoadProperty, value);
    }

    static Conditional()
    {
        LoadProperty.Changed.AddClassHandler<Conditional>((c, e) =>
        {
            if (e.NewValue is bool v)
                c.DoLoad(v);
        });
    }

    public Conditional()
    {
    }

    public static readonly StyledProperty<Control?> ChildContentProperty =
        AvaloniaProperty.Register<Conditional, Control?>(nameof(ChildContent));

    [Content]
    public Control? ChildContent
    {
        get => GetValue(ChildContentProperty);
        set => SetValue(ChildContentProperty, value);
    }

    private void DoLoad(bool load)
    {
        if (ChildContent == null)
            return;
        if (load && ChildContent.Parent == this)
            return;

        if (load)
        {
            ((ISetLogicalParent)ChildContent).SetParent(this);
            VisualChildren.Add(ChildContent);
            LogicalChildren.Add(ChildContent);
        }
        else
        {
            ((ISetLogicalParent)ChildContent).SetParent(null);
            VisualChildren.Clear();
            LogicalChildren.Remove(ChildContent);
        }
        InvalidateMeasure();
    }
}
