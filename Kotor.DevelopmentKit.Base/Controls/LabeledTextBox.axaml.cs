using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;

namespace Kotor.DevelopmentKit.Base.Controls;

public partial class LabeledTextBox : UserControl
{
    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<LabeledTextBox, string>(nameof(Label));

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<LabeledTextBox, string>(nameof(Text), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<bool> IsReadOnlyProperty =
        AvaloniaProperty.Register<LabeledTextBox, bool>(nameof(IsReadOnly), defaultBindingMode: BindingMode.OneWay);

    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool IsReadOnly
    {
        get => GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public LabeledTextBox()
    {
        InitializeComponent();
    }
}
