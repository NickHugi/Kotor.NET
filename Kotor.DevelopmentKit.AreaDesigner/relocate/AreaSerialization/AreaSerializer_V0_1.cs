using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.KitSerialization;
using Kotor.NET.Graphics.Extensions;
using Newtonsoft.Json;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaSerialization;

public class AreaSerializer_V0_1
{
    public const string FormatID = "0.1";

    public static Area Load(string filepath)
    {
        var json = File.ReadAllText(filepath);
        dynamic data = JsonConvert.DeserializeObject(json);

        var area = new Area();

        foreach (var roomData in data.rooms.ToObject<dynamic[]>())
        {
            var room = new Room(area);
            area.AddRoom(room);

            foreach (var tileData in roomData.tiles.ToObject<dynamic[]>())
            {
                var template = Kit.Manager.Get(tileData.kitID.Value).Tile(tileData.templateID.Value);
                var tile = new WorldObject(room, null, template, Guid.NewGuid(), WorldObjectType.Tile);
                tile.SwitchTemplate(template);
                tile.LocalPosition = new Vector3(tileData.position.ToObject<float[]>());
                tile.LocalOrientation = ((float[])tileData.orientation.ToObject<float[]>()).ToQuaternion();

                room.AddTile(tile);
            }
        }

        return area;
    }

    public static void Save(string filepath, Area area)
    {
        dynamic data = new ExpandoObject();

        data.format = FormatID;

        data.rooms = area.Rooms.Select(room => new
        {
            position = room.Position.ToFloatArray(),
            orientation = room.Orientation.ToFloatArray(),
            tiles = room.Objects.Where(x => x.Type == WorldObjectType.Tile).Select(tile => new
            {
                kitID = tile.KitID,
                templateID = tile.TemplateID,
                position = tile.LocalPosition.ToFloatArray(),
                orientation = tile.LocalOrientation.ToFloatArray(),
                virtalObjects = tile.AttachedObjects.Select(x => new
                {
                    // TODO
                })
            })
        });

        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(filepath, json);
    }
}
