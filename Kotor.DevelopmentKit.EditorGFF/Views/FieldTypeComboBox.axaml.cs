using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldTypeComboBox : UserControl
{
    public static readonly StyledProperty<string> FieldTypeProperty =
        AvaloniaProperty.Register<FieldTypeComboBox, string>(nameof(FieldType), defaultValue: "");

    public string FieldType
    {
        get => GetValue(FieldTypeProperty);
        set => SetValue(FieldTypeProperty, value);
    }

    public FieldTypeComboBox()
    {
        InitializeComponent();

        ComboBox.ItemsSource = new string[]
        {
            "UInt8",
            "Int8",
            "UInt16",
            "Int16",
            "UInt32",
            "Int32",
            "UInt64",
            "Int64",
            "Single",
            "Double",
            "ResRef",
            "String",
            "Localized String",
            "Binary",
            "Struct",
            "List"
        };
    }
}
