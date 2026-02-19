using System.Collections;
using System.Numerics;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorARE;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Resources.KotorGUI;

public class GUI
{
    
}

public class GUIControl
{
    private readonly GFFStruct _controlStruct;

    // Obj_Parent
    // Obj_ParentID

    public virtual int Type
    {
        get => _controlStruct.GetInt32("CONTROLTYPE") ?? throw new Exception();
    }

    public string Tag
    {
        get => _controlStruct.GetString("TAG") ?? "";
        set => _controlStruct.SetString("TAG", value);
    }

    public bool Locked
    {
        get => _controlStruct.GetUInt8("Obj_Locked") > 0;
        set => _controlStruct.SetUInt8("Obj_Locked", Convert.ToByte(value));
    }

    public float Alpha
    {
        get => _controlStruct.GetSingle("ALPHA") ?? 0.0f;
        set => _controlStruct.SetSingle("ALPHA", value);
    }

    public Vector3 Color
    {
        get => _controlStruct.GetVector3("COLOR") ?? new();
        set => _controlStruct.SetVector3("COLOR", value);
    }

    public GUIControl(GFFStruct controlStruct)
    {
        _controlStruct = controlStruct;
    }
}

public class GUIExtent
{
    private readonly GFFStruct _parentStruct;
    private GFFStruct _extentStruct => _parentStruct.GetStruct("EXTENT");

    public int Left
    {
        get => _extentStruct.GetInt32("LEFT") ?? 0;
        set => _extentStruct.SetInt32("LEFT", value);
    }

    public int Top
    {
        get => _extentStruct.GetInt32("TOP") ?? 0;
        set => _extentStruct.SetInt32("TOP", value);
    }

    public int Width
    {
        get => _extentStruct.GetInt32("WIDTH") ?? 0;
        set => _extentStruct.SetInt32("WIDTH", value);
    }

    public int Height
    {
        get => _extentStruct.GetInt32("HEIGHT") ?? 0;
        set => _extentStruct.SetInt32("HEIGHT", value);
    }

    public GUIExtent(GFFStruct parentStruct)
    {
        _parentStruct = parentStruct;
    }
}

public class GUIBorder
{
    private readonly GFFStruct _parentStruct;
    private GFFStruct _borderStruct => _parentStruct.GetStruct("BORDER");

    public GUIFillStyle FillStyle
    {
        get => (GUIFillStyle)(_borderStruct.GetInt32("FILLSTYLE") ?? 0);
        set => _borderStruct.SetInt32("FILLSTYLE", (int)value);
    }

    public ResRef Corner
    {
        get => _borderStruct.GetResRef("CORNER") ?? "";
        set => _borderStruct.SetResRef("CORNER", value);
    }

    public ResRef Edge
    {
        get => _borderStruct.GetResRef("EDGE") ?? "";
        set => _borderStruct.SetResRef("EDGE", value);
    }

    public ResRef Fill
    {
        get => _borderStruct.GetResRef("FILL") ?? "";
        set => _borderStruct.SetResRef("FILL", value);
    }

    public int Dimension
    {
        get => _borderStruct.GetInt32("DIMENSION") ?? 0;
        set => _borderStruct.SetInt32("DIMENSION", value);
    }

    public int InnerOffset
    {
        get => _borderStruct.GetInt32("INNEROFFSET") ?? 0;
        set => _borderStruct.SetInt32("INNEROFFSET", value);
    }

    public int InnerOffsetY
    {
        get => _borderStruct.GetInt32("INNEROFFSETY") ?? 0;
        set => _borderStruct.SetInt32("INNEROFFSETY", value);
    }

    public Vector3 Color
    {
        get => _borderStruct.GetVector3("COLOR") ?? new();
        set => _borderStruct.SetVector3("COLOR", value);
    }

    public bool Pulsing
    {
        get => _borderStruct.GetUInt8("PULSING") > 0;
        set => _borderStruct.SetUInt8("PULSING", Convert.ToByte(value));
    }

    public GUIBorder(GFFStruct parentStruct)
    {
        _parentStruct = parentStruct;
    }
}

public enum GUIFillStyle
{
    None    = -1,
    Empty   =  0,
    Solid   =  1,
    Texture =  2,
}

