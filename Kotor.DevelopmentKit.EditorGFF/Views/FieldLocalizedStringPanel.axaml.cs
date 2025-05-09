using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;
using Kotor.NET.Common.Data;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldLocalizedStringPanel : UserControl
{
    public LocalizedStringGFFTreeNodeViewModel Context => (LocalizedStringGFFTreeNodeViewModel)DataContext!;

    public FieldLocalizedStringPanel()
    {
        InitializeComponent();
    }

    public void AddSubString()
    {
        Context.FieldValue.AddSubString();
    }
    public void RemoveSelectedSubString()
    {
        Context.FieldValue.RemoveSelectedSubString();
    }
}
