using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapBuilder.Data
{
    public class TerrainData
    {
        public uint Width => _width;
        public uint Length => _length;
        public float[,] Height
        {
            get
            {
                var height = new float[Width, Length];
                for (var x = 0; x < Width; x++)
                for (var y = 0; y < Length; y++)
                {
                    height[x, y] = (float)new Random().NextDouble() / 2;
                }
                return height;
            }
        }

        
        private uint _width;
        private uint _length;
        private float[] _height;

        public TerrainData(uint width, uint length)
        {
            _width = width;
            _length = length;
            _height = new float[width * length];
        }
    }
}
