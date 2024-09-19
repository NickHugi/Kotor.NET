using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.AsciiLYT;

public class LYTAsciiLayout
{
    public List<LYTAsciiRoom> Rooms { get; set; } = new();
    public List<LYTAsciiTrack> Tracks { get; set; } = new();
    public List<LYTAsciiObstacle> Obstacles { get; set; } = new();
    public List<LYTAsciiDoorHook> DoorHooks { get; set; } = new();

    public LYTAsciiLayout()
    {
    }
    public LYTAsciiLayout(StreamReader reader)
    {
        while (true)
        {
            var line = reader.ReadLine();
            var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var command = tokens.FirstOrDefault()?.ToLower();

            if (command is null)
                continue;
            if (command == "donelayout")
                break;

            var count = int.Parse(tokens.ElementAt(1));
            var range = Enumerable.Range(0, count).ToList();

            if (command == "roomcount")
                range.ForEach(x => Rooms.Add(new LYTAsciiRoom(reader)));
            if (command == "trackcount")
                range.ForEach(x => Tracks.Add(new LYTAsciiTrack(reader)));
            if (command == "obstaclecount")
                range.ForEach(x => Obstacles.Add(new LYTAsciiObstacle(reader)));
            if (command == "doorhookcount")
                range.ForEach(x => DoorHooks.Add(new LYTAsciiDoorHook(reader)));
        }
    }

    public void Write(StreamWriter writer)
    {
        writer.WriteLine($"beginlayout");

        writer.WriteLine($"   roomcount {Rooms.Count}");
        Rooms.ForEach(room => room.Write(writer));

        writer.WriteLine($"   trackcount {Tracks.Count}");
        Tracks.ForEach(track => track.Write(writer));

        writer.WriteLine($"   obstaclecount {Obstacles.Count}");
        Obstacles.ForEach(obstacle => obstacle.Write(writer));

        writer.WriteLine($"   doorhookcount {DoorHooks.Count}");
        DoorHooks.ForEach(doorhook => doorhook.Write(writer));

        writer.WriteLine($"donelayout");
    }
}
