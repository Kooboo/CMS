#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using Kooboo.CMS.Sites.Models;
using System.Xml;

namespace Kooboo.CMS.Sites.Persistence
{
    public static class Serialization
    {
        public static T DeserializeSettings<T>(string filePath)
        {
            return Deserialize<T>(filePath);
        }
        public static void Serialize<T>(T o, string filePath)
        {
            Serialize(o, new[] { typeof(T) }, filePath);
        }
        public static void Serialize<T>(T o, IEnumerable<Type> knownTypes, string filePath)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(T), knownTypes);
            string folderPath = Path.GetDirectoryName(filePath);
            Kooboo.IO.IOUtility.EnsureDirectoryExists(folderPath);
            var settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t"
            };
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                XmlWriter xmlWriter = XmlWriter.Create(stream, settings);
                using (var writer = XmlWriter.Create(xmlWriter, settings))
                {
                    ser.WriteObject(writer, o);
                }
            }
        }
        public static T Deserialize<T>(string filePath)
        {
            return (T)Deserialize(typeof(T), new[] { typeof(T) }, filePath);
        }
        public static object Deserialize(Type type, IEnumerable<Type> knownTypes, string filePath)
        {
            DataContractSerializer ser = new DataContractSerializer(type, knownTypes);
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (stream.Length > 0)
                {
                    return ser.ReadObject(stream);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
