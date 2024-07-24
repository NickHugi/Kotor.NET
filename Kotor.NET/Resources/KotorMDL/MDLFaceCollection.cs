using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.KotorMDL.Nodes;

namespace Kotor.NET.Resources.KotorMDL;

public class MDLFaceCollection(MDLTrimeshNode node) : ICollection<MDLFace>
{
    private MDLTrimeshNode _node = node;
    private List<MDLFace> _faces = new();

    public MDLFace this[int i] => _faces[i];

    public void Add(MDLVertex vertex1, MDLVertex vertex2, MDLVertex vertex3)
    {
        _faces.Add(new()
        {
            Vertex1 = SanitizeVertex(vertex1),
            Vertex2 = SanitizeVertex(vertex2),
            Vertex3 = SanitizeVertex(vertex3),
        });
    }
    public void AddRange(IEnumerable<MDLFace> faces)
    {
        foreach (var face in faces)
        {
            Add(face);
        }
    }
    public void RemoveAt(int index)
    {
        _faces.RemoveAt(index);
    }

    #region ICollection
    public int Count => _faces.Count();
    public bool IsReadOnly => false;
    public void Add(MDLFace face)
    {
        _faces.Add(new()
        {
            Vertex1 = face.Vertex1,
            Vertex2 = face.Vertex2,
            Vertex3 = face.Vertex3,
        });
    }
    public void Clear()
    {
        _faces.Clear();
    }
    public bool Contains(MDLFace face)
    {
        return _faces.Contains(face);
    }
    public void CopyTo(MDLFace[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }
    public bool Remove(MDLFace face)
    {
        return _faces.Remove(face);
    }
    public IEnumerator<MDLFace> GetEnumerator()
    {
        return _faces.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return _faces.GetEnumerator();
    }
    #endregion

    private MDLVertex SanitizeVertex(MDLVertex vertex)
    {
        return new MDLVertex()
        {
            _position = (vertex._position is null) ? (_node.HasPositions() ? new() : null) : (_node.HasPositions() ? vertex.Position : null),
            _normal = (vertex._normal is null) ? (_node.HasNormals() ? new() : null) : (_node.HasNormals() ? vertex.Normal : null),
            _diffuseUV = (vertex._diffuseUV is null) ? (_node.HasDiffuseUVs() ? new() : null) : (_node.HasDiffuseUVs() ? vertex.DiffuseUV : null),
            _lightmapUV = (vertex._lightmapUV is null) ? (_node.HasLightmapUVs() ? new() : null) : (_node.HasLightmapUVs() ? vertex.LightmapUV : null),
            _tangent1 = (vertex._tangent1 is null) ? (_node.HasTangents() ? new() : null) : (_node.HasTangents() ? vertex.Tangent1 : null),
            _tangent2 = (vertex._tangent2 is null) ? (_node.HasTangents() ? new() : null) : (_node.HasTangents() ? vertex.Tangent2 : null),
            _tangent3 = (vertex._tangent3 is null) ? (_node.HasTangents() ? new() : null) : (_node.HasTangents() ? vertex.Tangent3 : null),
            _tangent4 = (vertex._tangent4 is null) ? (_node.HasTangents() ? new() : null) : (_node.HasTangents() ? vertex.Tangent4 : null),
            _saber = (vertex._saber is null) ? (_node is MDLSaberNode ? new() : null) : (_node is MDLSaberNode ? vertex.Saber : null),
            _dangly = (vertex._dangly is null) ? (_node is MDLDanglyNode ? new() : null) : (_node is MDLDanglyNode ? vertex.Dangly : null),
            _skin = (vertex._skin is null) ? (_node is MDLSkinNode ? new() : null) : (_node is MDLSkinNode ? vertex.Skin : null),
        };
    }
}
