using System.Numerics;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public enum OverlapCountType
{
    NotEqualTo,
    EqualTo,
    LessThan,
    GreaterThan,
    Ignore
}

public class MagnetTemplate
{
    public required Vector3 LocalPosition { get; init; }
    public required Quaternion LocalOrientation { get; init; }
    public Matrix4x4 LocalTransform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

    public string KitID { get; init; } = "";
    public string TemplateID { get; init; } = "";
    public WorldObjectTemplate Template => Kit.Manager.Get(KitID).Object(TemplateID);

    public bool ConditionCheckLocalMagnetsOnly
    {
        get => Template.Type switch
        {
            WorldObjectType.Floor => false,
            WorldObjectType.Ceiling => false,
            WorldObjectType.Wall => false,
            WorldObjectType.OuterCorner => false,
            WorldObjectType.InnerCorner => false,
            WorldObjectType.DoorFrame => false,
            _ => field
        };
        init;
    } = false;
    public bool ConditionMustHaveTemplate
    {
        get => Template.Type switch
        {
            WorldObjectType.Floor => true,
            WorldObjectType.Ceiling => true,
            WorldObjectType.Wall => true,
            WorldObjectType.OuterCorner => true,
            WorldObjectType.InnerCorner => true,
            WorldObjectType.DoorFrame => false,
            _ => field
        };
        init;
    } = false;
    public bool ConditionOverlapWillDisable
    {
        get => Template.Type switch
        {
            WorldObjectType.Floor => true,
            WorldObjectType.Ceiling => true,
            WorldObjectType.Wall => true,
            WorldObjectType.OuterCorner => true,
            WorldObjectType.InnerCorner => true,
            WorldObjectType.DoorFrame => true,
            _ => field
        };
        init;
    } = false;
    public int ConditionOverlapCheckCount
    {
        get => Template.Type switch
        {
            WorldObjectType.Floor => 0,
            WorldObjectType.Ceiling => 0,
            WorldObjectType.Wall => 1,
            WorldObjectType.OuterCorner => 0, // -1
            WorldObjectType.InnerCorner => 0,
            WorldObjectType.DoorFrame => 0,
            _ => field
        };
        init;
    } = 0;
    public bool ConditionOverlapOnlySameRotation
    {
        get => Template.Type switch
        {
            WorldObjectType.Floor => false,
            WorldObjectType.Ceiling => false,
            WorldObjectType.Wall => false,
            WorldObjectType.OuterCorner => false,
            WorldObjectType.InnerCorner => false,
            WorldObjectType.DoorFrame => false,
            _ => field
        };
        init;
    } = false;
    public OverlapCountType ConditionOverlapType
    {
        get => Template.Type switch
        {
            WorldObjectType.Floor => OverlapCountType.Ignore,
            WorldObjectType.Ceiling => OverlapCountType.Ignore,
            WorldObjectType.Wall => OverlapCountType.LessThan,
            WorldObjectType.OuterCorner => OverlapCountType.EqualTo,
            WorldObjectType.InnerCorner => OverlapCountType.EqualTo,
            WorldObjectType.DoorFrame => OverlapCountType.Ignore,
            _ => field
        };
        init;
    } = OverlapCountType.Ignore;
    public bool ConditionOverlapOnlyEnableFirst
    {
        get => Template.Type switch
        {
            WorldObjectType.Floor => false,
            WorldObjectType.Ceiling => false,
            WorldObjectType.Wall => false,
            WorldObjectType.OuterCorner => false,
            WorldObjectType.InnerCorner => false,
            WorldObjectType.DoorFrame => true,
            _ => field
        };
        init;
    } = false;
    public bool ConditionOverlapOnlyEnableMiddle
    {
       get => Template.Type switch
        {
            WorldObjectType.Floor => false,
            WorldObjectType.Ceiling => false,
            WorldObjectType.Wall => false,
            WorldObjectType.OuterCorner => true,
            WorldObjectType.InnerCorner => false,
            WorldObjectType.DoorFrame => false,
            _ => field
        };
        init;
    } = false;
    public bool ConditionOverlapOnlySameTemplate
    {
        get => Template.Type switch
        {
            WorldObjectType.Floor => false,
            WorldObjectType.Ceiling => false,
            WorldObjectType.Wall => false,
            WorldObjectType.OuterCorner => true,
            WorldObjectType.InnerCorner => true,
            WorldObjectType.DoorFrame => false,
            _ => field
        };
        init;
    } = false;
    public bool ConditionOverlapOnlySameClass
    {
        get => Template.Type switch
        {
            WorldObjectType.Floor => false,
            WorldObjectType.Ceiling => false,
            WorldObjectType.Wall => false,
            WorldObjectType.OuterCorner => false,
            WorldObjectType.InnerCorner => false,
            WorldObjectType.DoorFrame => false,
            _ => field
        };
        init;
    } = false;
    public bool ConditionOverlapOnlySameType
    {
        get => Template.Type switch
        {
            WorldObjectType.Floor => false,
            WorldObjectType.Ceiling => false,
            WorldObjectType.Wall => false,
            WorldObjectType.OuterCorner => false,
            WorldObjectType.InnerCorner => false,
            WorldObjectType.DoorFrame => true,
            _ => field
        };
        init;
    } = false;
    public WorldObjectType?[]? ConditionOverlapOnlySpecificTypes
    {
        get => Template.Type switch
        {
            WorldObjectType.Floor => null,
            WorldObjectType.Ceiling => null,
            WorldObjectType.Wall => null,
            WorldObjectType.OuterCorner => null,
            WorldObjectType.InnerCorner => null,
            WorldObjectType.DoorFrame => [null],
            _ => field
        };
        init;
    } = null;
}
