using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;

namespace Kotor.DevelopmentKit.Base.Controls;

public partial class LabeledCheckbox : UserControl
{
    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<LabeledTextBox, string>(nameof(Label));

    public static readonly StyledProperty<bool> CheckedProperty =
        AvaloniaProperty.Register<LabeledTextBox, bool>(nameof(Checked), defaultBindingMode: BindingMode.TwoWay);

    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public bool Checked
    {
        get => GetValue(CheckedProperty);
        set => SetValue(CheckedProperty, value);
    }

    public LabeledCheckbox()
    {
        InitializeComponent();
    }
}
