using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Kotor.DevelopmentKit.Base.Views;

public partial class IntegerUpDown : UserControl
{
    public static readonly StyledProperty<Int128> ValueProperty =
        AvaloniaProperty.Register<IntegerUpDown, Int128>(nameof(Value), defaultValue: 0);

    public Int128 Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public IntegerUpDown()
    {
        InitializeComponent();
        //IntegerTextBox.AddHandler(KeyDownEvent, IntegerTextBox_KeyDown, RoutingStrategies.Tunnel);
        IntegerTextBox.AddHandler(TextBox.TextInputEvent, TextBox_TextInput, RoutingStrategies.Tunnel);
    }

    private void TextBox_TextInput(object? sender, Avalonia.Input.TextInputEventArgs e)
    {
        if (e.Text is null || !e.Text.All(char.IsDigit))
        {
            IntegerTextBox.Text = Value.ToString();
        }

        //if (e.Text is null || !e.Text.All(char.IsDigit))
        //{
        //    e.Handled = false;
        //}
        //else
        //{
        //    Value = Int128.Parse(e.Text);
        //    e.Handled = true;
        //}
    }

    private void IntegerTextBox_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {

    }

    private void TextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {

    }
}
