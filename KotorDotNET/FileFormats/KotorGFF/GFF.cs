using KotorDotNET.Common.Conversation;
using KotorDotNET.Common.Data;
using KotorDotNET.Common.Geometry;
using KotorDotNET.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotorDotNET.FileFormats.KotorGFF
{
    /// <summary>
    /// Represents the tree data structure of the GFF file format used by
    /// the KotOR game engine.
    /// </summary>
    public class GFF
    {
        public GFFStruct Root { get; set; } = new();

        /// <summary>
        /// Flattens and returns all structs in the GFF tree.
        /// </summary>
        /// <returns>A flat list of all structs.</returns>
        public IEnumerable<GFFStruct> AllStructs()
        {
            var search = new List<GFFStruct> { Root };
            var found = new List<GFFStruct>();

            while (search.Count > 0)
            {
                var take = search.ElementAt(0);
                search.RemoveAt(0);
                found.Add(take);

                var lists = take.Fields.Where(x => x.Type == GFFFieldType.List);
                var structs = take.Fields.Where(x => x.Type == GFFFieldType.Struct);

                foreach (var list in lists)
                {
                    search.AddRange(take.Get<GFFList>(list.Label)!);
                }

                foreach (var @struct in structs)
                {
                    search.Add(take.Get<GFFStruct>(@struct.Label)!);
                }
            }

            return found.ToList();
        }

        /// <summary>
        /// Returns a list of all fields in the GFF tree.
        /// </summary>
        /// <returns>A list of all fields.</returns>
        public List<GFFField> AllFields()
        {
            var found = new List<GFFField>();
            var allStructs = AllStructs();

            foreach (var @struct in allStructs)
            {
                found.AddRange(@struct.Fields);
            }

            return found;
        }
    }

    public class GFFStruct
    {
        public uint ID { get; set; }
        public List<GFFField> Fields { get; private set; }

        public GFFStruct()
        {
            ID = 0;
            Fields = new List<GFFField>();
        }

        public GFFStruct(uint id)
        {
            ID = id;
            Fields = new List<GFFField>();
        }

        public GFFField Get(string label)
        {
            return Fields.Single(x => x.Label == label);
        }

        public T Get<T>(string label, T fallback)
        {
            var field = Fields.SingleOrDefault(x => x.Label == label);

            return (T)field!.Value ?? fallback;
        }

        public T? Get<T>(string label)
        {
            var field = Fields.SingleOrDefault(x => x.Label == label);

            return (T)field!.Value;
        }

        public void Set<T>(string label, T value)
        {
            Fields.RemoveAll(x => x.Label == label);
            Fields.Add(new GFFField(label, value));
        }

        public bool Has(string label)
        {
            return (Fields.FirstOrDefault(x => x.Label == label) != null) ? true : false;
        }
    }

    public class GFFField
    {
        public static readonly IReadOnlyDictionary<GFFFieldType, Type> TYPE_MAPPING = new Dictionary<GFFFieldType, Type>()
        {
            [GFFFieldType.UInt8] = typeof(Byte),
            [GFFFieldType.Int8] = typeof(SByte),
            [GFFFieldType.UInt16] = typeof(UInt16),
            [GFFFieldType.Int16] = typeof(Int16),
            [GFFFieldType.UInt32] = typeof(UInt32),
            [GFFFieldType.Int32] = typeof(Int32),
            [GFFFieldType.UInt64] = typeof(UInt64),
            [GFFFieldType.Int64] = typeof(Int64),
            [GFFFieldType.Single] = typeof(Single),
            [GFFFieldType.Double] = typeof(Double),
            [GFFFieldType.String] = typeof(String),
            [GFFFieldType.ResRef] = typeof(ResRef),
            [GFFFieldType.LocalizedString] = typeof(LocalizedString),
            [GFFFieldType.Binary] = typeof(Byte[]),
            [GFFFieldType.Struct] = typeof(GFFStruct),
            [GFFFieldType.List] = typeof(GFFList),
            [GFFFieldType.Vector3] = typeof(Vector3),
            [GFFFieldType.Vector4] = typeof(Vector4),
        };

        private object _value;

        public string Label { get; set; }
        public object Value
        {
            get => _value;
            set
            {
                _value = value;
            }
        }
        public GFFFieldType Type { get => TYPE_MAPPING.FirstOrDefault(x => x.Value == Value.GetType()).Key; }

        public GFFField(string label, object value)
        {
            Label = label;
            Value = _value = value;
        }
    }

    public class GFFList : ICollection<GFFStruct>
    {
        private List<GFFStruct> _list = new();

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public void Add(GFFStruct item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(GFFStruct item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(GFFStruct[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<GFFStruct> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public bool Remove(GFFStruct item)
        {
            return _list.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public GFFStruct Get(int index)
        {
            var element = _list.ElementAtOrDefault(index);

            if (element == null)
            {
                return new();
            }
            else
            {
                return element;
            }
        }
    }

    public enum GFFFieldType
    {
        UInt8,
        Int8,
        UInt16,
        Int16,
        UInt32,
        Int32,
        UInt64,
        Int64,
        Single,
        Double,
        String,
        ResRef,
        LocalizedString,
        Binary,
        Struct,
        List,
        Vector4,
        Vector3,
    }

    public class GFFNavigator
    {
        private object _position;

        public GFFNavigator(GFF gff)
        {
            _position = gff.Root;
        }

        public GFFNavigator(GFFStruct gffStruct)
        {
            _position = gffStruct;
        }

        public GFFNavigator(GFFList gffList)
        {
            _position = gffList;
        }

        public object Resolve()
        {
            return _position;
        }

        public void NavigateTo(string fieldNameOrListIndex)
        {
            if (_position is GFFStruct gffStruct)
            {
                if (gffStruct.Has(fieldNameOrListIndex))
                {
                    _position = gffStruct.Get<object>(fieldNameOrListIndex)!;
                }
                else
                {
                    throw new ArgumentException("Trying to navigate to a field that does not exist.");
                }
            }
            else if (_position is GFFList gffList)
            {
                if (fieldNameOrListIndex.IsInt32())
                {
                    var index = Convert.ToInt32(fieldNameOrListIndex);

                    if (gffList.Count <= index)
                    {
                        throw new ArgumentException("The index specified was greater than the number of structs in the list.");
                    }
                    else
                    {
                        _position = gffList.Get(index);
                    }
                }
                else
                {
                    throw new ArgumentException("Trying to navigate through a list, but a non-integer value was given as an index.");
                }
            }
            else
            {
                throw new ArgumentException("Cannot navigate any further.");
            }
        }
    }
}
