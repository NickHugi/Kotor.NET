using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Kotor.DevelopmentKit.Core.Controls;

public partial class LabeledTextBox : UserControl
{
    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<LabeledTextBox, string>(nameof(Label));

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<LabeledTextBox, string>(nameof(Text));

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

    public LabeledTextBox()
    {
        InitializeComponent();
    }
}
