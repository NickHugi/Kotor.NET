using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DynamicData;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.Interfaces;
using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;
using Kotor.NET.Formats.BinaryGFF;

namespace Kotor.DevelopmentKit.EditorGFF.Services;

public class NodeDeserializer : INodeDeserializer
{
    public BaseGFFNode Deserialize(BaseGFFNode hook, string text)
    {
        var element = XElement.Parse(text);
        var node = Parse(hook, element);
        return node;
    }

    private BaseGFFNode Parse(BaseGFFNode hook, XElement element)
    {
        if (element.Name == "Field" && hook is IStructGFFNode structNode)
        {
            var label = element.Attribute("Label")!.Value;
            return element.Attribute("Type").Value switch
            {
                "UInt8" => new UInt8GFFNode(hook, label, Byte.Parse(element.Attribute("Value")!.Value)),
                "Int8" => new Int8GFFNode(hook, label, SByte.Parse(element.Attribute("Value")!.Value)),
                "UInt16" => new UInt16GFFNode(hook, label, UInt16.Parse(element.Attribute("Value")!.Value)),
                "Int16" => new Int16GFFNode(hook, label, Int16.Parse(element.Attribute("Value")!.Value)),
                "UInt32" => new UInt32GFFNode(hook, label, UInt32.Parse(element.Attribute("Value")!.Value)),
                "Int32" => new Int32GFFNode(hook, label, Int32.Parse(element.Attribute("Value")!.Value)),
                "UInt64" => new UInt64GFFNode(hook, label, UInt64.Parse(element.Attribute("Value")!.Value)),
                "Int64" => new Int64GFFNode(hook, label, Int64.Parse(element.Attribute("Value")!.Value)),
                "Single" => new SingleGFFNode(hook, label, Single.Parse(element.Attribute("Value")!.Value)),
                "Double" => new DoubleGFFNode(hook, label, Double.Parse(element.Attribute("Value")!.Value)),
                "ResRef" => new ResRefGFFNode(hook, label, element.Attribute("Value")!.Value),
                "String" => new StringGFFNode(hook, label, element.Attribute("Value")!.Value),
                "LocalizedString" => ParseLocalizedString(hook, label, element),
                "Vector3" => ParseVector3(hook, label, element),
                "Vector4" => ParseVector4(hook, label, element),
                "Binary" => ParseBinary(hook, label, element),
                "List" => ParseList(hook, label, element),
                "Struct" => ParseStruct(hook, label, element),
                _ => throw new NotSupportedException()
            };
        }
        else if (element.Name == "Struct" && hook is ListGFFNode listNode)
        {
            var listStructNode = new ListStructGFFNode(listNode);
            ParseStructInner(listStructNode, element);
            return listStructNode;
        }
        else if (element.Name == "Root")
        {
            throw new NotImplementedException();
        }
        else
        {
            throw new NotSupportedException();
        }
    }

    private LocalizedStringGFFNode ParseLocalizedString(BaseGFFNode hook, string label, XElement element)
    {
        var fieldNode = new LocalizedStringGFFNode(hook, label);
        foreach (var childElement in element.Descendants())
        {
            var substring = new ReactiveLocalizedSubString();
            fieldNode.FieldValue.AddSubString(substring);
        }
        return fieldNode;
    }

    private Vector3GFFNode ParseVector3(BaseGFFNode hook, string label, XElement element)
    {
        var x = float.Parse(element.Element("X")!.Attribute("Value")!.Value);
        var y = float.Parse(element.Element("Y")!.Attribute("Value")!.Value);
        var z = float.Parse(element.Element("Z")!.Attribute("Value")!.Value);
        return new Vector3GFFNode(hook, label, x, y, z);
    }

    private Vector4GFFNode ParseVector4(BaseGFFNode hook, string label, XElement element)
    {
        var x = float.Parse(element.Element("X")!.Attribute("Value")!.Value);
        var y = float.Parse(element.Element("Y")!.Attribute("Value")!.Value);
        var z = float.Parse(element.Element("Z")!.Attribute("Value")!.Value);
        var w = float.Parse(element.Element("W")!.Attribute("Value")!.Value);
        return new Vector4GFFNode(hook, label, x, y, z, w);
    }

    private BinaryGFFNode ParseBinary(BaseGFFNode hook, string label, XElement element)
    {
        var data = Convert.FromBase64String(element.Attribute("Value").Value);
        return new BinaryGFFNode(hook, label, data);
    }

    private ListGFFNode ParseList(BaseGFFNode hook, string label, XElement element)
    {
        var node = new ListGFFNode(hook, label);
        foreach (var childElement in element.Descendants())
        {
            var structNode = Parse(node, childElement);
            node.AddStruct((ListStructGFFNode)structNode);
        }
        return node;
    }

    private FieldStructGFFNode ParseStruct(BaseGFFNode hook, string label, XElement element)
    {
        var node = new FieldStructGFFNode(hook, label);
        ParseStructInner(node, element);
        return node;
    }

    private void ParseStructInner(BaseGFFNode structNode, XElement structElement)
    {
        if (structNode is IStructGFFNode structNodeInterface)
        {
            foreach (var fieldElement in structElement.Descendants())
            {
                if (Parse(structNode, fieldElement) is BaseFieldGFFNode fieldNode)
                {
                    structNodeInterface.AddField(fieldNode);
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }
        else
        {
            throw new NotSupportedException();
        }
    }
}
