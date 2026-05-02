namespace Kotor.NET.Common.Data.Geometry;

public enum SurfaceMaterial
{
    NotDefined = 0,
    Dirt = 1,
    Obscuring = 2,
    Grass = 3,
    Stone = 4,
    Wood = 5,
    Water = 6,
    Nonwalk = 7,
    Transparent = 8,
    Carpet = 9,
    Metal = 10,
    Puddles = 11,
    Swamp = 12,
    Mud = 13,
    Leaves = 14,
    Lava = 15,
    BottomlessPit = 16,
    DeepWater = 17,
    Door = 18,
    NonWalkGrass = 19,
    Trigger = 30,
}

public static class SurfaceMaterialExtension
{
    public static bool IsWalkable(this SurfaceMaterial material)
    {
        return material switch
        {
            SurfaceMaterial.Dirt => true,
            SurfaceMaterial.Grass => true,
            SurfaceMaterial.Stone => true,
            SurfaceMaterial.Wood => true,
            SurfaceMaterial.Water => true,
            SurfaceMaterial.Carpet => true,
            SurfaceMaterial.Metal => true,
            SurfaceMaterial.Puddles => true,
            SurfaceMaterial.Swamp => true,
            SurfaceMaterial.Mud => true,
            SurfaceMaterial.Leaves => true,
            SurfaceMaterial.BottomlessPit => true,
            SurfaceMaterial.Door => true,
            SurfaceMaterial.Trigger => true,
            _ => false
        };
    }
}
