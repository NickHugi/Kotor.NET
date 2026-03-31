using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.Base.ReactiveObjects;

namespace Kotor.DevelopmentKit.Base.Controls;

public partial class QuaternionEdit : UserControl
{
    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<LabeledTextBox, string>(nameof(Label));

    public static readonly StyledProperty<ReactiveQuaternion> ValueProperty =
        AvaloniaProperty.Register<LabeledTextBox, ReactiveQuaternion>(nameof(ReactiveQuaternion));

    public ReactiveQuaternion Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public QuaternionEdit()
    {
        InitializeComponent();
    }
}