public class GUIText
{
    private readonly GFFStruct _parentStruct;
    private GFFStruct _textStruct => _parentStruct.GetStruct("TEXT");

    public string Text
    {
        get => _textStruct.GetString("TEXT") ?? "";
        set => _textStruct.SetString("TEXT", value);
    }

    public uint StrRef
    {
        get => _textStruct.GetUInt32("STRREF") ?? 0;
        set => _textStruct.SetUInt32("STRREF", value);
    }

    public ResRef Font
    {
        get => _textStruct.GetResRef("FONT") ?? "";
        set => _textStruct.SetResRef("FONT", value);
    }

    public GUITextAlignment Alignment
    {
        get => (GUITextAlignment)(_textStruct.GetInt32("ALIGNMENT") ?? 0);
        set => _textStruct.SetInt32("ALIGNMENT", (int)value);
    }

    public Vector3 Color
    {
        get => _textStruct.GetVector3("COLOR") ?? new();
        set => _textStruct.SetVector3("COLOR", value);
    }

    public bool Pulsing
    {
        get => Convert.ToBoolean(_textStruct.GetUInt8("PULSING") ?? 0);
        set => _textStruct.SetUInt8("PULSING", Convert.ToByte(value));
    }

    public GUIText(GFFStruct parentStruct)
    {
        _parentStruct = parentStruct;
    }
}

public enum GUITextAlignment
{
    TopLeft = 1,
    TopCenter = 2,
    TopRight = 3,
    CenterLeft = 17,
    Center = 18,
    CenterRight = 19,
    BottomLeft = 33,
    BottomCenter = 34,
    BottomRight = 35,
}

public class GUIMoveTo
{
    private readonly GFFStruct _parentStruct;
    private GFFStruct _moveToStruct => _parentStruct.GetStruct("MOVETO");

    public int Up
    {
        get => _moveToStruct.GetInt32("UP") ?? 0;
        set => _moveToStruct.SetInt32("UP", value);
    }

    public int Down
    {
        get => _moveToStruct.GetInt32("DOWN") ?? 0;
        set => _moveToStruct.SetInt32("DOWN", value);
    }

    public int Left
    {
        get => _moveToStruct.GetInt32("LEFT") ?? 0;
        set => _moveToStruct.SetInt32("LEFT", value);
    }

    public int Right
    {
        get => _moveToStruct.GetInt32("RIGHT") ?? 0;
        set => _moveToStruct.SetInt32("RIGHT", value);
    }

    public GUIMoveTo(GFFStruct parentStruct)
    {
        _parentStruct = parentStruct;
    }
}

public class GUIHilight
{
    private readonly GFFStruct _parentStruct;
    private GFFStruct _hilightStruct => _parentStruct.GetStruct("HILIGHT");

    public ResRef Corner
    {
        get => _hilightStruct.GetResRef("CORNER") ?? "";
        set => _hilightStruct.SetResRef("CORNER", value);
    }

    public ResRef Edge
    {
        get => _hilightStruct.GetResRef("EDGE") ?? "";
        set => _hilightStruct.SetResRef("EDGE", value);
    }

    public ResRef Fill
    {
        get => _hilightStruct.GetResRef("FILL") ?? "";
        set => _hilightStruct.SetResRef("FILL", value);
    }

    public GUIFillStyle FillStyle
    {
        get => (GUIFillStyle)(_hilightStruct.GetInt32("FILLSTYLE") ?? 0);
        set => _hilightStruct.SetInt32("FILLSTYLE", (int)value);
    }

    public int Dimension
    {
        get => _hilightStruct.GetInt32("DIMENSION") ?? 0;
        set => _hilightStruct.SetInt32("DIMENSION", value);
    }

    public int InnerOffset
    {
        get => _hilightStruct.GetInt32("INNEROFFSET") ?? 0;
        set => _hilightStruct.SetInt32("INNEROFFSET", value);
    }

    public int InnerOffsetY
    {
        get => _hilightStruct.GetInt32("INNEROFFSETY") ?? 0;
        set => _hilightStruct.SetInt32("INNEROFFSETY", value);
    }

