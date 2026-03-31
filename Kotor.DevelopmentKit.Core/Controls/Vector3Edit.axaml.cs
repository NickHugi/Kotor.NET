using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.Core.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace Kotor.DevelopmentKit.Core;

public partial class Vector3Edit : UserControl
{
    public static readonly StyledProperty<string> Value =
        AvaloniaProperty.Register<LabeledTextBox, string>(nameof(ReactiveVector3));

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
