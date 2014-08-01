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
using System.Xml;
using Kooboo.Common.IO;

namespace Kooboo.CMS.Common
{
    public static class XmlSerialization
    {
        #region Serialize
        public static void Serialize<T>(T o, string filePath)
        {
            Serialize(o, new[] { typeof(T) }, filePath);
        }
        public static void Serialize<T>(T o, IEnumerable<Type> knownTypes, string filePath)
        {
            string folderPath = Path.GetDirectoryName(filePath);
            IOUtility.EnsureDirectoryExists(folderPath);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                Serialize<T>(o, knownTypes, stream);
            }
        }

        public static void Serialize<T>(T o, IEnumerable<Type> knownTypes, Stream stream)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(T), knownTypes);
            var settings = new XmlWriterSettings()
            {
                CheckCharacters = false,
                Indent = true,
                IndentChars = "\t"
            };
            using (var writer = XmlWriter.Create(stream, settings))
            {
                ser.WriteObject(writer, o);
            }
        } 
        #endregion

        #region Deserialize
        public static T Deserialize<T>(string filePath)
        {
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
            if (stream.Length > 0)
            {
                try
                {
                    return ser.ReadObject(stream);
                }
                catch (Exception e)
                {
                    Kooboo.Common.Logging.Logger.Error(e.Message, e);
                    return null;
                }
            }
            else
            {
                return null;
            }
        } 
        #endregion
    }
}
