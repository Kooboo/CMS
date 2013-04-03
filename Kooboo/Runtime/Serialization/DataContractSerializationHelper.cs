using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace Kooboo.Runtime.Serialization
{
    public class DataContractSerializationHelper
    {
        public static void Serialize<T>(T o, string filePath)
        {
            string folderPath = Path.GetDirectoryName(filePath);
            Kooboo.IO.IOUtility.EnsureDirectoryExists(folderPath);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                Serialize(o, stream);
            }
        }
        public static void Serialize<T>(T o, Stream stream)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(T));

            var settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t"
            };
            XmlWriter xmlWriter = XmlWriter.Create(stream, settings);
            using (var writer = XmlWriter.Create(xmlWriter, settings))
            {
                ser.WriteObject(writer, o);
            }
        }
        public static T Deserialize<T>(string filePath)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            return (T)Deserialize(typeof(T), new[] { typeof(T) }, filePath);
        }
        public static object Deserialize(Type type, IEnumerable<Type> knownTypes, string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return Deserialize(type, knownTypes, stream);
            }
        }
        public static object Deserialize(Type type, IEnumerable<Type> knownTypes, Stream stream)
        {
            DataContractSerializer ser = new DataContractSerializer(type, knownTypes);
            return ser.ReadObject(stream);
        }

        public static string SerializeAsXml<T>(T o, IEnumerable<Type> knownTypes = null)
        {
            if (knownTypes == null)
            {
                knownTypes = new[] { typeof(T) };
            }
            StringBuilder sb = new StringBuilder();
            using (XmlWriter xmlWriter = XmlWriter.Create(sb))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(T), knownTypes);
                ser.WriteObject(xmlWriter, o);
            }
            return sb.ToString();
        }
        public static T DeserializeFromXml<T>(string xml, IEnumerable<Type> knownTypes = null)
        {
            if (knownTypes == null)
            {
                knownTypes = new[] { typeof(T) };
            }
            using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(T), knownTypes);
                return (T)ser.ReadObject(reader);
            }
        }
    }
}
