using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace Kotor.NET.PatchingLanguage.Visitor;


public partial class KotorPatchingLanguageVisitor : KotorPatchingLanguageBaseVisitor<object>
{
    public override object VisitEquipmentSlot_Implant([NotNull] KotorPatchingLanguageParser.EquipmentSlot_ImplantContext context)
    {
        return 9;
    }
    public override object VisitEquipmentSlot_LeftUtility([NotNull] KotorPatchingLanguageParser.EquipmentSlot_LeftUtilityContext context)
    {
        return 9;
    }
    public override object VisitEquipmentSlot_Head([NotNull] KotorPatchingLanguageParser.EquipmentSlot_HeadContext context)
    {
        return 0;
    }
    public override object VisitEquipmentSlot_Sensor([NotNull] KotorPatchingLanguageParser.EquipmentSlot_SensorContext context)
    {
        return 0;
    }
    public override object VisitEquipmentSlot_Hands([NotNull] KotorPatchingLanguageParser.EquipmentSlot_HandsContext context)
    {
        return 3;
    }
    public override object VisitEquipmentSlot_RightUtility([NotNull] KotorPatchingLanguageParser.EquipmentSlot_RightUtilityContext context)
    {
        return 3;
    }
    public override object VisitEquipmentSlot_LeftArm([NotNull] KotorPatchingLanguageParser.EquipmentSlot_LeftArmContext context)
    {
        return 5;
    }
    public override object VisitEquipmentSlot_LeftSpecialWeapon([NotNull] KotorPatchingLanguageParser.EquipmentSlot_LeftSpecialWeaponContext context)
    {
        return 5;
    }
    public override object VisitEquipmentSlot_Body([NotNull] KotorPatchingLanguageParser.EquipmentSlot_BodyContext context)
    {
        return 1;
    }
    public override object VisitEquipmentSlot_Plating([NotNull] KotorPatchingLanguageParser.EquipmentSlot_PlatingContext context)
    {
        return 1;
    }
    public override object VisitEquipmentSlot_RightArm([NotNull] KotorPatchingLanguageParser.EquipmentSlot_RightArmContext context)
    {
        return 7;
    }
    public override object VisitEquipmentSlot_RightSpecialWeapon([NotNull] KotorPatchingLanguageParser.EquipmentSlot_RightSpecialWeaponContext context)
    {
        return 7;
    }
    public override object VisitEquipmentSlot_LeftWeapon([NotNull] KotorPatchingLanguageParser.EquipmentSlot_LeftWeaponContext context)
    {
        return 5;
    }
    public override object VisitEquipmentSlot_Belt([NotNull] KotorPatchingLanguageParser.EquipmentSlot_BeltContext context)
    {
        return 10;
    }
    public override object VisitEquipmentSlot_Shield([NotNull] KotorPatchingLanguageParser.EquipmentSlot_ShieldContext context)
    {
        return 10;
    }
    public override object VisitEquipmentSlot_RightWeapon([NotNull] KotorPatchingLanguageParser.EquipmentSlot_RightWeaponContext context)
    {
        return 4;
    }
    public override object VisitEquipmentSlot_FirstClaw([NotNull] KotorPatchingLanguageParser.EquipmentSlot_FirstClawContext context)
    {
        return 14;
    }
    public override object VisitEquipmentSlot_SecondClaw([NotNull] KotorPatchingLanguageParser.EquipmentSlot_SecondClawContext context)
    {
        return 15;
    }
    public override object VisitEquipmentSlot_ThirdClaw([NotNull] KotorPatchingLanguageParser.EquipmentSlot_ThirdClawContext context)
    {
        return 16;
    }
    public override object VisitEquipmentSlot_Hide([NotNull] KotorPatchingLanguageParser.EquipmentSlot_HideContext context)
    {
        return 17;
    }
    public override object VisitEquipmentSlot_AltLeftWeapon([NotNull] KotorPatchingLanguageParser.EquipmentSlot_AltLeftWeaponContext context)
    {
        return 19;
    }
    public override object VisitEquipmentSlot_AltRight_Weapon([NotNull] KotorPatchingLanguageParser.EquipmentSlot_AltRight_WeaponContext context)
    {
        return 18;
    }
}
