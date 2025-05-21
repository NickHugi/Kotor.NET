using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorGFF;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public abstract class BaseStructGFFTreeNodeViewModel : ReactiveObject, IGFFTreeNodeViewModel
{
    public abstract string Label { get; set; }
    public abstract bool CanEditLabel { get; }

    public int _structID;
    public int StructID
    {
        get => _structID;
        set => this.RaiseAndSetIfChanged(ref _structID, value);
    }
    
    private bool _expanded;
    public bool Expanded
    {
        get => _expanded;
        set => this.RaiseAndSetIfChanged(ref _expanded, value);
    }

    public string Type => "Struct";
    public string Value => StructID.ToString();
    
    public IGFFTreeNodeViewModel? Parent { get; }

    private ObservableCollection<IGFFTreeNodeViewModel> _children = new([]);
    public ReadOnlyObservableCollection<IGFFTreeNodeViewModel> Children => new(_children);


    public BaseStructGFFTreeNodeViewModel(IGFFTreeNodeViewModel? parent)
    {
        Parent = parent;
    }
    public BaseStructGFFTreeNodeViewModel(IGFFTreeNodeViewModel? parent, GFFStruct gffStruct) : this(parent)
    {
        PopulateStruct(gffStruct);
    }

    public T AddField<T>(T field) where T : IFieldGFFTreeNodeViewModel
    {
        _children.Add(field);
        Expanded = true;
        return field;
    }
    public void DeleteField(IFieldGFFTreeNodeViewModel field)
    {
        _children.Remove(field);
    }
    public IFieldGFFTreeNodeViewModel? GetField(string label)
    {
        return (IFieldGFFTreeNodeViewModel)Children.FirstOrDefault(x => x.Label == label);
    }

    public abstract void Delete();

    internal void PopulateStruct(GFFStruct gffStruct)
    {
        StructID = (int)gffStruct.ID; // TODO

        foreach (var (label, value) in gffStruct.GetFields())
        {
            IFieldGFFTreeNodeViewModel vmNode = value switch
            {
                Byte asUInt8 => new UInt8GFFTreeNodeViewModel(this, label, asUInt8),
                SByte asInt8 => new Int8GFFTreeNodeViewModel(this, label, asInt8),
                UInt16 asUInt16 => new UInt16GFFTreeNodeViewModel(this, label, asUInt16),
                Int16 asInt16 => new Int16GFFTreeNodeViewModel(this, label, asInt16),
                UInt32 asUInt32 => new UInt32GFFTreeNodeViewModel(this, label, asUInt32),
                Int32 asInt32 => new Int32GFFTreeNodeViewModel(this, label, asInt32),
                UInt64 asUInt64 => new UInt64GFFTreeNodeViewModel(this, label, asUInt64),
                Int64 asInt64 => new Int64GFFTreeNodeViewModel(this, label, asInt64),
                Single asSingle => new SingleGFFTreeNodeViewModel(this, label, asSingle),
                Double asDouble => new DoubleGFFTreeNodeViewModel(this, label, asDouble),
                ResRef asResRef => new ResRefGFFTreeNodeViewModel(this, label, asResRef),
                String asString => new StringGFFTreeNodeViewModel(this, label, asString),
                LocalisedString asLocalizedString => new LocalizedStringGFFTreeNodeViewModel(this, label, asLocalizedString),
                byte[] asBinary => new BinaryGFFTreeNodeViewModel(this, label, asBinary),
                Vector3 asVector3 => new Vector3GFFTreeNodeViewModel(this, label, asVector3),
                Vector4 asVector4 => new Vector4GFFTreeNodeViewModel(this, label, asVector4),
                GFFStruct asStruct => new StructGFFTreeNodeViewModel(this, label, (int)asStruct.ID), // TODO standardize as either int or uint
                GFFList asList => new ListGFFTreeNodeViewModel(this, label),
            };

            if (vmNode is BaseStructGFFTreeNodeViewModel thisStructField)
            {
                thisStructField.PopulateStruct(value as GFFStruct);
            }
            if (vmNode is ListGFFTreeNodeViewModel vmListField)
            {
                var list = value as GFFList;
                list.ToList().ForEach(x =>
                {
                    var vmChildStruct = vmListField.AddStruct();
                    vmChildStruct.PopulateStruct(x);
                });
            }

            this.AddField(vmNode);
        }
    }
}
