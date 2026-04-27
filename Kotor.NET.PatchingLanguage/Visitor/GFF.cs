using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Kotor.NET.Common.Data;
using Kotor.NET.Patcher.ForGFF;

namespace Kotor.NET.PatchingLanguage.Visitor;

public partial class KotorPatchingLanguageVisitor : KotorPatchingLanguageBaseVisitor<object>
{
    public override object VisitGFFLocateField([NotNull] KotorPatchingLanguageParser.GFFLocateFieldContext context)
    {
        return new ByPathFieldLocator
        {
            Path = GetStringLiteralText(context.STRING_LITERAL()).Split('\\')
        };
    }

    public override object VisitGFFAssignUInt8([NotNull] KotorPatchingLanguageParser.GFFAssignUInt8Context context)
    {
        return new EditUInt8Modifier
        {
            Field = (IFieldLocator)context.gff_locate_field(),
            Value = (IValue<byte>)context.gff_value_uint8(),
        };
    }
    public override object VisitGFFValueUInt8Literal([NotNull] KotorPatchingLanguageParser.GFFValueUInt8LiteralContext context)
    {
        return new ConstantValue<byte>
        {
            Value = byte.Parse(context.INT_LITERAL().GetText()),
        };
    }
    public override object VisitGFFValueUInt8Token([NotNull] KotorPatchingLanguageParser.GFFValueUInt8TokenContext context)
    {
        return new TokenValue<byte>
        {
            Token = GetMemoryTokenText(context.MEMORY_TOKEN())
        };
    }
    public override object VisitGFFValueUInt8From2DA([NotNull] KotorPatchingLanguageParser.GFFValueUInt8From2DAContext context)
    {
        return new TwoDARowIndexValue<byte>
        {
            ResRef = GetStringLiteralText(context.STRING_LITERAL(0)),
            SearchColumn = GetStringLiteralText(context.STRING_LITERAL(1)),
            SearchForCell = GetStringLiteralText(context.STRING_LITERAL(2)),
        };
    }

    public override object VisitGFFAssignUInt16([NotNull] KotorPatchingLanguageParser.GFFAssignUInt16Context context)
    {
        return new EditUInt16Modifier
        {
            Field = (IFieldLocator)context.gff_locate_field(),
            Value = (IValue<UInt16>)context.gff_value_uint16(),
        };
    }
    public override object VisitGFFValueUInt16Literal([NotNull] KotorPatchingLanguageParser.GFFValueUInt16LiteralContext context)
    {
        return new ConstantValue<UInt16>
        {
            Value = UInt16.Parse(context.INT_LITERAL().GetText()),
        };
    }
    public override object VisitGFFValueUInt16Token([NotNull] KotorPatchingLanguageParser.GFFValueUInt16TokenContext context)
    {
        return new TokenValue<UInt16>
        {
            Token = GetMemoryTokenText(context.MEMORY_TOKEN())
        };
    }
    public override object VisitGFFValueUInt16From2DA([NotNull] KotorPatchingLanguageParser.GFFValueUInt16From2DAContext context)
    {
        return new TwoDARowIndexValue<UInt16>
        {
            ResRef = GetStringLiteralText(context.STRING_LITERAL(0)),
            SearchColumn = GetStringLiteralText(context.STRING_LITERAL(1)),
            SearchForCell = GetStringLiteralText(context.STRING_LITERAL(2)),
        };
    }

    public override object VisitGFFAssignUInt32([NotNull] KotorPatchingLanguageParser.GFFAssignUInt32Context context)
    {
        return new EditUInt32Modifier
        {
            Field = (IFieldLocator)context.gff_locate_field(),
            Value = (IValue<uint>)context.gff_value_uint32(),
        };
    }
    public override object VisitGFFValueUInt32Literal([NotNull] KotorPatchingLanguageParser.GFFValueUInt32LiteralContext context)
    {
        return new ConstantValue<UInt32>
        {
            Value = uint.Parse(context.INT_LITERAL().GetText()),
        };
    }
    public override object VisitGFFValueUInt32Token([NotNull] KotorPatchingLanguageParser.GFFValueUInt32TokenContext context)
    {
        return new TokenValue<UInt32>
        {
            Token = GetMemoryTokenText(context.MEMORY_TOKEN())
        };
    }
    public override object VisitGFFValueUInt32From2DA([NotNull] KotorPatchingLanguageParser.GFFValueUInt32From2DAContext context)
    {
        return new TwoDARowIndexValue<UInt32>
        {
            ResRef = GetStringLiteralText(context.STRING_LITERAL(0)),
            SearchColumn = GetStringLiteralText(context.STRING_LITERAL(1)),
            SearchForCell = GetStringLiteralText(context.STRING_LITERAL(2)),
        };
    }

