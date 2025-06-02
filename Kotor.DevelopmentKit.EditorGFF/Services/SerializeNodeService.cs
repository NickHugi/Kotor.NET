using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.DevelopmentKit.EditorGFF.Services;

public class SerializeNodeService
{
    public string Serialize(BaseGFFNodeViewModel node)
    {
        return SerializeNode(node).ToString();
    }

    private XElement SerializeNode(BaseGFFNodeViewModel node)
    {
        var element = node switch
        {
            FieldUInt8GFFNodeViewModel fieldNode => Serialize(fieldNode),
            FieldInt8GFFNodeViewModel fieldNode => Serialize(fieldNode),
            FieldUInt16GFFNodeViewModel fieldNode => Serialize(fieldNode),
            FieldInt16GFFNodeViewModel fieldNode => Serialize(fieldNode),
            FieldUInt32GFFNodeViewModel fieldNode => Serialize(fieldNode),
            FieldInt32GFFNodeViewModel fieldNode => Serialize(fieldNode),
            FieldUInt64GFFNodeViewModel fieldNode => Serialize(fieldNode),
            FieldInt64GFFNodeViewModel fieldNode => Serialize(fieldNode),
            FieldSingleGFFNodeViewModel fieldNode => Serialize(fieldNode),
            FieldDoubleGFFNodeViewModel fieldNode => Serialize(fieldNode),
            FieldResRefGFFNodeViewModel fieldNode => Serialize(fieldNode),
            FieldStringGFFNodeViewModel fieldNode => Serialize(fieldNode),
            FieldLocalizedStringGFFNodeViewModel fieldNode => Serialize(fieldNode),
            FieldBinaryGFFNodeViewModel fieldNode => Serialize(fieldNode),
            FieldVector3GFFNodeViewModel fieldNode => Serialize(fieldNode),
            FieldVector4GFFNodeViewModel fieldNode => Serialize(fieldNode),
            FieldListGFFNodeViewModel fieldNode => Serialize(fieldNode),
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

    private XElement Serialize(FieldUInt8GFFNodeViewModel fieldNode)
    {
        return XElementForField("UInt8", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(FieldInt8GFFNodeViewModel fieldNode)
    {
        return XElementForField("Int8", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(FieldUInt16GFFNodeViewModel fieldNode)
    {
        return XElementForField("UInt16", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(FieldInt16GFFNodeViewModel fieldNode)
    {
        return XElementForField("UInt16", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(FieldUInt32GFFNodeViewModel fieldNode)
    {
        return XElementForField("UInt32", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(FieldInt32GFFNodeViewModel fieldNode)
    {
        return XElementForField("UInt32", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(FieldUInt64GFFNodeViewModel fieldNode)
    {
        return XElementForField("UInt64", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(FieldInt64GFFNodeViewModel fieldNode)
    {
        return XElementForField("UInt64", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(FieldSingleGFFNodeViewModel fieldNode)
    {
        return XElementForField("Single", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(FieldDoubleGFFNodeViewModel fieldNode)
    {
        return XElementForField("Double", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(FieldResRefGFFNodeViewModel fieldNode)
    {
        return XElementForField("ResRef", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(FieldStringGFFNodeViewModel fieldNode)
    {
        return XElementForField("String", fieldNode.Label, $"{fieldNode.FieldValue}");
    }
    private XElement Serialize(FieldLocalizedStringGFFNodeViewModel fieldNode)
    {
        var element = XElementForField("LocalizedString", fieldNode.Label, $"{fieldNode.FieldValue.StringRef}");
        foreach (var substring in fieldNode.FieldValue.SubStrings)
        {
            element.Add(new XElement("Substring", new XAttribute("Gender", substring.Gender), new XAttribute("Language", substring.Language), new XAttribute("Text", substring.Text)));
        }
        return element;
    }
    private XElement Serialize(FieldBinaryGFFNodeViewModel fieldNode)
    {
        return XElementForField("Binary", fieldNode.Label, Convert.ToBase64String(fieldNode.FieldValue));
    }
    private XElement Serialize(FieldVector3GFFNodeViewModel fieldNode)
    {
        var element = new XElement("Field", new XAttribute("Type", "Vector3"), new XAttribute("Label", fieldNode.Label));
        element.Add(new XElement("X", new XAttribute("Value", fieldNode.FieldValue.X)));
        element.Add(new XElement("Y", new XAttribute("Value", fieldNode.FieldValue.Y)));
        element.Add(new XElement("Z", new XAttribute("Value", fieldNode.FieldValue.Z)));
        return element;
    }
    private XElement Serialize(FieldVector4GFFNodeViewModel fieldNode)
    {
        var element = new XElement("Field", new XAttribute("Type", "Vector4"), new XAttribute("Label", fieldNode.Label));
        element.Add(new XElement("X", new XAttribute("Value", fieldNode.FieldValue.X)));
        element.Add(new XElement("Y", new XAttribute("Value", fieldNode.FieldValue.Y)));
        element.Add(new XElement("Z", new XAttribute("Value", fieldNode.FieldValue.Z)));
        element.Add(new XElement("W", new XAttribute("Value", fieldNode.FieldValue.W)));
        return element;
    }
    private XElement Serialize(FieldListGFFNodeViewModel fieldNode)
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
