using System.Collections.Generic;
using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kotor.NET.Common;

namespace Kotor.DevelopmentKit.Base.Views;

public partial class GenderComboBox : UserControl
{
    public static readonly StyledProperty<Gender> SelectedGenderProperty =
        AvaloniaProperty.Register<LanguageComboBox, Gender>(nameof(SelectedGender), defaultValue: Gender.Male);

    public Gender SelectedGender
    {
        get => GetValue(SelectedGenderProperty);
        set => SetValue(SelectedGenderProperty, value);
    }

    public ICollection<Gender> Genders
    {
        get => Enum.GetValues<Gender>();
    }

    public GenderComboBox()
    {
        InitializeComponent();
    }
}
