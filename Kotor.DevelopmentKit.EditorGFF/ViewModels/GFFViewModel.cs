using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;
using Kotor.NET.Resources.KotorGFF;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels;

public class GFFViewModel : ReactiveObject
{
    private RootStructGFFNode _rootNode = default!;
    public RootStructGFFNode RootNode
    {
        get => _rootNode;
        set => this.RaiseAndSetIfChanged(ref _rootNode, value);
    }


    public GFFViewModel()
    {
        RootNode = new();
    }

    public void Load(GFF gff)
    {
        RootNode = new(gff.Root);
    }

    public GFF Build()
    {
        return new GFF()
        {
            Root = BuildStruct(RootNode)
        };
    }
    private GFFStruct BuildStruct(IStructGFFNode structNode)
    {
        var gffStruct = new GFFStruct((uint)structNode.StructID);

        foreach (var vmChild in (structNode as BaseGFFNode).Children)
        {
            if (vmChild is UInt8GFFNode asUInt8) gffStruct.SetUInt8(asUInt8.Label, asUInt8.FieldValue);
            if (vmChild is Int8GFFNode asInt8) gffStruct.SetInt8(asInt8.Label, asInt8.FieldValue);
            if (vmChild is UInt16GFFNode asUInt16) gffStruct.SetUInt16(asUInt16.Label, asUInt16.FieldValue);
            if (vmChild is Int16GFFNode asInt16) gffStruct.SetInt16(asInt16.Label, asInt16.FieldValue);
            if (vmChild is UInt32GFFNode asUInt32) gffStruct.SetUInt32(asUInt32.Label, asUInt32.FieldValue);
            if (vmChild is Int32GFFNode asInt32) gffStruct.SetInt32(asInt32.Label, asInt32.FieldValue);
            if (vmChild is UInt64GFFNode asUInt64) gffStruct.SetUInt64(asUInt64.Label, asUInt64.FieldValue);
            if (vmChild is Int64GFFNode asInt64) gffStruct.SetInt64(asInt64.Label, asInt64.FieldValue);
            if (vmChild is SingleGFFNode asSingle) gffStruct.SetSingle(asSingle.Label, asSingle.FieldValue);
            if (vmChild is DoubleGFFNode asDouble) gffStruct.SetDouble(asDouble.Label, asDouble.FieldValue);
            if (vmChild is ResRefGFFNode asResRef) gffStruct.SetResRef(asResRef.Label, asResRef.FieldValue.AsModel());
            if (vmChild is StringGFFNode asString) gffStruct.SetString(asString.Label, asString.FieldValue);
            if (vmChild is LocalizedStringGFFNode asLocalisedString) gffStruct.SetLocalisedString(asLocalisedString.Label, asLocalisedString.FieldValue.AsModel());
            if (vmChild is BinaryGFFNode asBinary) gffStruct.SetBinary(asBinary.Label, asBinary.FieldValue);
            if (vmChild is Vector3GFFNode asVector3) gffStruct.SetVector3(asVector3.Label, asVector3.FieldValue.AsModel());
            if (vmChild is Vector4GFFNode asVector4) gffStruct.SetVector4(asVector4.Label, asVector4.FieldValue.AsModel());
            if (vmChild is ListGFFNode asList) gffStruct.SetList(asList.Label, BuildList(asList));
            if (vmChild is FieldStructGFFNode asStruct) gffStruct.SetStruct(asStruct.Label, BuildStruct(asStruct));
        }

        return gffStruct;
    }
    private GFFList BuildList(ListGFFNode listNode)
    {
        var gffList = new GFFList();

        foreach (var vmChild in listNode.Children)
        {
            if (vmChild is IStructGFFNode structNode)
            {
                gffList.Add(BuildStruct(structNode));
            }
        }

        return gffList;
    }
}
