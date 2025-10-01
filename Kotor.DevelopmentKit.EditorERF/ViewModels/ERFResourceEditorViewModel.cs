using System;
using System.Reactive;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.NET.Common.Data;
using Kotor.NET.Encapsulations;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorERF.ViewModels;

public class ERFResourceEditorViewModel : BaseResourceEditorViewModel<ERFViewModel>
{
    public override string WindowTitlePrefix => "ERF Editor";

    public ActionHistory<ERFResourceEditorViewModel> History
    {
        get => field;
    }


    public ERFResourceEditorViewModel() : base(null)
    {

    }

    public override void NewFile() => throw new NotImplementedException();
    public override byte[] SerializeModelToBytes() => throw new NotImplementedException();
    public override void SerializeModelToFile() => throw new NotImplementedException();
    protected override void DeserializeAndLoad(byte[] data) => throw new NotImplementedException();
    protected override void DeserializeAndLoad(string filepath) => throw new NotImplementedException();
}