    public Vector3 Color
    {
        get => _hilightStruct.GetVector3("COLOR") ?? new();
        set => _hilightStruct.SetVector3("COLOR", value);
    }

    public bool Pulsing
    {
        get => Convert.ToBoolean(_hilightStruct.GetUInt8("PULSING") ?? 0);
        set => _hilightStruct.SetUInt8("PULSING", Convert.ToByte(value));
    }

    public GUIHilight(GFFStruct parentStruct)
    {
        _parentStruct = parentStruct;
    }
}

public class GUISelected
{
    private readonly GFFStruct _parentStruct;
    private GFFStruct _selectedStruct => _parentStruct.GetStruct("SELECTED");

    public ResRef Corner
    {
        get => _selectedStruct.GetResRef("CORNER") ?? "";
        set => _selectedStruct.SetResRef("CORNER", value);
    }

    public ResRef Edge
    {
        get => _selectedStruct.GetResRef("EDGE") ?? "";
        set => _selectedStruct.SetResRef("EDGE", value);
    }

    public ResRef Fill
    {
        get => _selectedStruct.GetResRef("FILL") ?? "";
        set => _selectedStruct.SetResRef("FILL", value);
    }

    public GUIFillStyle FillStyle
    {
        get => (GUIFillStyle)(_selectedStruct.GetInt32("FILLSTYLE") ?? 0);
        set => _selectedStruct.SetInt32("FILLSTYLE", (int)value);
    }

    public int Dimension
    {
        get => _selectedStruct.GetInt32("DIMENSION") ?? 0;
        set => _selectedStruct.SetInt32("DIMENSION", value);
    }

    public int InnerOffset
    {
        get => _selectedStruct.GetInt32("INNEROFFSET") ?? 0;
        set => _selectedStruct.SetInt32("INNEROFFSET", value);
    }

    public int InnerOffsetY
    {
        get => _selectedStruct.GetInt32("INNEROFFSETY") ?? 0;
        set => _selectedStruct.SetInt32("INNEROFFSETY", value);
    }

    public Vector3 Color
    {
        get => _selectedStruct.GetVector3("COLOR") ?? new();
        set => _selectedStruct.SetVector3("COLOR", value);
    }

    public bool Pulsing
    {
        get => Convert.ToBoolean(_selectedStruct.GetUInt8("PULSING") ?? 0);
        set => _selectedStruct.SetUInt8("PULSING", Convert.ToByte(value));
    }

    public GUISelected(GFFStruct parentStruct)
    {
        _parentStruct = parentStruct;
    }
}

public class GUISelectedHilight
{
    private readonly GFFStruct _parentStruct;
    private GFFStruct _selectedHightlightStruct => _parentStruct.GetStruct("HILIGHTSELECTED");

    public ResRef Corner
    {
        get => _selectedHightlightStruct.GetResRef("CORNER") ?? "";
        set => _selectedHightlightStruct.SetResRef("CORNER", value);
    }

    public ResRef Edge
    {
        get => _selectedHightlightStruct.GetResRef("EDGE") ?? "";
        set => _selectedHightlightStruct.SetResRef("EDGE", value);
    }

    public ResRef Fill
    {
        get => _selectedHightlightStruct.GetResRef("FILL") ?? "";
        set => _selectedHightlightStruct.SetResRef("FILL", value);
    }

    public GUIFillStyle FillStyle
    {
        get => (GUIFillStyle)(_selectedHightlightStruct.GetInt32("FILLSTYLE") ?? 0);
        set => _selectedHightlightStruct.SetInt32("FILLSTYLE", (int)value);
    }

    public int Dimension
    {
        get => _selectedHightlightStruct.GetInt32("DIMENSION") ?? 0;
        set => _selectedHightlightStruct.SetInt32("DIMENSION", value);
    }

    public int InnerOffset
    {
        get => _selectedHightlightStruct.GetInt32("INNEROFFSET") ?? 0;
        set => _selectedHightlightStruct.SetInt32("INNEROFFSET", value);
    }

    public int InnerOffsetY
    {
        get => _selectedHightlightStruct.GetInt32("INNEROFFSETY") ?? 0;
        set => _selectedHightlightStruct.SetInt32("INNEROFFSETY", value);
    }