    public override object VisitGFFAssignUInt64([NotNull] KotorPatchingLanguageParser.GFFAssignUInt64Context context)
    {
        return new EditUInt64Modifier
        {
            Field = (IFieldLocator)context.gff_locate_field(),
            Value = (IValue<UInt64>)context.gff_value_uint64(),
        };
    }
    public override object VisitGFFValueUInt64Literal([NotNull] KotorPatchingLanguageParser.GFFValueUInt64LiteralContext context)
    {
        return new ConstantValue<UInt64>
        {
            Value = UInt64.Parse(context.INT_LITERAL().GetText()),
        };
    }
    public override object VisitGFFValueUInt64Token([NotNull] KotorPatchingLanguageParser.GFFValueUInt64TokenContext context)
    {
        return new TokenValue<UInt64>
        {
            Token = GetMemoryTokenText(context.MEMORY_TOKEN())
        };
    }
    public override object VisitGFFValueUInt64From2DA([NotNull] KotorPatchingLanguageParser.GFFValueUInt64From2DAContext context)
    {
        return new TwoDARowIndexValue<UInt64>
        {
            ResRef = GetStringLiteralText(context.STRING_LITERAL(0)),
            SearchColumn = GetStringLiteralText(context.STRING_LITERAL(1)),
            SearchForCell = GetStringLiteralText(context.STRING_LITERAL(2)),
        };
    }

    public override object VisitGFFAssignInt8([NotNull] KotorPatchingLanguageParser.GFFAssignInt8Context context)
    {
        return new EditInt8Modifier
        {
            Field = (IFieldLocator)context.gff_locate_field(),
            Value = (IValue<uint>)context.gff_value_uint32(),
        };
    }
    public override object VisitGFFValueInt8Literal([NotNull] KotorPatchingLanguageParser.GFFValueInt8LiteralContext context)
    {
        return new ConstantValue<sbyte>
        {
            Value = sbyte.Parse(context.INT_LITERAL().GetText()),
        };
    }
    public override object VisitGFFValueInt8Token([NotNull] KotorPatchingLanguageParser.GFFValueInt8TokenContext context)
    {
        return new TokenValue<sbyte>
        {
            Token = GetMemoryTokenText(context.MEMORY_TOKEN())
        };
    }
    public override object VisitGFFValueInt8From2DA([NotNull] KotorPatchingLanguageParser.GFFValueInt8From2DAContext context)
    {
        return new TwoDARowIndexValue<sbyte>
        {
            ResRef = GetStringLiteralText(context.STRING_LITERAL(0)),
            SearchColumn = GetStringLiteralText(context.STRING_LITERAL(1)),
            SearchForCell = GetStringLiteralText(context.STRING_LITERAL(2)),
        };
    }

    public override object VisitGFFAssignInt16([NotNull] KotorPatchingLanguageParser.GFFAssignInt16Context context)
    {
        return new EditInt16Modifier
        {
            Field = (IFieldLocator)context.gff_locate_field(),
            Value = (IValue<Int16>)context.gff_value_int16(),
        };
    }
    public override object VisitGFFValueInt16Literal([NotNull] KotorPatchingLanguageParser.GFFValueInt16LiteralContext context)
    {
        return new ConstantValue<Int16>
        {
            Value = ushort.Parse(context.INT_LITERAL().GetText()),
        };
    }
    public override object VisitGFFValueInt16Token([NotNull] KotorPatchingLanguageParser.GFFValueInt16TokenContext context)
    {
        return new TokenValue<Int16>
        {
            Token = GetMemoryTokenText(context.MEMORY_TOKEN())
        };
    }
    public override object VisitGFFValueInt16From2DA([NotNull] KotorPatchingLanguageParser.GFFValueInt16From2DAContext context)
    {
        return new TwoDARowIndexValue<Int16>
        {
            ResRef = GetStringLiteralText(context.STRING_LITERAL(0)),
            SearchColumn = GetStringLiteralText(context.STRING_LITERAL(1)),
            SearchForCell = GetStringLiteralText(context.STRING_LITERAL(2)),
        };
    }

    public override object VisitGFFAssignInt32([NotNull] KotorPatchingLanguageParser.GFFAssignInt32Context context)
    {
        return new EditInt32Modifier
        {
            Field = (IFieldLocator)context.gff_locate_field(),
            Value = (IValue<Int32>)context.gff_value_int32(),
        };
    }
    public override object VisitGFFValueInt32Literal([NotNull] KotorPatchingLanguageParser.GFFValueInt32LiteralContext context)
    {
        return new ConstantValue<Int32>
        {
            Value = Int32.Parse(context.INT_LITERAL().GetText()),
        };
    }
    public override object VisitGFFValueInt32Token([NotNull] KotorPatchingLanguageParser.GFFValueInt32TokenContext context)
    {
        return new TokenValue<Int32>
        {
            Token = GetMemoryTokenText(context.MEMORY_TOKEN())
        };
    }
    public override object VisitGFFValueInt32From2DA([NotNull] KotorPatchingLanguageParser.GFFValueInt32From2DAContext context)
    {
        return new TwoDARowIndexValue<Int32>
        {
            ResRef = GetStringLiteralText(context.STRING_LITERAL(0)),
            SearchColumn = GetStringLiteralText(context.STRING_LITERAL(1)),
            SearchForCell = GetStringLiteralText(context.STRING_LITERAL(2)),
        };
    }

