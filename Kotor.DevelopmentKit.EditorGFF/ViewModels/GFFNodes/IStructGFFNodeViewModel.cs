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

public interface IStructGFFNodeViewModel : IGFFNodeViewModel
{
    public int StructID { get; set; }
    public bool IsDeleted { get; }

    public T AddField<T>(T field) where T : BaseFieldGFFNodeViewModel;
    public void DeleteField(BaseFieldGFFNodeViewModel field);
    public BaseFieldGFFNodeViewModel? GetField(string label);
}

public static class IStructGFFTreeNodeViewModelExtensions
{
    internal static void PopulateStruct(this IStructGFFNodeViewModel self, GFFStruct gffStruct)
    {
        if (self is BaseGFFNodeViewModel node)
        {
            self.StructID = (int)gffStruct.ID; // TODO

            foreach (var (label, value) in gffStruct.GetFields())
            {
                BaseFieldGFFNodeViewModel vmNode = value switch
                {
                    Byte asUInt8 => new UInt8GFFNodeViewModel(node, label, asUInt8),
                    SByte asInt8 => new Int8GFFNodeViewModel(node, label, asInt8),
                    UInt16 asUInt16 => new UInt16GFFNodeViewModel(node, label, asUInt16),
                    Int16 asInt16 => new Int16GFFNodeViewModel(node, label, asInt16),
                    UInt32 asUInt32 => new UInt32GFFNodeViewModel(node, label, asUInt32),
                    Int32 asInt32 => new Int32GFFNodeViewModel(node, label, asInt32),
                    UInt64 asUInt64 => new UInt64GFFNodeViewModel(node, label, asUInt64),
                    Int64 asInt64 => new Int64GFFNodeViewModel(node, label, asInt64),
                    Single asSingle => new SingleGFFNodeViewModel(node, label, asSingle),
                    Double asDouble => new DoubleGFFNodeViewModel(node, label, asDouble),
                    ResRef asResRef => new ResRefGFFNodeViewModel(node, label, asResRef),
                    String asString => new StringGFFNodeViewModel(node, label, asString),
                    LocalisedString asLocalizedString => new LocalizedStringGFFNodeViewModel(node, label, asLocalizedString),
                    byte[] asBinary => new BinaryGFFNodeViewModel(node, label, asBinary),
                    Vector3 asVector3 => new Vector3GFFNodeViewModel(node, label, asVector3),
                    Vector4 asVector4 => new Vector4GFFNodeViewModel(node, label, asVector4),
                    GFFStruct asStruct => new FieldStructGFFNodeViewModel(node, label, (int)asStruct.ID), // TODO standardize as either int or uint
                    GFFList asList => new ListGFFNodeViewModel(node, label),
                };

                if (vmNode is IStructGFFNodeViewModel thisStructField)
                {
                    thisStructField.PopulateStruct(value as GFFStruct);
                }
                if (vmNode is ListGFFNodeViewModel vmListField)
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