    public Vector3 Color
    {
        get => _selectedHightlightStruct.GetVector3("COLOR") ?? new();
        set => _selectedHightlightStruct.SetVector3("COLOR", value);
    }

    public bool Pulsing
    {
        get => Convert.ToBoolean(_selectedHightlightStruct.GetUInt8("PULSING") ?? 0);
        set => _selectedHightlightStruct.SetUInt8("PULSING", Convert.ToByte(value));
    }

    public GUISelectedHilight(GFFStruct parentStruct)
    {
        _parentStruct = parentStruct;
    }
}

public class GUIListBox : GUIControl
{
    private readonly GFFStruct _listBoxStruct;

    public GUIProtoItem ProtoItem => new(_listBoxStruct.GetStruct("PROTOITEM"));
    public GUIScrollBar ScrollBar => new(_listBoxStruct.GetStruct("SCROLLBAR"));

    public int Padding
    {
        get => _listBoxStruct.GetInt32("PADDING") ?? 0;
        set => _listBoxStruct.SetInt32("PADDING", value);
    }

    public int MaxValue
    {
        get => _listBoxStruct.GetInt32("MAXVALUE") ?? 0;
        set => _listBoxStruct.SetInt32("MAXVALUE", value);
    }

    public int CurrentValue
    {
        get => _listBoxStruct.GetInt32("CURVALUE") ?? 0;
        set => _listBoxStruct.SetInt32("CURVALUE", value);
    }

    public bool Looping
    {
        get => Convert.ToBoolean(_listBoxStruct.GetUInt8("LOOPING") ?? 0);
        set => _listBoxStruct.SetUInt8("LOOPING", Convert.ToByte(value));
    }

    public GUIListBox(GFFStruct listBoxStruct) : base(listBoxStruct)
    {
        _listBoxStruct = listBoxStruct;
    }
}

public class GUIScrollBar : GUIControl
{
    private readonly GFFStruct _scrollBarStruct;

    public GUIScrollBar DirectionButtons => new(_scrollBarStruct.GetStruct("DIR"));
    public GUIScrollBar Thumb => new(_scrollBarStruct.GetStruct("THUMB"));

    public int MaxValue
    {
        get => _scrollBarStruct.GetInt32("MAXVALUE") ?? 0;
        set => _scrollBarStruct.SetInt32("MAXVALUE", value);
    }

    public int VisibleValue
    {
        get => _scrollBarStruct.GetInt32("VISIBLEVALUE") ?? 0;
        set => _scrollBarStruct.SetInt32("VISIBLEVALUE", value);
    }

    public int CurrentValue
    {
        get => _scrollBarStruct.GetInt32("CURVALUE") ?? 0;
        set => _scrollBarStruct.SetInt32("CURVALUE", value);
    }

    public bool DrawMode
    {
        get => Convert.ToBoolean(_scrollBarStruct.GetUInt8("DRAWMODE") ?? 0);
        set => _scrollBarStruct.SetUInt8("DRAWMODE", Convert.ToByte(value));
    }

    public GUIScrollBar(GFFStruct scrollBarStruct) : base(scrollBarStruct)
    {
        _scrollBarStruct = scrollBarStruct;
    }
}

public class GUIScrollBarDirectionButton
{
    private readonly GFFStruct _scrollBarStruct;
    private GFFStruct _directionStruct => _scrollBarStruct.GetStruct("DIR");

    public ResRef Image
    {
        get => _directionStruct.GetResRef("IMAGE") ?? "";
        set => _directionStruct.SetResRef("IMAGE", value);
    }

    public GUITextAlignment Alignment
    {
        get => (GUITextAlignment)(_directionStruct.GetInt32("ALIGNMENT") ?? 0);
        set => _directionStruct.SetInt32("ALIGNMENT", (int)value);
    }

    public GUIScrollBarDirectionButton(GFFStruct scrollBarStruct)
    {
        _scrollBarStruct = scrollBarStruct;
    }
}

public class GUIScrollBarThumb
{
    private readonly GFFStruct _scrollBarStruct;
    private GFFStruct _thumbStruct => _scrollBarStruct.GetStruct("THUMB");

