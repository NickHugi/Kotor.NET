using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kotor.NET.Common;

namespace Kotor.DevelopmentKit.Base.Views;

public partial class LanguageComboBox : UserControl
{
    public static readonly StyledProperty<Language> SelectedLanguageProperty =
        AvaloniaProperty.Register<LanguageComboBox, Language>(nameof(SelectedLanguage), defaultValue: Language.English);

    public Language SelectedLanguage
    {
        get => GetValue(SelectedLanguageProperty);
        set => SetValue(SelectedLanguageProperty, value);
    }

    public ICollection<Language> Languages
    {
        get => Enum.GetValues<Language>();
    }

    public LanguageComboBox()
    {
        InitializeComponent();
    }
}
