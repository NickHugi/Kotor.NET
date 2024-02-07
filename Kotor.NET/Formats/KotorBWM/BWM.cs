using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Geometry;
using Vector3 = Kotor.NET.Common.Geometry.Vector3;

namespace Kotor.NET.Formats.KotorBWM
{
    public class BWM
    {
        public BWMType Type { get; set; }
        public Vector3 Position { get; set; }
        public List<BWMFace> Faces { get; set; }

        public IEnumerable<BWMFace> CalculateWalkableFaces()
        {
            return Faces.Where(x => x.Material.IsWalkable()).ToArray();
        }

        public IEnumerable<BWMFace> CalculateUnWalkableFaces()
        {
            return Faces.Where(x => !x.Material.IsWalkable()).ToArray();
        }

        public IEnumerable<BWMAABB> CalculateAABBs()
        {
            // TODO
            return null;
        }

        public IEnumerable<BWMEdge> CalculateEdges()
        {
            // TODO
            return null;
        }

        public BoundingBox CalculateBoundingBox()
        {
            // TODO
            return null;
        }

        public IEnumerable<Vector3> GetAllVertices()
        {
            return Faces.SelectMany(x => x.Vertices).ToArray();
        }

        public IEnumerable<BWMFace> FindAdjacencies()
        {
            // TODO
            return null;
        }

        public IEnumerable<BWMFace> FindFace(bool walkableOnly)
        {
            // TODO
            return null;
        }

        public void PerformTranslation(float x, float y, float z)
        {
            // TODO
        }

        public void PerformRotation(float yaw)
        {
            // TODO
        }

        public void PerformReflection(bool X, bool Y)
        {
            // TODO
        }

        public void SwitchRoomLink(int oldIndex, int newIndex)
        {
            // TODO
        }
    }

    public enum BWMType
    {
        PlaceableOrDoor = 0,
        Area = 1,
    }

    public class BWMFace
    {
        public SurfaceMaterial Material { get; set; }
        public Vector3 Normal { get; set; } = new();
        public BWMFace[] Adjacencies { get; } = new BWMFace[3];
        public Vector3[] Vertices { get; } = new Vector3[3];
    }

    public class BWMEdge
    {
        public int Transition { get; set; } = -1;
    }

    public class BWMAABB
    {

    }
}
