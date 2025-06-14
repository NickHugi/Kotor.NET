using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData.Binding;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.NET.Common.Localization;
using Kotor.NET.Interfaces;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.FieldPanel;

public class UInt64PanelViewModel : BaseNodePanelViewModel<UInt64>
{
    public UInt64PanelViewModel(NodePath path, UInt64 value)
    {
        SourcePath = path;
        Value = value;
    }
}
