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

public interface IStructGFFTreeNodeViewModel : IGFFNodeViewModel
{
    public int StructID { get; set; }

    public T AddField<T>(T field) where T : IFieldGFFNodeViewModel;
    public void DeleteField(IFieldGFFNodeViewModel field);
    public IFieldGFFNodeViewModel? GetField(string label);

    public abstract void Delete();
}

public static class IStructGFFTreeNodeViewModelExtensions
{
    internal static void PopulateStruct(this IStructGFFTreeNodeViewModel self, GFFStruct gffStruct)
    {
        if (self is BaseGFFNodeViewModel node)
        {
            self.StructID = (int)gffStruct.ID; // TODO

            foreach (var (label, value) in gffStruct.GetFields())
            {
                IFieldGFFNodeViewModel vmNode = value switch
                {
                    Byte asUInt8 => new FieldUInt8GFFNodeViewModel(node, label, asUInt8),
                    SByte asInt8 => new FieldInt8GFFNodeViewModel(node, label, asInt8),
                    UInt16 asUInt16 => new FieldUInt16GFFNodeViewModel(node, label, asUInt16),
                    Int16 asInt16 => new FieldInt16GFFNodeViewModel(node, label, asInt16),
                    UInt32 asUInt32 => new FieldUInt32GFFNodeViewModel(node, label, asUInt32),
                    Int32 asInt32 => new FieldInt32GFFNodeViewModel(node, label, asInt32),
                    UInt64 asUInt64 => new FieldUInt64GFFNodeViewModel(node, label, asUInt64),
                    Int64 asInt64 => new FieldInt64GFFNodeViewModel(node, label, asInt64),
                    Single asSingle => new FieldSingleGFFNodeViewModel(node, label, asSingle),
                    Double asDouble => new FieldDoubleGFFNodeViewModel(node, label, asDouble),
                    ResRef asResRef => new FieldResRefGFFNodeViewModel(node, label, asResRef),
                    String asString => new FieldStringGFFNodeViewModel(node, label, asString),
                    LocalisedString asLocalizedString => new FieldLocalizedStringGFFNodeViewModel(node, label, asLocalizedString),
                    byte[] asBinary => new FieldBinaryGFFNodeViewModel(node, label, asBinary),
                    Vector3 asVector3 => new FieldVector3GFFNodeViewModel(node, label, asVector3),
                    Vector4 asVector4 => new FieldVector4GFFNodeViewModel(node, label, asVector4),
                    GFFStruct asStruct => new FieldStructGFFNodeViewModel(node, label, (int)asStruct.ID), // TODO standardize as either int or uint
                    GFFList asList => new FieldListGFFNodeViewModel(node, label),
                };

                if (vmNode is IStructGFFTreeNodeViewModel thisStructField)
                {
                    thisStructField.PopulateStruct(value as GFFStruct);
                }
                if (vmNode is FieldListGFFNodeViewModel vmListField)
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
