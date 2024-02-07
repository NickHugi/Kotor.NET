using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Common.Geometry
{
    public class BoundingBox
    {
        public Vector3 Minimum { get; set; }
        public Vector3 Maximum { get; set; }
        // TODO
        public Vector3 Centre => throw new NotImplementedException();
        // plus getters for all 8 points of bounding box

        public BoundingBox(Vector3 minimum, Vector3 maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public bool Contains(Vector3 vector3, bool includeEdges = true)
        {
            // TODO
            return false;
        }
    }
}
