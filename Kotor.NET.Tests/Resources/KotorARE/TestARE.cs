using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xunit;
using Kotor.NET.Common.Data;
using Kotor.NET.Resources.KotorARE;

namespace Kotor.NET.Tests.Resources.KotorARE;


public class TestARE
{
    public static readonly string File1Filepath = "Resources/KotorARE/file1.are";

    [Fact]
    public void Getters()
    {
        // Setup
        var are = ARE.FromFile(File1Filepath);

        // Assert
        Assert.Equal(0.2f, are.AlphaTest);
        Assert.Equal(0, are.CameraStyle);
        Assert.Equal(0, are.ChanceLightning);
        Assert.Equal(0, are.ChanceRain);
        Assert.Equal(0, are.ChanceSnow);
        Assert.Equal("", are.Comments);
        Assert.Equal(0, are.CreatorID);
        Assert.Equal("", are.DefaultEnvironmentMap);
        Assert.Equal(0, are.DirtyARGBOne);
        Assert.Equal(0, are.DirtyARGBTwo);
        Assert.Equal(0, are.DirtyARGBThree);
        Assert.Equal(0, are.DirtyFormulaOne);
        Assert.Equal(0, are.DirtyFormulaTwo);
        Assert.Equal(0, are.DirtyFormulaThree);
        Assert.Equal(0, are.DirtyFuncOne);
        Assert.Equal(0, are.DirtyFuncTwo);
        Assert.Equal(0, are.DirtyFuncThree);
        Assert.Equal(4, are.DirtySizeOne);
        Assert.Equal(4, are.DirtySizeTwo);
        Assert.Equal(4, are.DirtySizeThree);
        Assert.False(are.DisableTransit);
        Assert.Equal(4407092U, are.DynAmbientColor);
        Assert.Equal(0u, are.Flags);
        Assert.Equal(16777215U, are.GrassAmbient);
        Assert.Equal(0.75f, are.GrassDensity);
        Assert.Equal(16777215U, are.GrassDiffuse);
        Assert.Equal(3099206U, are.GrassEmissive);
        Assert.Equal(0.01999f, are.GrassProbLL, 0.0001f);
        Assert.Equal(0.44999f, are.GrassProbLR, 0.0001f);
        Assert.Equal(0.07999f, are.GrassProbUL, 0.0001f);
        Assert.Equal(0.44999f, are.GrassProbUR, 0.0001f);
        Assert.Equal(1.25f, are.GrassQuadSize);
        Assert.Equal("grass", are.GrassTexture);
        Assert.Equal("grass", are.GrassTexture);
        Assert.Equal(102162, are.Name.StringRef);
        Assert.Equal("", are.OnEnter);
        Assert.Equal("", are.OnExit);
        Assert.Equal("", are.OnHeartbeat);
        Assert.Equal("", are.OnUserDefined);
        Assert.False(are.PlayerOnly);
        Assert.Equal(205, are.ShadowOpacity);
        Assert.False(are.StealthXPEnabled);
        Assert.Equal(0U, are.StealthXPLoss);
        Assert.Equal(0U, are.StealthXPMax);
        Assert.Equal(0U, are.SunAmbientColour);
        Assert.Equal(0U, are.SunDiffuseColour);
        Assert.Equal(5062207U, are.SunFogColour);
        Assert.Equal(200U, are.SunFogFar);
        Assert.Equal(25U, are.SunFogNear);
        Assert.True(are.SunFogOn);
        Assert.False(are.SunShadows);
        Assert.Equal("KhoondaPlains", are.Tag);
        Assert.False(are.Unescapable);
        Assert.Equal(1, are.WindPower);

        Assert.Equal(0.49399f, are.Map.MapPoint1X, 0.001f);
        Assert.Equal(0.17399f, are.Map.MapPoint1Y, 0.001f);
        Assert.Equal(0.85100f, are.Map.MapPoint2X, 0.001f);
        Assert.Equal(0.68300f, are.Map.MapPoint2Y, 0.001f);
        Assert.Equal(299.390f, are.Map.WorldPoint1X, 0.001f);
        Assert.Equal(229.229f, are.Map.WorldPoint1Y, 0.001f);
        Assert.Equal(540.500f, are.Map.WorldPoint2X, 0.001f);
        Assert.Equal(56.0099f, are.Map.WorldPoint2Y, 0.001f);
        Assert.Equal(24, are.Map.MapResX);
        Assert.Equal(1, are.Map.MapZoom);
        Assert.Equal(0, are.Map.NorthAxis);

        Assert.Equal(3, are.Rooms.Count);
        var room1 = are.Rooms.ElementAt(1);
        Assert.Equal(1, room1.AmbientScale);
        Assert.False(room1.DisableWeather);
        Assert.Equal(0, room1.EnvironmentAudio);
        Assert.Equal(20, room1.ForceRating);
        Assert.Equal("601danf", room1.RoomName);
    }

