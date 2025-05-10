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
    private RootStructGFFTreeNodeViewModel _rootNode = default!;
    public RootStructGFFTreeNodeViewModel RootNode
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
    private GFFStruct BuildStruct(BaseStructGFFTreeNodeViewModel vmStruct)
    {
        var gffStruct = new GFFStruct((uint)vmStruct.StructID); // TODO

        foreach (var vmChild in vmStruct.Children)
        {
            if (vmChild is UInt8GFFTreeNodeViewModel asUInt8) gffStruct.SetUInt8(asUInt8.Label, asUInt8.FieldValue);
            if (vmChild is Int8GFFTreeNodeViewModel asInt8) gffStruct.SetInt8(asInt8.Label, asInt8.FieldValue);
            if (vmChild is UInt16GFFTreeNodeViewModel asUInt16) gffStruct.SetUInt16(asUInt16.Label, asUInt16.FieldValue);
            if (vmChild is Int16GFFTreeNodeViewModel asInt16) gffStruct.SetInt16(asInt16.Label, asInt16.FieldValue);
            if (vmChild is UInt32GFFTreeNodeViewModel asUInt32) gffStruct.SetUInt32(asUInt32.Label, asUInt32.FieldValue);
            if (vmChild is Int32GFFTreeNodeViewModel asInt32) gffStruct.SetInt32(asInt32.Label, asInt32.FieldValue);
            if (vmChild is UInt64GFFTreeNodeViewModel asUInt64) gffStruct.SetUInt64(asUInt64.Label, asUInt64.FieldValue);
            if (vmChild is Int64GFFTreeNodeViewModel asInt64) gffStruct.SetInt64(asInt64.Label, asInt64.FieldValue);
            if (vmChild is SingleGFFTreeNodeViewModel asSingle) gffStruct.SetSingle(asSingle.Label, asSingle.FieldValue);
            if (vmChild is DoubleGFFTreeNodeViewModel asDouble) gffStruct.SetDouble(asDouble.Label, asDouble.FieldValue);
            if (vmChild is ResRefGFFTreeNodeViewModel asResRef) gffStruct.SetResRef(asResRef.Label, asResRef.FieldValue.AsModel());
            if (vmChild is StringGFFTreeNodeViewModel asString) gffStruct.SetString(asString.Label, asString.FieldValue);
            if (vmChild is LocalizedStringGFFTreeNodeViewModel asLocalisedString) gffStruct.SetLocalisedString(asLocalisedString.Label, asLocalisedString.FieldValue.AsModel());
            if (vmChild is BinaryGFFTreeNodeViewModel asBinary) gffStruct.SetBinary(asBinary.Label, asBinary.FieldValue);
            if (vmChild is Vector3GFFTreeNodeViewModel asVector3) gffStruct.SetVector3(asVector3.Label, asVector3.FieldValue.AsModel());
            if (vmChild is Vector4GFFTreeNodeViewModel asVector4) gffStruct.SetVector4(asVector4.Label, asVector4.FieldValue.AsModel());
            if (vmChild is ListGFFTreeNodeViewModel asList) gffStruct.SetList(asList.Label, BuildList(asList));
            if (vmChild is StructGFFTreeNodeViewModel asStruct) gffStruct.SetStruct(asStruct.Label, BuildStruct(asStruct));
        }

        return gffStruct;
    }
    private GFFList BuildList(ListGFFTreeNodeViewModel vmList)
    {
        var gffList = new GFFList();

        foreach (var vmChild in vmList.Children)
        {
            if (vmChild is BaseStructGFFTreeNodeViewModel vmStruct)
            {
                gffList.Add(BuildStruct(vmStruct));
            }
        }

        return gffList;
    }
}
