using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryMDL;

public enum MDLBinaryControllerType
{
    // Simple Node
    Position = 8,
    Orientation = 20,
    Scale = 36,

    // Mesh
    SelfIlluminationColour = 100,
    Alpha = 132,

    // Light
    Colour = 76,
    Radius = 88,
    ShadowRadius = 96,
    VerticalDisplacement = 100,
    Multiplier = 140,

    // Emitter
    AlphaEnd = 80,
    AlphaStart = 84,
    BirthRate = 88,
    BounceCo = 92,
    CombineTime = 96,
    Drag = 100,
    FPS = 104,
    FrameEnd = 108,
    FrameStart = 112,
    Gravity = 116,
    LifeExpectancy = 120,
    Mass = 124,
    P2PBezier2 = 128,
    P2PBezier3 = 132,
    ParticleRotation = 136,
    RandomVelocity = 140,
    SizeStart = 144,
    SizeEnd = 148,
    SizeStartY = 152,
    SizeEndY = 156,
    Spread = 160,
    Threshold = 164,
    Velocity = 168,
    XSize = 172,
    YSize = 176,
    BlurLength = 180,
    LightningDelay = 184,
    LightningRadius = 188,
    LightningScale = 192,
    LightningSubdivide = 196,
    LightningZigZag = 200,
    AlphaMid = 216,
    PercentStart = 220,
    PercentMid = 224,
    PercentEnd = 228,
    SizeMid = 232,
    SizeMidY = 236,
    RandomBirthRate = 240,
    TargetSize = 252,
    NumberOfControlPoints = 256,
    ControlPointRadius = 260,
    ControlPointDelay = 264,
    TangentSpread = 268,
    TangentLength = 272,
    ColorMid = 284,
    ColorEnd = 380,
    ColorStart = 392,
    EmitterDetonate = 502,
}
