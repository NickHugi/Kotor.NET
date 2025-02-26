using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Editor2DA.ViewModels;

namespace Kotor.DevelopmentKit.Editor2DA.Actions;

public class EditRowHeaderAction : IAction<TwoDAResourceEditorViewModel>
{
    public int RowID { get; }
    public string NewValue { get; }
    public string OldValue { get; }

    public EditRowHeaderAction(int rowID, string newValue, string oldValue)
    {
        RowID = rowID;
        NewValue = newValue;
        OldValue = oldValue;
    }

    public void Apply(TwoDAResourceEditorViewModel data)
    {
        data.Resource.SetRowHeader(RowID, NewValue);
        data.SelectedColumnIndex = 0;
        var rowIndex = data.Resource.GetRowIndex(RowID);
        data.SelectedRowIndex = rowIndex;
    }

    public void Undo(TwoDAResourceEditorViewModel data)
    {
        data.Resource.SetRowHeader(RowID, OldValue);
        data.SelectedColumnIndex = 0;
        data.SelectedRowIndex = RowID;
    }
}