    public override object VisitGFFAssignInt64([NotNull] KotorPatchingLanguageParser.GFFAssignInt64Context context)
    {
        return new EditInt64Modifier
        {
            Field = (IFieldLocator)context.gff_locate_field(),
            Value = (IValue<Int64>)context.gff_value_int64(),
        };
    }
    public override object VisitGFFValueInt64Literal([NotNull] KotorPatchingLanguageParser.GFFValueInt64LiteralContext context)
    {
        return new ConstantValue<Int64>
        {
            Value = Int64.Parse(context.INT_LITERAL().GetText()),
        };
    }
    public override object VisitGFFValueInt64Token([NotNull] KotorPatchingLanguageParser.GFFValueInt64TokenContext context)
    {
        return new TokenValue<Int64>
        {
            Token = GetMemoryTokenText(context.MEMORY_TOKEN())
        };
    }
    public override object VisitGFFValueInt64From2DA([NotNull] KotorPatchingLanguageParser.GFFValueInt64From2DAContext context)
    {
        return new TwoDARowIndexValue<Int64>
        {
            ResRef = GetStringLiteralText(context.STRING_LITERAL(0)),
            SearchColumn = GetStringLiteralText(context.STRING_LITERAL(1)),
            SearchForCell = GetStringLiteralText(context.STRING_LITERAL(2)),
        };
    }

    public override object VisitGFFAssignSingle([NotNull] KotorPatchingLanguageParser.GFFAssignSingleContext context)
    {
        return new EditSingleModifier
        {
            Field = (IFieldLocator)context.gff_locate_field(),
            Value = (IValue<Single>)context.gff_value_single(),
        };
    }
    public override object VisitGFFValueSingleLiteral([NotNull] KotorPatchingLanguageParser.GFFValueSingleLiteralContext context)
    {
        return new ConstantValue<Single>
        {
            Value = Single.Parse(context.FLOAT_LITERAL().GetText()),
        };
    }
    public override object VisitGFFValueSingleToken([NotNull] KotorPatchingLanguageParser.GFFValueSingleTokenContext context)
    {
        return new TokenValue<Single>
        {
            Token = GetMemoryTokenText(context.MEMORY_TOKEN())
        };
    }
    public override object VisitGFFValueSingleFrom2DA([NotNull] KotorPatchingLanguageParser.GFFValueSingleFrom2DAContext context)
    {
        return new TwoDARowIndexValue<Single>
        {
            ResRef = GetStringLiteralText(context.STRING_LITERAL(0)),
            SearchColumn = GetStringLiteralText(context.STRING_LITERAL(1)),
            SearchForCell = GetStringLiteralText(context.STRING_LITERAL(2)),
        };
    }


    public override object VisitGFFAssignDouble([NotNull] KotorPatchingLanguageParser.GFFAssignDoubleContext context)
    {
        return new EditDoubleModifier
        {
            Field = (IFieldLocator)context.gff_locate_field(),
            Value = (IValue<Double>)context.gff_value_double(),
        };
    }
    public override object VisitGFFValueDoubleLiteral([NotNull] KotorPatchingLanguageParser.GFFValueDoubleLiteralContext context)
    {
        return new ConstantValue<Double>
        {
            Value = Double.Parse(context.FLOAT_LITERAL().GetText()),
        };
    }
    public override object VisitGFFValueDoubleToken([NotNull] KotorPatchingLanguageParser.GFFValueDoubleTokenContext context)
    {
        return new TokenValue<Double>
        {
            Token = GetMemoryTokenText(context.MEMORY_TOKEN())
        };
    }
    public override object VisitGFFValueDoubleFrom2DA([NotNull] KotorPatchingLanguageParser.GFFValueDoubleFrom2DAContext context)
    {
        return new TwoDARowIndexValue<Double>
        {
            ResRef = GetStringLiteralText(context.STRING_LITERAL(0)),
            SearchColumn = GetStringLiteralText(context.STRING_LITERAL(1)),
            SearchForCell = GetStringLiteralText(context.STRING_LITERAL(2)),
        };
    }

