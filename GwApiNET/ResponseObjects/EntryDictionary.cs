using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GwApiNET.CacheStrategy;

namespace GwApiNET.ResponseObjects
{
    [ResponseCollection(ResponseCollectionAttribute.CollectionType.IDictionary)]
    [Serializable]
    public class EntryDictionary<TKey, TValue> : ResponseObject, IDictionary<TKey, TValue>, IXmlSerializable
    {
        private ICacheStrategy _cacheStrategy = null;
        public override CacheStrategy.ICacheStrategy CacheStrategy
        {
            get { return _cacheStrategy ?? Constants.GetCacheStrategy(typeof(TValue)); }
            set
            {
                _cacheStrategy = value;
            }
        }
        public EntryDictionary() : this(10)
        {
        }
        public EntryDictionary(IEqualityComparer<TKey> comparer)
        {
            _dictionary = new SerializableDictionary<TKey, TValue>(comparer);
        }
        public EntryDictionary(int size)
        {
            _dictionary = new SerializableDictionary<TKey, TValue>(size);
        }
        private readonly IDictionary<TKey, TValue> _dictionary = new SerializableDictionary<TKey, TValue>();
        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }

        #endregion

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _dictionary.Add(item);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.Remove(item);
        }

        public int Count { get { return _dictionary.Count; } }
        public bool IsReadOnly { get { return _dictionary.IsReadOnly; } }
        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
        }

        public bool Remove(TKey key)
        {
            return _dictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get
            {
                try
                {
                    return _dictionary[key];
                }
                catch (KeyNotFoundException e)
                {
                    Debug.WriteLine(string.Format("Key not found {0}: {1}", key, typeof (TValue)));
                    throw;
                }
            }
            set { _dictionary[key] = value; }
        }

        public ICollection<TKey> Keys { get { return _dictionary.Keys; } }
        public ICollection<TValue> Values { get { return _dictionary.Values; } }

        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            if (items == null) return;
            foreach (var item in items)
                Add(item);
        }
    }
}
