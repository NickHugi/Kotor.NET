using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorMDL;
using Kotor.NET.Resources.KotorMDL.Controllers;
using Kotor.NET.Resources.KotorMDL.Nodes;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaExportation;

public static class AreaExporter
{
    public static MDL RoomToMDL(Room room)
    {
        var mdl = new MDL();
        mdl.Name = "test";

        foreach (var tile in room.Tiles)
        {
            mdl.Root.Children.Add(FloorToMDLNode(tile.Floor));
            //mdl.Root.Children.Add(CeilingToMDLNode(tile.Ceiling));
            mdl.Root.Children.AddRange(tile.Walls.Where(x => x.Visible).Select(WallToMDLNode));
            mdl.Root.Children.AddRange(tile.Walls.Select(x => x.DoorFrame).Where(x => x?.Visible == true).Select(DoorFrameToMDLNode));
            mdl.Root.Children.AddRange(tile.InnerCorners.Where(x => x.Visible == true).Select(InnerCornerToMDLNode));
        }

        mdl.Root.GetAllDescendants().Select((x, i) => x.Name = i.ToString()).ToArray();
        mdl.RedoNodeNumbers();
        return mdl;
    }

    private static MDLNode FloorToMDLNode(Floor floor)
    {
        var floorMDL = MDL.FromFile($"{Kit.Manager.ActiveDirectory}/{floor.KitID}/{floor.Template.Model}.mdl");
        floorMDL.Root.GetController<MDLControllerDataPosition>().AddLinear(0, new(floor.Position));
        floorMDL.Root.GetController<MDLControllerDataOrientation>().AddLinear(0, new(floor.Orientation));
        return floorMDL.Root;
    }

    private static MDLNode CeilingToMDLNode(Ceiling ceiling)
    {
        var ceilingMDL = MDL.FromFile($"{Kit.Manager.ActiveDirectory}/{ceiling.KitID}/{ceiling.Template.Model}.mdl");
        ceilingMDL.Root.GetController<MDLControllerDataPosition>().AddLinear(0, new(ceiling.Position));
        ceilingMDL.Root.GetController<MDLControllerDataOrientation>().AddLinear(0, new(ceiling.Orientation));
        return ceilingMDL.Root;
    }

    private static MDLNode WallToMDLNode(Wall wall)
    {
        var wallMDL = MDL.FromFile($"{Kit.Manager.ActiveDirectory}/{wall.KitID}/{wall.Template.Model}.mdl");
        wallMDL.Root.GetController<MDLControllerDataPosition>().AddLinear(0, new(wall.Position));
        wallMDL.Root.GetController<MDLControllerDataOrientation>().AddLinear(0, new(wall.Orientation));
        return wallMDL.Root;
    }

    private static MDLNode DoorFrameToMDLNode(DoorFrame doorframe)
    {
        var doorframeMDL = MDL.FromFile($"{Kit.Manager.ActiveDirectory}/{doorframe.KitID}/{doorframe.Template.Model}.mdl");
        doorframeMDL.Root.GetController<MDLControllerDataPosition>().AddLinear(0, new(doorframe.Position));
        doorframeMDL.Root.GetController<MDLControllerDataOrientation>().AddLinear(0, new(doorframe.Orientation));
        return doorframeMDL.Root;
    }

    private static MDLNode InnerCornerToMDLNode(InnerCorner corner)
    {
        var cornerMDL = MDL.FromFile($"{Kit.Manager.ActiveDirectory}/{corner.KitID}/{corner.Template.Model}.mdl");
        cornerMDL.Root.GetController<MDLControllerDataPosition>().AddLinear(0, new(corner.Position));
        cornerMDL.Root.GetController<MDLControllerDataOrientation>().AddLinear(0, new(corner.Orientation));
        return cornerMDL.Root;
    }
}
