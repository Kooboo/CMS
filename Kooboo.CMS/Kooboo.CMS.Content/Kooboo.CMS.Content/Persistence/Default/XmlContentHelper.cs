#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Kooboo.CMS.Content.Persistence.Default
{
    internal static class XmlContentHelper
    {
        #region locker
        private static System.Threading.ReaderWriterLockSlim contentLocker = new System.Threading.ReaderWriterLockSlim();
        private static System.Threading.ReaderWriterLockSlim categoryLocker = new System.Threading.ReaderWriterLockSlim();

        #endregion

        #region GetContentFile
        public static string GetContentFile(this Schema schema)
        {
            DataPath dataPath = new DataPath(schema.Repository.Name);
            return Path.Combine(dataPath.PhysicalPath, schema.Name + ".xml");
        } 
        #endregion

        #region GetCategoryDataFile
        public static string GetCategoryDataFile(this Repository repository)
        {
            DataPath dataPath = new DataPath(repository.Name);
            return Path.Combine(dataPath.PhysicalPath, "_category_" + ".xml");
        } 
        #endregion

        #region GetContents
        public static List<TextContent> GetContents(this Schema schema)
        {
            var dataFile = schema.GetContentFile();
            contentLocker.EnterReadLock();
            try
            {
                var contents = InternalGetContents(dataFile);
                foreach (var item in contents)
                {
                    item["Repository"] = schema.Repository.Name;
                }
                return contents.Select(it => new TextContent(it).ConvertToLocalTime()).ToList();
            }
            finally
            {
                contentLocker.ExitReadLock();
            }

        }
        private static IEnumerable<Dictionary<string, object>> InternalGetContents(string dataFile)
        {
            var result = new List<Dictionary<string, object>>();
            if (File.Exists(dataFile))
            {

                using (FileStream fs = new FileStream(dataFile, FileMode.Open, FileAccess.Read))
                {
                    var ser = GetContentSerializer();
                    result = (List<Dictionary<string, object>>)(ser.ReadObject(fs));
                }
            }
            return result;
        }
        #endregion

        #region SaveContents
        public static void SaveContents(this Schema schema, List<TextContent> data)
        {
            var dataFile = schema.GetContentFile();
            contentLocker.EnterWriteLock();
            try
            {
                InternalSaveContents(data.Select(it => new Dictionary<string, object>(it)), dataFile);
            }
            finally
            {
                contentLocker.ExitWriteLock();
            }
        }

        private static void InternalSaveContents(IEnumerable<Dictionary<string, object>> data, string dataFile)
        {
            var ser = GetContentSerializer();
            string folderPath = Path.GetDirectoryName(dataFile);
            Kooboo.IO.IOUtility.EnsureDirectoryExists(folderPath);
            using (FileStream stream = new FileStream(dataFile, FileMode.Create))
            {
                ser.WriteObject(stream, data.ToList());
            }
        }

        #endregion

        #region GetCategoryData
        public static List<Category> GetCategoryData(this Repository repository)
        {
            var dataFile = repository.GetCategoryDataFile();
            categoryLocker.EnterReadLock();
            try
            {
                if (!File.Exists(dataFile))
                {
                    return new List<Category>();
                }
                return Serialization.Deserialize<Category[]>(dataFile, new[] { typeof(Category) }).ToList();
            }
            finally
            {
                categoryLocker.ExitReadLock();
            }
        }

        
        #endregion

        #region SaveCategoryData
        public static void SaveCategoryData(this Repository repository, List<Category> data)
        {
            var dataFile = repository.GetCategoryDataFile();
            categoryLocker.EnterWriteLock();
            try
            {
                Serialization.Serialize(data, dataFile);
            }
            finally
            {
                categoryLocker.ExitWriteLock();
            }
        } 
        #endregion

        #region GetContentSerializer
        private static DataContractSerializer GetContentSerializer()
        {
            return new DataContractSerializer(typeof(List<Dictionary<string, object>>), "ArrayOfArrayOfKeyValueOfstringanyType", "http://schemas.microsoft.com/2003/10/Serialization/Arrays", new Type[] { typeof(Dictionary<string, object>) });
        } 
        #endregion
    }
}
