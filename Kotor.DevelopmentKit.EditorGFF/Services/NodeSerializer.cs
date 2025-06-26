using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Kotor.DevelopmentKit.EditorGFF.Interfaces;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.DevelopmentKit.EditorGFF.Services;

public class NodeSerializer : INodeSerializer
{
    public string Serialize(BaseGFFNodeViewModel node)
    {
        return SerializeNode(node).ToString();
    }

    private XElement SerializeNode(BaseGFFNodeViewModel node)
    {
        var element = node switch
        {
            UInt8GFFNodeViewModel fieldNode => Serialize(fieldNode),
            Int8GFFNodeViewModel fieldNode => Serialize(fieldNode),
            UInt16GFFNodeViewModel fieldNode => Serialize(fieldNode),
            Int16GFFNodeViewModel fieldNode => Serialize(fieldNode),
            UInt32GFFNodeViewModel fieldNode => Serialize(fieldNode),
            Int32GFFNodeViewModel fieldNode => Serialize(fieldNode),
            UInt64GFFNodeViewModel fieldNode => Serialize(fieldNode),
            Int64GFFNodeViewModel fieldNode => Serialize(fieldNode),
            SingleGFFNodeViewModel fieldNode => Serialize(fieldNode),
            DoubleGFFNodeViewModel fieldNode => Serialize(fieldNode),
            ResRefGFFNodeViewModel fieldNode => Serialize(fieldNode),
            StringGFFNodeViewModel fieldNode => Serialize(fieldNode),
            LocalizedStringGFFNodeViewModel fieldNode => Serialize(fieldNode),
            BinaryGFFNodeViewModel fieldNode => Serialize(fieldNode),
            Vector3GFFNodeViewModel fieldNode => Serialize(fieldNode),
            Vector4GFFNodeViewModel fieldNode => Serialize(fieldNode),
            ListGFFNodeViewModel fieldNode => Serialize(fieldNode),
            FieldStructGFFNodeViewModel fieldNode => Serialize(fieldNode),
            ListStructGFFNodeViewModel fieldNode => Serialize(fieldNode),
            RootStructGFFNodeViewModel fieldNode => Serialize(fieldNode),
            _ => throw new NotSupportedException()
        };

        return element;
    }

    private XElement XElementForField(string type, string label, string value)
    {
        return new XElement("Field", new XAttribute("Type", type), new XAttribute("Label", label), new XAttribute("Value", value));
    }

    private XElement Serialize(UInt8GFFNodeViewModel fieldNode)
    {
        return XElementForField("UInt8", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(Int8GFFNodeViewModel fieldNode)
    {
        return XElementForField("Int8", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(UInt16GFFNodeViewModel fieldNode)
    {
        return XElementForField("UInt16", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(Int16GFFNodeViewModel fieldNode)
    {
        return XElementForField("UInt16", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(UInt32GFFNodeViewModel fieldNode)
    {
        return XElementForField("UInt32", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(Int32GFFNodeViewModel fieldNode)
    {
        return XElementForField("UInt32", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(UInt64GFFNodeViewModel fieldNode)
    {
        return XElementForField("UInt64", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(Int64GFFNodeViewModel fieldNode)
    {
        return XElementForField("UInt64", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(SingleGFFNodeViewModel fieldNode)
    {
        return XElementForField("Single", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(DoubleGFFNodeViewModel fieldNode)
    {
        return XElementForField("Double", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(ResRefGFFNodeViewModel fieldNode)
    {
        return XElementForField("ResRef", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(StringGFFNodeViewModel fieldNode)
    {
        return XElementForField("String", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(LocalizedStringGFFNodeViewModel fieldNode)
    {
        var element = XElementForField("LocalizedString", fieldNode.Label, $"{fieldNode.FieldValue.StringRef}");
        foreach (var substring in fieldNode.FieldValue.SubStrings)
        {
            element.Add(new XElement("Substring", new XAttribute("Gender", substring.Gender), new XAttribute("Language", substring.Language), new XAttribute("Text", substring.Text)));
        }
        return element;
    }
    private XElement Serialize(BinaryGFFNodeViewModel fieldNode)
    {
        return XElementForField("Binary", fieldNode.Label, Convert.ToBase64String(fieldNode.FieldValue));
    }
    private XElement Serialize(Vector3GFFNodeViewModel fieldNode)
    {
        var element = new XElement("Field", new XAttribute("Type", "Vector3"), new XAttribute("Label", fieldNode.Label));
        element.Add(new XElement("X", new XAttribute("Value", fieldNode.FieldValue.X)));
        element.Add(new XElement("Y", new XAttribute("Value", fieldNode.FieldValue.Y)));
        element.Add(new XElement("Z", new XAttribute("Value", fieldNode.FieldValue.Z)));
        return element;
    }
    private XElement Serialize(Vector4GFFNodeViewModel fieldNode)
    {
        var element = new XElement("Field", new XAttribute("Type", "Vector4"), new XAttribute("Label", fieldNode.Label));
        element.Add(new XElement("X", new XAttribute("Value", fieldNode.FieldValue.X)));
        element.Add(new XElement("Y", new XAttribute("Value", fieldNode.FieldValue.Y)));
        element.Add(new XElement("Z", new XAttribute("Value", fieldNode.FieldValue.Z)));
        element.Add(new XElement("W", new XAttribute("Value", fieldNode.FieldValue.W)));
        return element;
    }
    private XElement Serialize(ListGFFNodeViewModel fieldNode)
    {
        var element = new XElement("Field", new XAttribute("Type", "List"), new XAttribute("Label", fieldNode.Label));
        foreach (var structNode in fieldNode.Children)
        {
            element.Add(SerializeNode(structNode));
        }
        return element;
    }
    private XElement Serialize(FieldStructGFFNodeViewModel fieldNode)
    {
        var element = new XElement("Field", new XAttribute("Type", "Struct"), new XAttribute("Label", fieldNode.Label), new XAttribute("Value", fieldNode.StructID));
        foreach (var structNode in fieldNode.Children)
        {
            element.Add(SerializeNode(structNode));
        }
        return element;
    }
    private XElement Serialize(ListStructGFFNodeViewModel structNode)
    {
        var element = new XElement("Struct",  new XAttribute("StructID", structNode.StructID));
        foreach (var fieldNode in structNode.Children)
        {
            element.Add(SerializeNode(fieldNode));
        }
        return element;
    }
    private XElement Serialize(RootStructGFFNodeViewModel structNode)
    {
        return new XElement("Struct", new XAttribute("StructID", structNode.StructID));
    }
}
