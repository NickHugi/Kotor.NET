using System.Numerics;

namespace Kotor.NET.Graphics.Extensions;

public static class QuaternionExtensions
{
    public static Quaternion UncompressQuaternion(this float value)
    {
        var data = BitConverter.GetBytes(value);
        return data.UncompressQuaternion();
    }
    public static Quaternion UncompressQuaternion(this byte[] data)
    {
        var temp = BitConverter.ToInt32(data);
        var tmpQuat = new Quaternion();

        var QUAT_X_MASK = 0x07ff;        // 11 bits for X component
        var QUAT_Y_MASK = 0x07ff;        // 11 bits for Y component  
        var QUAT_Z_MASK = 0x3FF;         // 10 bits for Z component
        var QUAT_X_SCALE = 1.0 / 1023.0; // Scale factor for X,Y
        var QUAT_Z_SCALE = 1.0 / 511.0;  // Scale factor for Z
        var QUAT_Y_SHIFT = 11;           // Y component bit shift
        var QUAT_Z_SHIFT = 22;           // Z component bit shift

        var x = (temp & QUAT_X_MASK) * QUAT_X_SCALE - 1.0;
        var y = (temp >> QUAT_Y_SHIFT & QUAT_Y_MASK) * QUAT_X_SCALE - 1.0;
        var z = (temp >> QUAT_Z_SHIFT & QUAT_Z_MASK) * QUAT_Z_SCALE - 1.0;

        var fSquares = x * x + y * y + z * z;

        // Early exit for identity quaternion (all components near zero)
        if (fSquares < 1e-10)
        {
            tmpQuat =  new(0, 0, 0, 1);
        }
        else if (fSquares < 1.0)
        {
            tmpQuat = new((float)x, (float)y, (float)z, (float)Math.Sqrt(1.0 - fSquares));
        }
        else
        {
            // Normalize the vector to unit length instead of setting w=0
            var invLength = 1.0 / Math.Sqrt(fSquares);
            tmpQuat = new((float)(x * invLength), (float)(y * invLength), (float)(z * invLength), 0);
        }
        return Quaternion.Normalize(tmpQuat);
    }
}