    public ResRef Image
    {
        get => _thumbStruct.GetResRef("IMAGE") ?? "";
        set => _thumbStruct.SetResRef("IMAGE", value);
    }

    public GUITextAlignment Alignment
    {
        get => (GUITextAlignment)(_thumbStruct.GetInt32("ALIGNMENT") ?? 0);
        set => _thumbStruct.SetInt32("ALIGNMENT", (int)value);
    }

    public GUIScrollBarThumb(GFFStruct scrollBarStruct)
    {
        _scrollBarStruct = scrollBarStruct;
    }
}

public class GUIProgressBar : GUIControl
{
    private readonly GFFStruct _progressBarStruct;

    public GUIProgressBarFill Fill => new(_progressBarStruct);

    public int CurrentValue
    {
        get => _progressBarStruct.GetInt32("CURVALUE") ?? 0;
        set => _progressBarStruct.SetInt32("CURVALUE", value);
    }

    public int MaxValue
    {
        get => _progressBarStruct.GetInt32("MAXVALUE") ?? 0;
        set => _progressBarStruct.SetInt32("MAXVALUE", value);
    }

    public GUIFillDirection FillDirection
    {
        get => (GUIFillDirection)(_progressBarStruct.GetUInt8("STARTFROMLEFT") ?? 0);
        set => _progressBarStruct.SetUInt8("STARTFROMLEFT", (byte)value);
    }

    public GUIProgressBar(GFFStruct progressBarStruct) : base(progressBarStruct)
    {
        _progressBarStruct = progressBarStruct;
    }
}

public enum GUIFillDirection
{
    Right = 0,
    Left = 1,
}

public class GUIProgressBarFill
{
    private readonly GFFStruct _progressBarStruct;
    private GFFStruct _fillStruct => _progressBarStruct.GetStruct("PROGRESS");

    public ResRef Corner
    {
        get => _fillStruct.GetResRef("CORNER") ?? "";
        set => _fillStruct.SetResRef("CORNER", value);
    }

    public ResRef Edge
    {
        get => _fillStruct.GetResRef("EDGE") ?? "";
        set => _fillStruct.SetResRef("EDGE", value);
    }

    public ResRef Fill
    {
        get => _fillStruct.GetResRef("FILL") ?? "";
        set => _fillStruct.SetResRef("FILL", value);
    }

    public GUIFillStyle FillStyle
    {
        get => (GUIFillStyle)(_fillStruct.GetInt32("FILLSTYLE") ?? 0);
        set => _fillStruct.SetInt32("FILLSTYLE", (int)value);
    }

    public int BorderThickness
    {
        get => _fillStruct.GetInt32("DIMENSION") ?? 0;
        set => _fillStruct.SetInt32("DIMENSION", value);
    }

    public int InnerOffsetX
    {
        get => _fillStruct.GetInt32("INNEROFFSET") ?? 0;
        set => _fillStruct.SetInt32("INNEROFFSET", value);
    }

    public int InnerOffsetY
    {
        get => _fillStruct.GetInt32("INNEROFFSETY") ?? 0;
        set => _fillStruct.SetInt32("INNEROFFSETY", value);
    }

    public Vector3 Color
    {
        get => _fillStruct.GetVector3("COLOR") ?? new();
        set => _fillStruct.SetVector3("COLOR", value);
    }

    public bool Pulsing
    {
        get => Convert.ToBoolean(_fillStruct.GetUInt8("PULSING") ?? 0);
        set => _fillStruct.SetUInt8("PULSING", Convert.ToByte(value));
    }

    public GUIProgressBarFill(GFFStruct progressBarStruct)
    {
        _progressBarStruct = progressBarStruct;
    }
}

public class GUICheckbox : GUIControl
{
    private readonly GFFStruct _checkboxStruct;

    public GUISelected Selected => new(_checkboxStruct);
    public GUIHilight HilightSelected => new(_checkboxStruct);

    public bool Pulsing
    {
        get => Convert.ToBoolean(_checkboxStruct.GetUInt8("ISSELECTED") ?? 0);
        set => _checkboxStruct.SetUInt8("ISSELECTED", Convert.ToByte(value));
    }