    [Fact]
    public void Setters()
    {
        // Setup
        var are = ARE.FromFile(File1Filepath);
        are.AlphaTest = 1;
        are.CameraStyle = 2;
        are.ChanceLightning = 3;
        are.ChanceRain = 4;
        are.ChanceSnow = 5;
        are.Comments = "comment";
        are.CreatorID = 6;
        are.DefaultEnvironmentMap = "defaultenvmap";
        are.DirtyARGBOne = 7;
        are.DirtyARGBTwo = 8;
        are.DirtyARGBThree = 9;
        are.DirtyFormulaOne = 10;
        are.DirtyFormulaTwo = 11;
        are.DirtyFormulaThree = 12;
        are.DirtyFuncOne = 13;
        are.DirtyFuncTwo = 14;
        are.DirtyFuncThree = 15;
        are.DirtySizeOne = 16;
        are.DirtySizeTwo = 17;
        are.DirtySizeThree = 18;
        are.DisableTransit = true;
        are.DynAmbientColor = 19;
        are.Flags = 20;
        are.GrassAmbient = 21;
        are.GrassDensity = 22;
        are.GrassDiffuse = 23;
        are.GrassEmissive = 24;
        are.GrassProbLL = 25;
        are.GrassProbLR = 26;
        are.GrassProbUL = 27;
        are.GrassProbUR = 28;
        are.GrassQuadSize = 29;
        are.GrassTexture = "grasstexture";
        are.Name.StringRef = 30;
        are.OnEnter = "onenter";
        are.OnExit = "onexit";
        are.OnHeartbeat = "onheartbeat";
        are.OnUserDefined = "onuserdefined";
        are.PlayerOnly = true;
        are.ShadowOpacity = 31;
        are.StealthXPEnabled = true;
        are.StealthXPLoss = 32;
        are.StealthXPMax = 33;
        are.SunAmbientColour = 34;
        are.SunDiffuseColour = 35;
        are.SunFogColour = 36;
        are.SunFogFar = 37;
        are.SunFogNear = 38;
        are.SunFogOn = true;
        are.SunShadows = true;
        are.Tag = "tag";
        are.Unescapable = true;
        are.WindPower = 39;

        are.Map.MapPoint1X = 1;
        are.Map.MapPoint1Y = 2;
        are.Map.MapPoint2X = 3;
        are.Map.MapPoint2Y = 4;
        are.Map.WorldPoint1X = 5;
        are.Map.WorldPoint1Y = 6;
        are.Map.WorldPoint2X = 7;
        are.Map.WorldPoint2Y = 8;
        are.Map.MapZoom = 9;
        are.Map.MapResX = 10;
        are.Map.NorthAxis = 11;

        var room1 = are.Rooms.ElementAt(1);
        room1.AmbientScale = 1;
        room1.DisableWeather = true;
        room1.EnvironmentAudio = 2;
        room1.ForceRating = 3;
        room1.RoomName = "roomname";

        // Assert
        Assert.Equal(1, are.AlphaTest);
        Assert.Equal(2, are.CameraStyle);
        Assert.Equal(3, are.ChanceLightning);
        Assert.Equal(4, are.ChanceRain);
        Assert.Equal(5, are.ChanceSnow);
        Assert.Equal("comment", are.Comments);
        Assert.Equal(6, are.CreatorID);
        Assert.Equal("defaultenvmap", are.DefaultEnvironmentMap);
        Assert.Equal(7, are.DirtyARGBOne);
        Assert.Equal(8, are.DirtyARGBTwo);
        Assert.Equal(9, are.DirtyARGBThree);
        Assert.Equal(10, are.DirtyFormulaOne);
        Assert.Equal(11, are.DirtyFormulaTwo);
        Assert.Equal(12, are.DirtyFormulaThree);
        Assert.Equal(13, are.DirtyFuncOne);
        Assert.Equal(14, are.DirtyFuncTwo);
        Assert.Equal(15, are.DirtyFuncThree);
        Assert.Equal(16, are.DirtySizeOne);
        Assert.Equal(17, are.DirtySizeTwo);
        Assert.Equal(18, are.DirtySizeThree);
        Assert.True(are.DisableTransit);
        Assert.Equal(19U, are.DynAmbientColor);
        Assert.Equal(20U, are.Flags);
        Assert.Equal(21U, are.GrassAmbient);
        Assert.Equal(22, are.GrassDensity);
        Assert.Equal(23U, are.GrassDiffuse);
        Assert.Equal(24U, are.GrassEmissive);
        Assert.Equal(25, are.GrassProbLL);
        Assert.Equal(26, are.GrassProbLR);
        Assert.Equal(27, are.GrassProbUL);
        Assert.Equal(28, are.GrassProbUR);
        Assert.Equal(29, are.GrassQuadSize);
        Assert.Equal("grasstexture", are.GrassTexture);
        Assert.Equal(30, are.Name.StringRef);
        Assert.Equal("onenter", are.OnEnter);
        Assert.Equal("onexit", are.OnExit);
        Assert.Equal("onheartbeat", are.OnHeartbeat);
        Assert.Equal("onuserdefined", are.OnUserDefined);
        Assert.True(are.PlayerOnly);
        Assert.Equal(31, are.ShadowOpacity);
        Assert.True(are.StealthXPEnabled);
        Assert.Equal(32U, are.StealthXPLoss);
        Assert.Equal(33U, are.StealthXPMax);
        Assert.Equal(34U, are.SunAmbientColour);
        Assert.Equal(35U, are.SunDiffuseColour);
        Assert.Equal(36U, are.SunFogColour);
        Assert.Equal(37, are.SunFogFar);
        Assert.Equal(38, are.SunFogNear);
        Assert.True(are.SunFogOn);
        Assert.True(are.SunShadows);
        Assert.Equal("tag", are.Tag);
        Assert.True(are.Unescapable);
        Assert.Equal(39, are.WindPower);

        Assert.Equal(1, are.Map.MapPoint1X);
        Assert.Equal(2, are.Map.MapPoint1Y);
        Assert.Equal(3, are.Map.MapPoint2X);
        Assert.Equal(4, are.Map.MapPoint2Y);
        Assert.Equal(5, are.Map.WorldPoint1X);
        Assert.Equal(6, are.Map.WorldPoint1Y);
        Assert.Equal(7, are.Map.WorldPoint2X);
        Assert.Equal(8, are.Map.WorldPoint2Y);
        Assert.Equal(9, are.Map.MapZoom);
        Assert.Equal(10, are.Map.MapResX);
        Assert.Equal(11, are.Map.NorthAxis);

        room1 = are.Rooms.ElementAt(1);
        Assert.Equal(1, room1.AmbientScale);
        Assert.True(room1.DisableWeather);
        Assert.Equal(2, room1.EnvironmentAudio);
        Assert.Equal(3, room1.ForceRating);
        Assert.Equal("roomname", room1.RoomName);
    }

    [Fact]
    public void RemoveRoom()
    {
        // Arrange
        var are = ARE.FromFile(File1Filepath);

        // Act
        are.Rooms[1].Remove();

        // Assert
        Assert.Equal(2, are.Rooms.Count);

        var room1 = are.Rooms.ElementAt(1);
        Assert.Equal(1, room1.AmbientScale);
        Assert.False(room1.DisableWeather);
        Assert.Equal(0, room1.EnvironmentAudio);
        Assert.Equal(20, room1.ForceRating);
        Assert.Equal("601danc", room1.RoomName);
    }

    [Fact]
    public void AddRoom()
    {
        // Arrange
        var are = new ARE();

        // Act
        are.Rooms.Add("room", 1, 2, 3, true);

        // Assert
        Assert.Single(are.Rooms);

        var room0 = are.Rooms.ElementAt(0);
        Assert.Equal(3, room0.AmbientScale);
        Assert.True(room0.DisableWeather);
        Assert.Equal(1, room0.EnvironmentAudio);
        Assert.Equal(2, room0.ForceRating);
        Assert.Equal("room", room0.RoomName);
    }
}
