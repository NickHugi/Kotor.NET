using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ViewModels;

public class ResRefViewModel : ReactiveObject
{
    private string _value = "";
    public string Value
    {
        get => _value;
        set
        {
            if (value.Length > 16)
                value = value.Substring(0, 16);

            this.RaiseAndSetIfChanged(ref _value, value);
        }
    }

    public ResRef AsModel()
    {
        return new(Value);
    }
}
