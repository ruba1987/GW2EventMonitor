using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GwApiNET.CacheStrategy;

namespace GwApiNET.ResponseObjects
{
    [Serializable]
    [ResponseCollection(ResponseCollectionAttribute.CollectionType.IList)]
    public class EntryCollection<T> : ResponseObject, IList<T> where T : ResponseObject
    {
        private readonly List<T> _entries;

        private ICacheStrategy _cacheStrategy = null;
        public override CacheStrategy.ICacheStrategy CacheStrategy
        {
            get { return _cacheStrategy ?? Constants.GetCacheStrategy(typeof(T)); }
            set
            {
                _cacheStrategy = value;
            }
        }

        public EntryCollection()
            : this(10)
        {
            
        }

        public EntryCollection(int initialSize) : this(new List<T>(initialSize))
        {
            _entries = new List<T>(initialSize);
        }

        public EntryCollection(IEnumerable<T> collection)
        {
            _entries = new List<T>(collection);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            _entries.Add(item);
        }

        public void Clear()
        {
            _entries.Clear();
        }

        public bool Contains(T item)
        {
            return _entries.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _entries.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return _entries.Remove(item);
        }

        public int Count { get { return _entries.Count; } }
        public bool IsReadOnly { get { return false; } }
        public int IndexOf(T item)
        {
            return _entries.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _entries.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _entries.RemoveAt(index);
        }

        public T this[int index]
        {
            get { return _entries[index]; }
            set { _entries[index] = value; }
        }
    }
}