    public override object VisitGFFAssignResRef([NotNull] KotorPatchingLanguageParser.GFFAssignResRefContext context)
    {
        return new EditResRefModifier
        {
            Field = (IFieldLocator)context.gff_locate_field(),
            Value = (IValue<ResRef>)context.gff_value_resref(),
        };
    }
    public override object VisitGFFValueResRefLiteral([NotNull] KotorPatchingLanguageParser.GFFValueResRefLiteralContext context)
    {
        return new ConstantValue<ResRef>
        {
            Value = context.STRING_LITERAL().GetText(),
        };
    }
    public override object VisitGFFValueResRefToken([NotNull] KotorPatchingLanguageParser.GFFValueResRefTokenContext context)
    {
        return new TokenValue<ResRef>
        {
            Token = GetMemoryTokenText(context.MEMORY_TOKEN())
        };
    }
    public override object VisitGFFValueResRefFrom2DA([NotNull] KotorPatchingLanguageParser.GFFValueResRefFrom2DAContext context)
    {
        return new TwoDARowIndexValue<ResRef>
        {
            ResRef = GetStringLiteralText(context.STRING_LITERAL(0)),
            SearchColumn = GetStringLiteralText(context.STRING_LITERAL(1)),
            SearchForCell = GetStringLiteralText(context.STRING_LITERAL(2)),
        };
    }

    public override object VisitGFFAssignString([NotNull] KotorPatchingLanguageParser.GFFAssignStringContext context)
    {
        return new EditStringModifier
        {
            Field = (IFieldLocator)context.gff_locate_field(),
            Value = (IValue<String>)context.gff_value_string(),
        };
    }
    public override object VisitGFFValueStringLiteral([NotNull] KotorPatchingLanguageParser.GFFValueStringLiteralContext context)
    {
        return new ConstantValue<String>
        {
            Value = context.STRING_LITERAL().GetText(),
        };
    }
    public override object VisitGFFValueStringToken([NotNull] KotorPatchingLanguageParser.GFFValueStringTokenContext context)
    {
        return new TokenValue<String>
        {
            Token = GetMemoryTokenText(context.MEMORY_TOKEN())
        };
    }
    public override object VisitGFFValueStringFrom2DA([NotNull] KotorPatchingLanguageParser.GFFValueStringFrom2DAContext context)
    {
        return new TwoDARowIndexValue<String>
        {
            ResRef = GetStringLiteralText(context.STRING_LITERAL(0)),
            SearchColumn = GetStringLiteralText(context.STRING_LITERAL(1)),
            SearchForCell = GetStringLiteralText(context.STRING_LITERAL(2)),
        };
    }

    public override object VisitGFFAssignBinary([NotNull] KotorPatchingLanguageParser.GFFAssignBinaryContext context)
    {
        return new EditBinaryModifier
        {
            Field = (IFieldLocator)context.gff_locate_field(),
            Value = (IValue<byte[]>)context.gff_value_binary(),
        };
    }
    public override object VisitGFFValueBinaryBase64([NotNull] KotorPatchingLanguageParser.GFFValueBinaryBase64Context context)
    {
        var base64 = GetStringLiteralText(context.STRING_LITERAL());
        var data = Convert.FromBase64String(base64);

        return new ConstantValue<byte[]>
        {
            Value = data,
        };
    }

    public override object VisitGFFAssignLocalizedStringStringRef([NotNull] KotorPatchingLanguageParser.GFFAssignLocalizedStringStringRefContext context)
    {
        return new EditLocalizedStringStringRefModifier
        {
            Field = (IFieldLocator)context.gff_locate_field(),
            Value = (IValue<int>)context.gff_value_int32(),
        };
    }

    public override object VisitGFFAssignVector3([NotNull] KotorPatchingLanguageParser.GFFAssignVector3Context context)
    {
        return new EditVector3Modifier
        {
            Field = (IFieldLocator)context.gff_locate_field(),
            Value = (IValue<Vector3>)context.gff_value_vector3(),
        };
    }
    public override object VisitGFFValueVector3Literal([NotNull] KotorPatchingLanguageParser.GFFValueVector3LiteralContext context)
    {
        return new ConstantValue<Vector3>
        {
            Value = GetVector3LiteralValue(context.VECTOR3_LITERAL()),
        };
    }

    public override object VisitGFFAssignVector4([NotNull] KotorPatchingLanguageParser.GFFAssignVector4Context context)
    {
        return new EditVector4Modifier
        {
            Field = (IFieldLocator)context.gff_locate_field(),
            Value = (IValue<Vector4>)context.gff_value_vector4(),
        };
    }
    public override object VisitGFFValueVector4Literal([NotNull] KotorPatchingLanguageParser.GFFValueVector4LiteralContext context)
    {
        return new ConstantValue<Vector4>
        {
            Value = GetVector4LiteralValue(context.VECTOR4_LITERAL()),
        };
    }

}
