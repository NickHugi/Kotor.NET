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

namespace Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;

public interface IStructGFFNode : IGFFNode
{
    public GFFStructID StructID { get; set; }
    public bool IsDeleted { get; }

    public T AddField<T>(T field) where T : BaseFieldGFFNode;
    public void DeleteField(BaseFieldGFFNode field);
    public BaseFieldGFFNode? GetField(string label);
}

public static class IStructGFFTreeNodeViewModelExtensions
{
    internal static void PopulateStruct(this IStructGFFNode self, GFFStruct gffStruct)
    {
        if (self is BaseGFFNode node)
        {
            self.StructID = gffStruct.ID;

            foreach (var (label, value) in gffStruct.GetFields())
            {
                BaseFieldGFFNode vmNode = value switch
                {
                    byte asUInt8 => new UInt8GFFNode(node, label, asUInt8),
                    sbyte asInt8 => new Int8GFFNode(node, label, asInt8),
                    ushort asUInt16 => new UInt16GFFNode(node, label, asUInt16),
                    short asInt16 => new Int16GFFNode(node, label, asInt16),
                    uint asUInt32 => new UInt32GFFNode(node, label, asUInt32),
                    int asInt32 => new Int32GFFNode(node, label, asInt32),
                    ulong asUInt64 => new UInt64GFFNode(node, label, asUInt64),
                    long asInt64 => new Int64GFFNode(node, label, asInt64),
                    float asSingle => new SingleGFFNode(node, label, asSingle),
                    double asDouble => new DoubleGFFNode(node, label, asDouble),
                    ResRef asResRef => new ResRefGFFNode(node, label, asResRef),
                    string asString => new StringGFFNode(node, label, asString),
                    LocalisedString asLocalizedString => new LocalizedStringGFFNode(node, label, asLocalizedString),
                    byte[] asBinary => new BinaryGFFNode(node, label, asBinary),
                    Vector3 asVector3 => new Vector3GFFNode(node, label, asVector3),
                    Vector4 asVector4 => new Vector4GFFNode(node, label, asVector4),
                    GFFStruct asStruct => new FieldStructGFFNode(node, label, (int)asStruct.ID), // TODO standardize as either int or uint
                    GFFList asList => new ListGFFNode(node, label),
                    _ => throw new InvalidOperationException()
                };

                if (vmNode is IStructGFFNode thisStructField)
                {
                    thisStructField.PopulateStruct(value as GFFStruct);
                }
                if (vmNode is ListGFFNode vmListField)
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
