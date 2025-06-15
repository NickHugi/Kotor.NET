using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;
using Kotor.NET.Resources.KotorGFF;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels;

public class GFFViewModel : ReactiveObject
{
    private RootStructGFFNodeViewModel _rootNode = default!;
    public RootStructGFFNodeViewModel RootNode
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
    private GFFStruct BuildStruct(IStructGFFNodeViewModel structNode)
    {
        var gffStruct = new GFFStruct((uint)structNode.StructID);

        foreach (var vmChild in (structNode as BaseGFFNodeViewModel).Children)
        {
            if (vmChild is UInt8GFFNodeViewModel asUInt8) gffStruct.SetUInt8(asUInt8.Label, asUInt8.FieldValue);
            if (vmChild is Int8GFFNodeViewModel asInt8) gffStruct.SetInt8(asInt8.Label, asInt8.FieldValue);
            if (vmChild is UInt16GFFNodeViewModel asUInt16) gffStruct.SetUInt16(asUInt16.Label, asUInt16.FieldValue);
            if (vmChild is Int16GFFNodeViewModel asInt16) gffStruct.SetInt16(asInt16.Label, asInt16.FieldValue);
            if (vmChild is UInt32GFFNodeViewModel asUInt32) gffStruct.SetUInt32(asUInt32.Label, asUInt32.FieldValue);
            if (vmChild is Int32GFFNodeViewModel asInt32) gffStruct.SetInt32(asInt32.Label, asInt32.FieldValue);
            if (vmChild is UInt64GFFNodeViewModel asUInt64) gffStruct.SetUInt64(asUInt64.Label, asUInt64.FieldValue);
            if (vmChild is Int64GFFNodeViewModel asInt64) gffStruct.SetInt64(asInt64.Label, asInt64.FieldValue);
            if (vmChild is SingleGFFNodeViewModel asSingle) gffStruct.SetSingle(asSingle.Label, asSingle.FieldValue);
            if (vmChild is DoubleGFFNodeViewModel asDouble) gffStruct.SetDouble(asDouble.Label, asDouble.FieldValue);
            if (vmChild is ResRefGFFNodeViewModel asResRef) gffStruct.SetResRef(asResRef.Label, asResRef.FieldValue.AsModel());
            if (vmChild is StringGFFNodeViewModel asString) gffStruct.SetString(asString.Label, asString.FieldValue);
            if (vmChild is LocalizedStringGFFNodeViewModel asLocalisedString) gffStruct.SetLocalisedString(asLocalisedString.Label, asLocalisedString.FieldValue.AsModel());
            if (vmChild is BinaryGFFNodeViewModel asBinary) gffStruct.SetBinary(asBinary.Label, asBinary.FieldValue);
            if (vmChild is Vector3GFFNodeViewModel asVector3) gffStruct.SetVector3(asVector3.Label, asVector3.FieldValue.AsModel());
            if (vmChild is Vector4GFFNodeViewModel asVector4) gffStruct.SetVector4(asVector4.Label, asVector4.FieldValue.AsModel());
            if (vmChild is ListGFFNodeViewModel asList) gffStruct.SetList(asList.Label, BuildList(asList));
            if (vmChild is FieldStructGFFNodeViewModel asStruct) gffStruct.SetStruct(asStruct.Label, BuildStruct(asStruct));
        }

        return gffStruct;
    }
    private GFFList BuildList(ListGFFNodeViewModel listNode)
    {
        var gffList = new GFFList();

        foreach (var vmChild in listNode.Children)
        {
            if (vmChild is IStructGFFNodeViewModel structNode)
            {
                gffList.Add(BuildStruct(structNode));
            }
        }

        return gffList;
    }
}