    public GUICheckbox(GFFStruct checkboxStruct) : base(checkboxStruct)
    {
        _checkboxStruct = checkboxStruct;
    }
}

public class GUISlider : GUIControl
{
    private readonly GFFStruct _sliderStruct;

    public GUISliderThumb Thumb => new(_sliderStruct);

    public int CurrentValue
    {
        get => _sliderStruct.GetInt32("CURVALUE") ?? 0;
        set => _sliderStruct.SetInt32("CURVALUE", value);
    }

    public int MaxValue
    {
        get => _sliderStruct.GetInt32("MAXVALUE") ?? 0;
        set => _sliderStruct.SetInt32("MAXVALUE", value);
    }

    public GUIDirection Direction
    {
        get => (GUIDirection)(_sliderStruct.GetInt32("DIRECTION") ?? 0);
        set => _sliderStruct.SetInt32("DIRECTION", (int)value);
    }

    public GUISlider(GFFStruct sliderStruct) : base(sliderStruct)
    {
        _sliderStruct = sliderStruct;
    }
}

public enum GUIDirection
{
    Horizontal = 0,
    Vertical = 1,
}

public class GUISliderThumb
{
    private readonly GFFStruct _sliderStruct;
    private GFFStruct _thumbStruct => _sliderStruct.GetStruct("THUMB");

    public ResRef Image
    {
        get => _thumbStruct.GetResRef("IMAGE") ?? "";
        set => _thumbStruct.SetResRef("IMAGE", value);
    }

    public int Alignment
    {
        get => _thumbStruct.GetInt32("ALIGNMENT") ?? 0;
        set => _thumbStruct.SetInt32("ALIGNMENT", value);
    }

    public GUISliderThumb(GFFStruct sliderStruct)
    {
        _sliderStruct = sliderStruct;
    }
}

public class GUIButton : GUIControl
{
    private readonly GFFStruct _buttonStruct;

    public GUIHilight Hilight => new(_buttonStruct);
    public GUIMoveTo MoveTo => new(_buttonStruct);
    public GUIText Text => new(_buttonStruct);

    public GUIButton(GFFStruct buttonStruct) : base(buttonStruct)
    {
        _buttonStruct = buttonStruct;
    }
}

public class GUILabel : GUIControl
{
    private readonly GFFStruct _labelStruct;

    public GUIText Text => new(_labelStruct);

    public GUILabel(GFFStruct labelStruct) : base(labelStruct)
    {
        _labelStruct = labelStruct;
    }

}

public class GUIPanel : GUIControl
{
    private readonly GFFStruct _panelStruct;

    public GUIControlList Controls => new(_panelStruct);
    public GUIBorder Border => new(_panelStruct);

    public GUIPanel(GFFStruct panelStruct) : base(panelStruct)
    {
        _panelStruct = panelStruct;
    }
}

public class GUIProtoItem : GUIControl
{
    private readonly GFFStruct _protoItemStruct;

    public GUIText Text => new(_protoItemStruct);
    public GUIBorder Border => new(_protoItemStruct);
    public GUIHilight Hilight => new(_protoItemStruct);
    public GUISelectedHilight SelectedHilight => new(_protoItemStruct);
    public GUISelected Selected => new(_protoItemStruct);

    public bool IsSelected
    {
        get => Convert.ToBoolean(_protoItemStruct.GetInt32("ISSELECTED") ?? 0);
        set => _protoItemStruct.SetInt32("ISSELECTED", Convert.ToByte(value));
    }

    public GUIProtoItem(GFFStruct protoItemStruct) : base(protoItemStruct)
    {
        _protoItemStruct = protoItemStruct;
    }
}

public class GUIControlList : IEnumerable<GUIControl>
{
    private readonly GFFStruct _struct;
    private GFFList? _list => _struct.GetList("Controls");

    public GUIControlList(GFFStruct @struct)
    {
        _struct = @struct;
    }

    public IEnumerator<GUIControl> GetEnumerator()
    {
        return _list.Select(x => new GUIControl(x)).GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return _list.Select(x => new GUIControl(x)).GetEnumerator();
    }
}
