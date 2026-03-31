using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.Base.ReactiveObjects;

namespace Kotor.DevelopmentKit.Base.Controls;

public partial class Vector3Edit : UserControl
{
    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<LabeledTextBox, string>(nameof(Label));

    public static readonly StyledProperty<ReactiveVector3> ValueProperty =
        AvaloniaProperty.Register<LabeledTextBox, ReactiveVector3>(nameof(ReactiveVector3));

    public ReactiveVector3 Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public Vector3Edit()
    {
        InitializeComponent();
    }
}
