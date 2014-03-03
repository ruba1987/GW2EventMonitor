using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using GwApiNET.ResponseObjects;

namespace GwApiNET
{

    /// <summary>
    /// Provides common XML functions
    /// </summary>
    public class XmlUtilities
    {
        /// <summary>
        /// Serialize object of type T into an xml string
        /// </summary>
        /// <typeparam name="T">type of <paramref name="item"/></typeparam>
        /// <param name="item">item to serialize</param>
        /// <param name="extraTypes">A <see cref="System.Type"/> array of additional object types to serialize.</param>
        /// <returns>xml string representation of <paramref name="item"/></returns>
        public static string XmlSerialize<T>(T item, Type[] extraTypes = null)
        {
            if (item == null) return string.Empty;
            XmlSerializer serializer = extraTypes == null
                                           ? new XmlSerializer(typeof (T))
                                           : new XmlSerializer(typeof (T), extraTypes);
            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, item);
                writer.Close();
            }
            return sb.ToString();
        }

        /// <summary>
        /// Serialize object to an xml string
        /// </summary>
        /// <param name="item">item to serialize</param>
        /// <returns>xml string representation of <paramref name="item"/></returns>
        public static string XmlSerialize(object item)
        {
            if (item == null) return string.Empty;
            XmlSerializer serializer = new XmlSerializer(item.GetType());
            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, item);
                writer.Close();
            }
            return sb.ToString();
        }

        /// <summary>
        /// Deserialize an xml string into an object
        /// <remarks>This function is the inverse of <see cref="XmlUtilities.XmlSerialize(object)"/></remarks>
        /// </summary>
        /// <typeparam name="T"><see cref="Type"/> of object to return</typeparam>
        /// <param name="xml">xml data serialized via <see cref="XmlSerializer"/></param>
        /// <returns>Deserialized object of type T</returns>
        public static T XmlDeserialize<T>(string xml)
        {
            return (T)XmlDeserialize(xml, typeof(T));
        }
        /// <summary>
        /// Deserialize an xml string into an object
        /// <remarks>This function is the inverse of <see cref="XmlUtilities.XmlSerialize(object)"/></remarks>
        /// </summary>
        /// <param name="xml">xml data serialized via <see cref="XmlSerializer"/></param>
        /// <param name="type">object type being deserialized</param>
        /// <returns>Deserialized object of <paramref name="type"/></returns>
        public static object XmlDeserialize(string xml, Type type)
        {
            if (xml == string.Empty) return null;
            object item = null;
            XmlSerializer deserializer = new XmlSerializer(type);
            using (StringReader reader = new StringReader(xml))
            {
                try
                {
                    item = deserializer.Deserialize(reader);
                }
                catch (InvalidOperationException)
                {
                    item = null;
                }
            }
            return item;
        }

        public static string DataContractSerializer(object obj)
        {
            return DataContractSerializer(obj, new List<Type>());
        }

        public static string DataContractSerializer(object obj, IEnumerable<Type> knownTypes)
        {
            DataContractSerializer serializer = new DataContractSerializer(obj.GetType(), knownTypes);
            StringBuilder sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb))
            {
                serializer.WriteObject(writer, obj);
                writer.Flush();
                return sb.ToString();
            }
        }

        public static T DataContractDeserializer<T>(string data)
        {
            return DataContractDeserializer<T>(data, new List<Type>());
        }

        public static T DataContractDeserializer<T>(string data, IEnumerable<Type> knownTypes)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T), knownTypes);
            using (StringReader sReader = new StringReader(data))
            {
                using (XmlReader reader = XmlReader.Create(sReader))
                {
                    var obj = (T)serializer.ReadObject(reader);
                    return obj;
                }
            }
        }
    }

    public static class BinarySerializer
    {
        public static byte[] BinarySerialize(object o)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            byte[] serialized;
            using (MemoryStream s = new MemoryStream(1024))
            {
                formatter.Serialize(s, o);
                serialized = s.ToArray();
            }
            return serialized;
        }

        public static T BinaryDeserialize<T>(byte[] serializedObject)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(serializedObject))
            {
                T o = (T)formatter.Deserialize(stream);
                return o;
            }
        }

        public static T BinaryDeserialize<T>(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            T o = (T) formatter.Deserialize(stream);
            return o;
        }
    }

}
