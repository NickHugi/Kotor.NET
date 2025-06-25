using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DynamicData;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;
using Kotor.NET.Formats.BinaryGFF;

namespace Kotor.DevelopmentKit.EditorGFF.Services;

public class NodeDeserializer
{
    public BaseGFFNodeViewModel Deserialize(BaseGFFNodeViewModel hook, string text)
    {
        var element = XElement.Parse(text);
        var node = Parse(hook, element);
        return node;
    }

    private BaseGFFNodeViewModel Parse(BaseGFFNodeViewModel hook, XElement element)
    {
        if (element.Name == "Field" && hook is IStructGFFNodeViewModel structNode)
        {
            var label = element.Attribute("Label")!.Value;
            return element.Attribute("Type").Value switch
            {
                "UInt8" => new UInt8GFFNodeViewModel(hook, label, Byte.Parse(element.Attribute("Value")!.Value)),
                "Int8" => new Int8GFFNodeViewModel(hook, label, SByte.Parse(element.Attribute("Value")!.Value)),
                "UInt16" => new UInt16GFFNodeViewModel(hook, label, UInt16.Parse(element.Attribute("Value")!.Value)),
                "Int16" => new Int16GFFNodeViewModel(hook, label, Int16.Parse(element.Attribute("Value")!.Value)),
                "UInt32" => new UInt32GFFNodeViewModel(hook, label, UInt32.Parse(element.Attribute("Value")!.Value)),
                "Int32" => new Int32GFFNodeViewModel(hook, label, Int32.Parse(element.Attribute("Value")!.Value)),
                "UInt64" => new UInt64GFFNodeViewModel(hook, label, UInt64.Parse(element.Attribute("Value")!.Value)),
                "Int64" => new Int64GFFNodeViewModel(hook, label, Int64.Parse(element.Attribute("Value")!.Value)),
                "Single" => new SingleGFFNodeViewModel(hook, label, Single.Parse(element.Attribute("Value")!.Value)),
                "Double" => new DoubleGFFNodeViewModel(hook, label, Double.Parse(element.Attribute("Value")!.Value)),
                "ResRef" => new ResRefGFFNodeViewModel(hook, label, element.Attribute("Value")!.Value),
                "String" => new StringGFFNodeViewModel(hook, label, element.Attribute("Value")!.Value),
                "LocalizedString" => ParseLocalizedString(hook, label, element),
                "Vector3" => ParseVector3(hook, label, element),
                "Vector4" => ParseVector4(hook, label, element),
                "Binary" => ParseBinary(hook, label, element),
                "List" => ParseList(hook, label, element),
                "Struct" => ParseStruct(hook, label, element),
                _ => throw new NotSupportedException()
            };
        }
        else if (element.Name == "Struct" && hook is ListGFFNodeViewModel listNode)
        {
            var listStructNode = new ListStructGFFNodeViewModel(listNode);
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

    private LocalizedStringGFFNodeViewModel ParseLocalizedString(BaseGFFNodeViewModel hook, string label, XElement element)
    {
        var fieldNode = new LocalizedStringGFFNodeViewModel(hook, label);
        foreach (var childElement in element.Descendants())
        {
            var substring = new LocalizedSubStringViewModel();
            fieldNode.FieldValue.AddSubString(substring);
        }
        return fieldNode;
    }

    private Vector3GFFNodeViewModel ParseVector3(BaseGFFNodeViewModel hook, string label, XElement element)
    {
        var x = float.Parse(element.Element("X")!.Attribute("Value")!.Value);
        var y = float.Parse(element.Element("Y")!.Attribute("Value")!.Value);
        var z = float.Parse(element.Element("Z")!.Attribute("Value")!.Value);
        return new Vector3GFFNodeViewModel(hook, label, x, y, z);
    }

    private Vector4GFFNodeViewModel ParseVector4(BaseGFFNodeViewModel hook, string label, XElement element)
    {
        var x = float.Parse(element.Element("X")!.Attribute("Value")!.Value);
        var y = float.Parse(element.Element("Y")!.Attribute("Value")!.Value);
        var z = float.Parse(element.Element("Z")!.Attribute("Value")!.Value);
        var w = float.Parse(element.Element("W")!.Attribute("Value")!.Value);
        return new Vector4GFFNodeViewModel(hook, label, x, y, z, w);
    }

    private BinaryGFFNodeViewModel ParseBinary(BaseGFFNodeViewModel hook, string label, XElement element)
    {
        var data = Convert.FromBase64String(element.Attribute("Value").Value);
        return new BinaryGFFNodeViewModel(hook, label, data);
    }

    private ListGFFNodeViewModel ParseList(BaseGFFNodeViewModel hook, string label, XElement element)
    {
        var node = new ListGFFNodeViewModel(hook, label);
        foreach (var childElement in element.Descendants())
        {
            var structNode = Parse(node, childElement);
            node.AddStruct((ListStructGFFNodeViewModel)structNode);
        }
        return node;
    }

    private FieldStructGFFNodeViewModel ParseStruct(BaseGFFNodeViewModel hook, string label, XElement element)
    {
        var node = new FieldStructGFFNodeViewModel(hook, label);
        ParseStructInner(node, element);
        return node;
    }

    private void ParseStructInner(BaseGFFNodeViewModel structNode, XElement structElement)
    {
        if (structNode is IStructGFFNodeViewModel structNodeInterface)
        {
            foreach (var fieldElement in structElement.Descendants())
            {
                if (Parse(structNode, fieldElement) is BaseFieldGFFNodeViewModel fieldNode)
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
