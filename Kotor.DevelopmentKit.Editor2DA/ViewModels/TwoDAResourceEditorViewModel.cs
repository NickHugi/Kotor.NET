using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using DynamicData;
using Kotor.DevelopmentKit.Base.Common;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.Editor2DA.Actions;
using Kotor.NET.Encapsulations;
using Kotor.NET.Formats.Binary2DA.Serialisation;
using Kotor.NET.Resources.Kotor2DA;
using Kotor.NET.Tests.Encapsulation;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Editor2DA.ViewModels;

public class TwoDAResourceEditorViewModel : BaseResourceEditorViewModel<TwoDAViewModel>
{
    public override string WindowTitlePrefix => "2DA Editor";

    private bool _showFilter;
    public bool ShowFilter
    {
        get => _showFilter;
        set => this.RaiseAndSetIfChanged(ref _showFilter, value);
    }

    public int SelectedRowIndex
    {
        get
        {
            return Resource.Rows.IndexOf(SelectedRow);
        }
        set
        {
            var row = Resource.Rows.ElementAt(value);
            SelectedRow = row;
        }
    }

    private int _selectedColumnIndex;
    public int SelectedColumnIndex
    {
        get => _selectedColumnIndex;
        set => this.RaiseAndSetIfChanged(ref _selectedColumnIndex, value);
    }

    private RowViewModel _selectedRow;
    public RowViewModel SelectedRow
    {
        get => _selectedRow;
        set => this.RaiseAndSetIfChanged(ref _selectedRow, value);
    }

    public List<List<string>> SelectedRows { get; }

    private readonly ActionHistory<TwoDAResourceEditorViewModel> _history;
    public ActionHistory<TwoDAResourceEditorViewModel> History
    {
        get => _history;
    }


    public TwoDAResourceEditorViewModel() : base(null)
    {
        _history = new(this);
        Resource = new();
    }

    public override void NewFile()
    {
        FilePath = null;
        LoadModel(new());
    }

    protected override void DeserializeAndLoad(byte[] data)
    {
        var model = TwoDA.FromBytes(data);
        LoadModel(model);
    }
    protected override void DeserializeAndLoad(string filepath)
    {
        var model = TwoDA.FromFile(filepath);
        LoadModel(model);
    }

    public override byte[] SerializeModelToBytes()
    {
        var twoda = Resource.Build();
        using var memoryStream = new MemoryStream();
        new TwoDABinarySerializer(twoda).Serialize().Write(memoryStream);
        return memoryStream.ToArray();
    }
    public override void SerializeModelToFile()
    {
        var twoda = Resource.Build();
        using var fileStream = File.OpenWrite(FilePath);
        new TwoDABinarySerializer(twoda).Serialize().Write(fileStream);
    }

    public void LoadModel(TwoDA model)
    {
        Resource.Filter = "";
        Resource.Load(model);
    }

    public void ToggleFilter()
    {
        ShowFilter = !ShowFilter;
    }

    public void Undo()
    {
        _history.Undo();
    }

    public void Redo()
    {
        _history.Redo();
    }

    public void SetRowCell(int rowID, string columnHeader, string newValue)
    {
        var oldValue = Resource.GetRowCell(rowID, columnHeader);
        var action = new EditRowCellAction(rowID, columnHeader, newValue, oldValue);
        _history.Apply(action);
    }
    public void SetRowCell(int rowID, string columnHeader, string newValue, string oldValue)
    {
        if (oldValue == newValue)
            return;

        var action = new EditRowCellAction(rowID, columnHeader, newValue, oldValue);
        History.Apply(action);
    }

    public void SetRowHeader(int rowID, string newValue)
    {
        var oldValue = Resource.GetRowHeader(rowID);
        var action = new EditRowHeaderAction(rowID, newValue, oldValue);
        History.Apply(action);
    }
    public void SetRowHeader(int rowID, string newValue, string oldValue)
    {
        if (oldValue == newValue)
            return;

        var action = new EditRowHeaderAction(rowID, newValue, oldValue);
        History.Apply(action);
    }

    public void AddColumn(string columnHeader)
    {
        var action = new AddColumnAction(columnHeader);
        History.Apply(action);
    }

    public void RenameColumn(string oldColumnHeader, string newColumnHeader)
    {
        var action = new RenameColumnAction(oldColumnHeader, newColumnHeader);
        History.Apply(action);
    }

    public void RemoveColumn(string columnHeader)
    {
        var columnIndex = Resource.GetColumnIndex(columnHeader);
        var cells = Resource.Rows.Select(x => x.Cells.TryGetValue(columnHeader, out var cells) ? cells : throw new Exception());
        var action = new RemoveColumnAction(columnHeader, columnIndex, cells);
        History.Apply(action);
    }

    public void AddRow()
    {
        var action = new AddRowAction();
        History.Apply(action);
    }

    public void ResetRowHeaders()
    {
        var action = new ResetRowHeadersAction(Resource.Rows.Select(x => x.RowHeader));
        History.Apply(action);
    }
}
