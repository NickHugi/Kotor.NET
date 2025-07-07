using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData.Binding;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.Models;
using Kotor.NET.Common.Data;
using Kotor.NET.Common.Localization;
using Kotor.NET.Interfaces;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.FieldPanel;

public class ResRefPanelViewModel : BaseNodePanelViewModel<ReactiveResRef>
{
    public ResRefPanelViewModel(ReactiveResRef value)
    {
        Value = value;
    }
}
