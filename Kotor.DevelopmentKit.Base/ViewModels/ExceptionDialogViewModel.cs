using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.ViewModels;

public class ExceptionDialogViewModel : ReactiveObject
{
    private Exception _exception = default!;
    public required Exception Exception
    {
        get => _exception;
        init => this.RaiseAndSetIfChanged(ref _exception, value);
    }

    private string _message = default!;
    public required string Message
    {
        get => _message;
        init => this.RaiseAndSetIfChanged(ref _message, value);
    }
}
