using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Kotor.DevelopmentKit.EditorGFF.Interfaces;
using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.DevelopmentKit.EditorGFF.Services;

public class NodeSerializer : INodeSerializer
{
    public string Serialize(BaseGFFNode node)
    {
        return SerializeNode(node).ToString();
    }

    private XElement SerializeNode(BaseGFFNode node)
    {
        var element = node switch
        {
            UInt8GFFNode fieldNode => Serialize(fieldNode),
            Int8GFFNode fieldNode => Serialize(fieldNode),
            UInt16GFFNode fieldNode => Serialize(fieldNode),
            Int16GFFNode fieldNode => Serialize(fieldNode),
            UInt32GFFNode fieldNode => Serialize(fieldNode),
            Int32GFFNode fieldNode => Serialize(fieldNode),
            UInt64GFFNode fieldNode => Serialize(fieldNode),
            Int64GFFNode fieldNode => Serialize(fieldNode),
            SingleGFFNode fieldNode => Serialize(fieldNode),
            DoubleGFFNode fieldNode => Serialize(fieldNode),
            ResRefGFFNode fieldNode => Serialize(fieldNode),
            StringGFFNode fieldNode => Serialize(fieldNode),
            LocalizedStringGFFNode fieldNode => Serialize(fieldNode),
            BinaryGFFNode fieldNode => Serialize(fieldNode),
            Vector3GFFNode fieldNode => Serialize(fieldNode),
            Vector4GFFNode fieldNode => Serialize(fieldNode),
            ListGFFNode fieldNode => Serialize(fieldNode),
            FieldStructGFFNode fieldNode => Serialize(fieldNode),
            ListStructGFFNode fieldNode => Serialize(fieldNode),
            RootStructGFFNode fieldNode => Serialize(fieldNode),
            _ => throw new NotSupportedException()
        };

        return element;
    }

    private XElement XElementForField(string type, string label, string value)
    {
        return new XElement("Field", new XAttribute("Type", type), new XAttribute("Label", label), new XAttribute("Value", value));
    }

    private XElement Serialize(UInt8GFFNode fieldNode)
    {
        return XElementForField("UInt8", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(Int8GFFNode fieldNode)
    {
        return XElementForField("Int8", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(UInt16GFFNode fieldNode)
    {
        return XElementForField("UInt16", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(Int16GFFNode fieldNode)
    {
        return XElementForField("UInt16", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(UInt32GFFNode fieldNode)
    {
        return XElementForField("UInt32", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(Int32GFFNode fieldNode)
    {
        return XElementForField("UInt32", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(UInt64GFFNode fieldNode)
    {
        return XElementForField("UInt64", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(Int64GFFNode fieldNode)
    {
        return XElementForField("UInt64", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(SingleGFFNode fieldNode)
    {
        return XElementForField("Single", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(DoubleGFFNode fieldNode)
    {
        return XElementForField("Double", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(ResRefGFFNode fieldNode)
    {
        return XElementForField("ResRef", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(StringGFFNode fieldNode)
    {
        return XElementForField("String", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(LocalizedStringGFFNode fieldNode)
    {
        var element = XElementForField("LocalizedString", fieldNode.Label, $"{fieldNode.FieldValue.StringRef}");
        foreach (var substring in fieldNode.FieldValue.SubStrings)
        {
            element.Add(new XElement("Substring", new XAttribute("Gender", substring.Gender), new XAttribute("Language", substring.Language), new XAttribute("Text", substring.Text)));
        }
        return element;
    }
    private XElement Serialize(BinaryGFFNode fieldNode)
    {
        return XElementForField("Binary", fieldNode.Label, Convert.ToBase64String(fieldNode.FieldValue));
    }
    private XElement Serialize(Vector3GFFNode fieldNode)
    {
        var element = new XElement("Field", new XAttribute("Type", "Vector3"), new XAttribute("Label", fieldNode.Label));
        element.Add(new XElement("X", new XAttribute("Value", fieldNode.FieldValue.X)));
        element.Add(new XElement("Y", new XAttribute("Value", fieldNode.FieldValue.Y)));
        element.Add(new XElement("Z", new XAttribute("Value", fieldNode.FieldValue.Z)));
        return element;
    }
    private XElement Serialize(Vector4GFFNode fieldNode)
    {
        var element = new XElement("Field", new XAttribute("Type", "Vector4"), new XAttribute("Label", fieldNode.Label));
        element.Add(new XElement("X", new XAttribute("Value", fieldNode.FieldValue.X)));
        element.Add(new XElement("Y", new XAttribute("Value", fieldNode.FieldValue.Y)));
        element.Add(new XElement("Z", new XAttribute("Value", fieldNode.FieldValue.Z)));
        element.Add(new XElement("W", new XAttribute("Value", fieldNode.FieldValue.W)));
        return element;
    }
    private XElement Serialize(ListGFFNode fieldNode)
    {
        var element = new XElement("Field", new XAttribute("Type", "List"), new XAttribute("Label", fieldNode.Label));
        foreach (var structNode in fieldNode.Children)
        {
            element.Add(SerializeNode(structNode));
        }
        return element;
    }
    private XElement Serialize(FieldStructGFFNode fieldNode)
    {
        var element = new XElement("Field", new XAttribute("Type", "Struct"), new XAttribute("Label", fieldNode.Label), new XAttribute("Value", fieldNode.StructID));
        foreach (var structNode in fieldNode.Children)
        {
            element.Add(SerializeNode(structNode));
        }
        return element;
    }
    private XElement Serialize(ListStructGFFNode structNode)
    {
        var element = new XElement("Struct",  new XAttribute("StructID", structNode.StructID));
        foreach (var fieldNode in structNode.Children)
        {
            element.Add(SerializeNode(fieldNode));
        }
        return element;
    }
    private XElement Serialize(RootStructGFFNode structNode)
    {
        return new XElement("Struct", new XAttribute("StructID", structNode.StructID));
    }
}
