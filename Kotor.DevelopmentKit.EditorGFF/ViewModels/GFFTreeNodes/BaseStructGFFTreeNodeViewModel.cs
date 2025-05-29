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

public interface BaseStructGFFTreeNodeViewModel : IGFFTreeNodeViewModel
{
    public int StructID { get; set; }

    public T AddField<T>(T field) where T : IFieldGFFTreeNodeViewModel;
    public void DeleteField(IFieldGFFTreeNodeViewModel field);
    public IFieldGFFTreeNodeViewModel? GetField(string label);

    public abstract void Delete();
}

public static class BaseStructGFFTreeNodeViewModelExtensions
{
    internal static void PopulateStruct(this BaseStructGFFTreeNodeViewModel self, GFFStruct gffStruct)
    {
        if (self is BaseGFFTreeNodeViewModel node)
        {
            self.StructID = (int)gffStruct.ID; // TODO

            foreach (var (label, value) in gffStruct.GetFields())
            {
                IFieldGFFTreeNodeViewModel vmNode = value switch
                {
                    Byte asUInt8 => new UInt8GFFTreeNodeViewModel(node, label, asUInt8),
                    SByte asInt8 => new Int8GFFTreeNodeViewModel(node, label, asInt8),
                    UInt16 asUInt16 => new UInt16GFFTreeNodeViewModel(node, label, asUInt16),
                    Int16 asInt16 => new Int16GFFTreeNodeViewModel(node, label, asInt16),
                    UInt32 asUInt32 => new UInt32GFFTreeNodeViewModel(node, label, asUInt32),
                    Int32 asInt32 => new Int32GFFTreeNodeViewModel(node, label, asInt32),
                    UInt64 asUInt64 => new UInt64GFFTreeNodeViewModel(node, label, asUInt64),
                    Int64 asInt64 => new Int64GFFTreeNodeViewModel(node, label, asInt64),
                    Single asSingle => new SingleGFFTreeNodeViewModel(node, label, asSingle),
                    Double asDouble => new DoubleGFFTreeNodeViewModel(node, label, asDouble),
                    ResRef asResRef => new ResRefGFFTreeNodeViewModel(node, label, asResRef),
                    String asString => new StringGFFTreeNodeViewModel(node, label, asString),
                    LocalisedString asLocalizedString => new LocalizedStringGFFTreeNodeViewModel(node, label, asLocalizedString),
                    byte[] asBinary => new BinaryGFFTreeNodeViewModel(node, label, asBinary),
                    Vector3 asVector3 => new Vector3GFFTreeNodeViewModel(node, label, asVector3),
                    Vector4 asVector4 => new Vector4GFFTreeNodeViewModel(node, label, asVector4),
                    GFFStruct asStruct => new StructGFFTreeNodeViewModel(node, label, (int)asStruct.ID), // TODO standardize as either int or uint
                    GFFList asList => new ListGFFTreeNodeViewModel(node, label),
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

                self.AddField(vmNode);
            }
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
}
